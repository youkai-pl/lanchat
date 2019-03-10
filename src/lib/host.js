//import
const fn = require("./common")
const out = require("./out")
const colors = require("colors")
const client = require("./client")
const http = require("http").Server()
const io = require("socket.io")(http)
const { RateLimiterMemory } = require("rate-limiter-flexible")
var settings = require("./settings")
var global = require("./global")
var rateLimiter

//variavles
var motd

//HOST
module.exports = {
	//create host
	start: function () {

		//load db
		settings.loadDb()

		//check database
		var index = global.db.findIndex(x => x.nickname === global.nick)

		//create database index
		if (index === -1) {
			settings.createDb(global.nick)
		}

		//write permission to database
		settings.writedb(global.nick, "level", 5)

		//rate limiter init
		rateLimiter = new RateLimiterMemory(
			{
				points: global.rateLimit,
				duration: 1,
			})

		out.status("starting server")

		//load motd
		motd = global.motd
		if (!motd) {
			out.status("motd not found")
		}

		//check port
		fn.testPort(global.port, "127.0.0.1", function (e) {
			if (e === "failure") {
				http.listen(global.port, function () {
					out.status("server running")
				})
				//start host
				run()
				global.server_status = true
				client.connect("localhost")
			} else {
				out.alert("Server is already running on this PC")
			}
		})
	},
}

//ALL SERVER THINGS
function run() {
	io.on("connection", function (socket) {

		//BASIC ACTIONS
		//login
		socket.on("login", function (nick) {

			//detect blank nick
			if (!nick) {
				nick = "default"
			}

			//shortening long nick
			if (nick.length > 15) {
				nick = nick.substring(0, 15)
			}

			//check database
			var index = global.db.findIndex(x => x.nickname === nick)

			//create database index
			if (index === -1) {
				settings.createDb(nick)
			}

			//update index
			index = global.db.findIndex(x => x.nickname === nick)

			//add keys
			if (!global.db[index].hasOwnProperty("level")) {
				settings.writedb(nick, "level", 2)
			}
			if (!global.db[index].hasOwnProperty("ip")) {
				settings.writedb(nick, "ip", socket.handshake.address)
			}
			if (!global.db[index].hasOwnProperty("lock")) {
				settings.writedb(nick, "lock", false)
			}
			if (!global.db[index].hasOwnProperty("pass")) {
				settings.writedb(nick, "pass", false)
			}

			//check ban

			if (global.db[index].level === 0) {

				//return
				socket.emit("return", "BANNED")
				io.sockets.connected[socket.id].disconnect()
			} else {

				//check lock
				if (global.db[index].lock) {
					//return
					socket.emit("return", "Account locked\nLogin with /login <password>")

				} else {
					auth(nick, socket)
				}
			}
		})

		//logoff
		socket.on("disconnect", function () {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			//emit status
			socket.broadcast.to("main").emit("status", {
				content: "left the chat",
				nick: global.users[index].nickname
			})
			//delete user from table
			global.users.splice(global.users.indexOf(index), 1)
		})

		//auth
		socket.on("auth", function (nick, password) {
			var index = global.db.findIndex(x => x.nickname === nick)
			if (password === global.db[index].pass) {
				auth(nick, socket)
				//return
				socket.emit("return", "Logged!")
			} else {
				socket.emit("return", "Wrong password")
			}
		})

		//register
		socket.on("register", function (nick, password) {
			var index = global.users.findIndex(x => x.nickname === nick)
			if (index !== -1) {
				settings.writedb(nick, "lock", true)
				settings.writedb(nick, "pass", password)
				socket.emit("return", "Done")
			} else {
				socket.emit("return", "Login first")
			}
		})

		//USER ACTIONS
		//message
		socket.on("message", async (content) => {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			if (content) {
				//check lenght
				try {
					//flood block
					await rateLimiter.consume(socket.handshake.address)
					//validate message

					if (content.length > 1500) {
						socket.emit("return", "FLOOD BLOCKED")
					} else {
						//emit message
						socket.broadcast.to("main").emit("message", {
							nick: global.users[index].nickname,
							content: content
						})
					}

				} catch (rejRes) {
					//emit alert
					socket.emit("return", "FLOOD BLOCKED")
				}
			}
		})

		//mention
		socket.on("mention", function (nick) {
			//find user
			var index = global.users.findIndex(x => x.nickname === nick)
			//find user
			var index2 = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index2]) {
				return
			}
			if (global.users[index]) {
				//send mention
				socket.to(`${global.users[index].id}`).emit("mentioned", {
					nick: global.users[index2].nickname,
					content: "mentioned you"
				})
			} else {
				//send return
				socket.emit("return", "This user not exist")
			}
		})

		//change nick
		socket.on("nick", function (nick) {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			//shorten the long nick
			if (nick.length > 15) {
				nick = nick.substring(0, 15)
			}
			//check is nick already used
			var index2 = global.db.findIndex(x => x.nickname === nick)
			if (index2 !== -1) {
				socket.emit("return", "nick already used on this server")
			} else {
				var old = global.users[index].nickname
				//save new nick
				global.users[index].nickname = nick
				settings.writedb(old, "nickname", nick)
				//send return to user
				socket.broadcast.to("main").emit("return", old.blue + " change nick to " + nick.blue)
				socket.emit("nick", nick)
			}
		})

		//list
		socket.on("list", function () {
			//crate user list
			var list = []
			var status
			list[0] = "\nUser List:"
			for (i = 1; i < global.users.length + 1; i++) {
				var a = global.users[i - 1]
				if (a.status === "online") {
					status = a.status.green
				}
				if (a.status === "afk") {
					status = a.status.yellow
				}
				if (a.status === "dnd") {
					status = a.status.red
				}
				list[i] = a.nickname.blue + " (" + status + ")"
			}

			//emit table
			var table = list.join("\n")
			socket.emit("return", table)

		})

		//STATUS CHANGE
		//afk
		socket.on("afk", function () {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			//change user status
			global.users[index].status = "afk"
			//emit status
			socket.broadcast.to("main").emit("status", {
				content: "is afk",
				nick: global.users[index].nickname
			})
		})

		//online
		socket.on("online", function () {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			//change user status
			global.users[index].status = "online"
			//emit status
			socket.broadcast.to("main").emit("status", {
				content: "is online",
				nick: global.users[index].nickname
			})

		})

		//dnd
		socket.on("dnd", function () {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			//change user status
			global.users[index].status = "dnd"
			//emit status
			socket.broadcast.to("main").emit("status", {
				content: "is dnd",
				nick: global.users[index].nickname
			})

		})

		//MODERATION
		//kick
		socket.on("kick", function (nick, arg) {
			var index = global.users.findIndex(x => x.nickname === nick)
			if (index !== -1) {
				var index2 = global.db.findIndex(x => x.nickname === nick)
				if (global.db[index2].level < 4) {
					socket.emit("return", "You not have permission")
				} else {
					//check user
					var index3 = global.users.findIndex(x => x.nickname === arg)
					if (!global.users[index3]) {
						socket.emit("return", "This user not exist")
					} else {
						//kick user
						socket.emit("return", "Kicked " + global.users[index3].nickname)
						io.sockets.connected[global.users[index3].id].disconnect()
					}
				}
			}
		})

		//ban
		socket.on("ban", function (nick, arg) {
			//find user
			var index = global.users.findIndex(x => x.nickname === nick)
			if (index !== -1) {
				//find user
				var index2 = global.db.findIndex(x => x.nickname === nick)
				//find user
				var index3 = global.db.findIndex(x => x.nickname === arg)
				if ((global.db[index2].level < 4) || (global.db[index2].level < global.db[index3].level)) {
					socket.emit("return", "You not have permission")
				} else {
					if (!global.db[index3]) {
						socket.emit("return", "This user not exist")
					} else {
						//ban user
						socket.emit("return", "Banned " + global.users[index3].nickname)
						settings.writedb(arg, "level", 0)
						io.sockets.connected[global.users[index3].id].disconnect()
					}
				}
			}
		})

		//unban
		socket.on("unban", function (nick, arg) {
			//find user
			var index = global.users.findIndex(x => x.nickname === nick)
			if (index !== -1) {
				//find user
				var index2 = global.db.findIndex(x => x.nickname === nick)
				//find user
				var index3 = global.db.findIndex(x => x.nickname === arg)
				if (global.db[index2].level < 4) {
					socket.emit("return", "You not have permission")
				} else {
					if (!global.db[index3]) {
						socket.emit("return", "This user not exist")
					} else {
						//ban user
						socket.emit("return", "Unbanned " + global.db[index3].nickname)
						settings.writedb(arg, "level", 2)
					}
				}
			}
		})

		//DEV
		//long list
		socket.on("long_list", function () {
			var list = global.users
			socket.emit("return", list)
		})
	})
}

//FUNCIONS
//Auth
function auth(nick, socket) {

	//check connected list
	if (global.users.findIndex(x => x.nickname === nick) !== -1) {

		//return
		socket.emit("return", "User already connected\nChange nick and try again")
		io.sockets.connected[socket.id].disconnect()
	} else {

		//create user objcet
		var user = {
			id: socket.id,
			nickname: nick,
			status: "online",
			ip: socket.handshake.address
		}

		//add user to array
		global.users.push(user)

		//broadcast status
		socket.broadcast.to("main").emit("status", {
			content: "join the chat",
			nick: nick
		})

		//emit motd
		if (motd) {
			socket.emit("motd", motd)
		}

		//join
		socket.join("main")

		return
	}
}