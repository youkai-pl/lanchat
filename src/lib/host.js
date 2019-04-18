//import
const out = require("./out")
const client = require("./client")
const net = require("net")
const http = require("http").Server()
const io = require("socket.io")(http)
const { RateLimiterMemory } = require("rate-limiter-flexible")
const db = require("./db")
const config = require("./config")
var global = require("./global")

var database
users = []

//HOST
module.exports = {

	//create host
	host: function () {

		//load db
		database = db.load()

		//check database
		var index = database.findIndex(x => x.nickname === config.nick)

		//create database index
		if (index === -1) {
			db.add(config.nick)
		}

		//write permission to database
		db.write(config.nick, "level", 5)

		//rate limiter init
		rateLimiter = new RateLimiterMemory(
			{
				points: config.ratelimit,
				duration: 1,
			})

		out.status("starting server")

		//check motd
		if (!config.motd) {
			out.status("motd not found")
		}

		//check port
		testPort(config.port, "127.0.0.1", function (e) {
			if (e === "failure") {
				http.listen(config.port, function () {
					out.status("server running")
				})
				//start host
				run()
				global.server_status = true
				client.connect("localhost")
			} else {
				out.alert("Server is already running on this PC")
				return
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
			var index = database.findIndex(x => x.nickname === nick)

			//create database index
			if (index === -1) {
				db.add(nick)
			}

			//update index
			index = database.findIndex(x => x.nickname === nick)

			//add keys
			if (!database[index].hasOwnProperty("level")) {
				db.write(nick, "level", 2)
			}
			if (!database[index].hasOwnProperty("ip")) {
				db.write(nick, "ip", socket.handshake.address)
			}
			if (!database[index].hasOwnProperty("lock")) {
				db.write(nick, "lock", false)
			}
			if (!database[index].hasOwnProperty("pass")) {
				db.write(nick, "pass", false)
			}

			//ban check
			var ban

			//chekc via ip
			for (var i = 0; i < database.length; i++) {
				if (database[i].level === 0) {
					ban = true
				}
			}

			//check via nick
			if (database[index].level === 0) {
				ban = true
			}

			//handle banned user
			if (ban) {

				//return
				socket.emit("rcode", "005")
				socket.emit("return", "BANNED")
				io.sockets.connected[socket.id].disconnect()
			} else {

				//check lock
				if (database[index].lock) {

					//return
					socket.emit("rcode", "006")
					socket.emit("return", "Account locked\nLogin with /login <password>")
				} else {
					auth(nick, socket)
				}
			}
		})

		//logoff
		socket.on("disconnect", function () {

			//find user
			var index = users.findIndex(x => x.id === socket.id)

			//if user authorized
			if (index !== -1) {

				//emit status
				socket.broadcast.to("main").emit("status", {
					content: "left the chat",
					nick: users[index].nickname
				})

				//delete user from table
				users.splice(users.indexOf(index), 1)
			}
		})

		//auth
		socket.on("auth", function (nick, password) {
			var index = database.findIndex(x => x.nickname === nick)
			if (users.findIndex(x => x.id === socket.id) !== -1) {
				socket.emit("rcode", "007")
				socket.emit("return", "You are already logged")
			} else {
				if (password === database[index].pass) {
					auth(nick, socket)
				} else {
					socket.emit("rcode", "008")
					socket.emit("return", "Wrong password")
				}
			}
		})

		//register
		socket.on("register", function (nick, password) {
			var index = users.findIndex(x => x.nickname === nick)
			if (password) {
				if (index !== -1) {
					db.write(nick, "lock", true)
					db.write(nick, "pass", password)
					socket.emit("rcode", "009")
					socket.emit("return", "Done")
				}
			} else {
				socket.emit("rcode", "010")
				socket.emit("return", "Password cannot be blank")
			}
		})

		//USER ACTIONS
		//message
		socket.on("message", async (content) => {

			//validate message
			if (typeof content === "string" || content instanceof String) {
				//find user
				var index = users.findIndex(x => x.id === socket.id)

				//if user loggined
				if (users[index]) {

					//get user from db
					var index2 = database.findIndex(x => x.nickname === users[index].nickname)

					//check mute
					if (database[index2].level === 1) {
						socket.emit("rcode", "011")
						socket.emit("return", "muted")
					} else {
						if (content) {
							//check lenght
							try {
								//flood block
								await rateLimiter.consume(socket.handshake.address)
								//validate message

								//block long messages
								if (content.length > 1500) {
									socket.emit("rcode", "012")
									socket.emit("return", "FLOOD BLOCKED")
								} else {
									//emit message
									socket.broadcast.to("main").emit("message", {
										nick: users[index].nickname,
										content: content
									})
								}

							} catch (rejRes) {

								//emit alert
								socket.emit("rcode", "004")
								socket.emit("return", "HOST: FLOOD BLOCKED")
							}
						} else {
							//emit blank message error
							socket.emit("rcode", "001")
						}
					}
				}
			} else {
				//disconnect user with wrong client
				io.sockets.connected[socket.id].disconnect()
			}
		})

		//mention
		socket.on("mention", function (nick) {
			//find user
			var selected = users.findIndex(x => x.nickname === nick)
			//find user
			var index = users.findIndex(x => x.id === socket.id)
			//if user exist
			if (users[selected]) {
				//send mention
				socket.to(`${users[selected].id}`).emit("mentioned", users[index].nickname)
			} else {
				//send return
				socket.emit("rcode", "002")
				socket.emit("return", "This user not exist")
			}
		})

		//change nick
		socket.on("nick", function (nick) {

			//find user
			var index = users.findIndex(x => x.id === socket.id)

			//shorten the long nick
			if (nick.length > 15) {
				nick = nick.substring(0, 15)
			}

			//check is nick already used
			if (database.findIndex(x => x.nickname === nick) !== -1) {
				socket.emit("rcode", "003")
				socket.emit("return", "nick already used on this server")
			} else {

				//get old nick
				var old = users[index].nickname

				//save new nick
				users[index].nickname = nick
				db.write(old, "nickname", nick)

				//send return to user
				socket.broadcast.to("main").emit("return", old + " change nick to " + nick)
			}
		})

		//list
		socket.on("list", function () {

			//crate user list
			var list = []
			list[0] = "\nUser List:"

			//add users to table
			for (i = 1; i < users.length + 1; i++) {
				var a = users[i - 1]

				list[i] = a.nickname + " (" + a.status + ")"
			}

			//emit table
			socket.emit("return", list.join("\n"))

		})

		//change status
		socket.on("changeStatus", function (value) {
			if (value === "online" || value === "dnd" || value === "afk") {
				//find user
				var index = users.findIndex(x => x.id === socket.id)
				//change user status
				users[index].status = value
				//emit status
				socket.broadcast.to("main").emit("status", {
					content: "is " + value,
					nick: users[index].nickname
				})
			} else {
				socket.emit("rcode", "001")
			}
		})

		//MODERATION
		//kick
		socket.on("kick", function (arg) {
			var index = users.findIndex(x => x.id === socket.id)
			if (index !== -1) {
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				if (database[index2].level < 3) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					//check user
					var index3 = users.findIndex(x => x.nickname === arg)
					if (!users[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						//kick user
						socket.emit("return", "Kicked " + users[index3].nickname)
						io.sockets.connected[users[index3].id].disconnect()
					}
				}
			}
		})

		//ban
		socket.on("ban", function (arg) {
			//find user
			var index = users.findIndex(x => x.id === socket.id)
			if (index !== -1) {
				//find user
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				//find user
				var index3 = database.findIndex(x => x.nickname === arg)
				if ((database[index2].level < 3) || (database[index2].level < database[index3].level)) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					if (!database[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						//ban user
						socket.emit("return", "Banned " + users[index3].nickname)
						db.write(arg, "level", 0)
						io.sockets.connected[users[index3].id].disconnect()
					}
				}
			}
		})

		//unban
		socket.on("unban", function (arg) {
			//find user
			var index = users.findIndex(x => x.id === socket.id)
			if (index !== -1) {
				//find user
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				//find user
				var index3 = database.findIndex(x => x.nickname === arg)
				if (database[index2].level < 3) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					if (!database[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						//ban user
						socket.emit("return", "Unbanned " + database[index3].nickname)
						db.write(arg, "level", 2)
					}
				}
			}
		})

		//mute
		socket.on("mute", function (arg) {
			//find user
			var index = users.findIndex(x => x.id === socket.id)
			if (index !== -1) {
				//find user
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				//find user
				var index3 = database.findIndex(x => x.nickname === arg)
				if ((database[index2].level < 3) || (database[index2].level < database[index3].level)) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					if (!database[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						//mute user
						socket.emit("return", "Muted " + database[index3].nickname)
						db.write(arg, "level", 1)
					}
				}
			}
		})

		//unmute
		socket.on("unmute", function (arg) {

			//find user
			var index = users.findIndex(x => x.id === socket.id)
			if (index !== -1) {
				//find user
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				//find user
				var index3 = database.findIndex(x => x.nickname === arg)
				if (database[index2].level < 3) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					if (!database[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						//mute user
						socket.emit("return", "Unmuted " + database[index3].nickname)
						db.write(arg, "level", 2)
					}
				}
			}
		})

		//change permission level
		socket.on("level", function (nick, arg) {
			//find user
			var index = users.findIndex(x => x.nickname === socket.id)
			if (index !== -1) {
				//find user
				var index2 = database.findIndex(x => x.nickname === users[index].nickname)
				//find user
				var index3 = database.findIndex(x => x.nickname === arg[0])
				if ((database[index2].level < 4) || (database[index2].level < database[index3].level)) {
					socket.emit("rcode", "013")
					socket.emit("return", "You not have permission")
				} else {
					if (!database[index3]) {
						socket.emit("rcode", "002")
						socket.emit("return", "This user not exist")
					} else {
						if (arg[1] >= 0 && arg[1] <= 4) {
							//change permission
							socket.emit("return", "Updated permission for " + database[index3].nickname)
							db.write(arg[0], "level", Number(arg[1]))
						} else {
							socket.emit("rcode", "014")
							socket.emit("return", "Bad permission ID")
						}
					}
				}
			}
		})
	})
}

//FUNCIONS
//Auth
function auth(nick, socket) {

	//check connected list
	if (users.findIndex(x => x.nickname === nick) !== -1) {

		socket.emit("rcode", "003")
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
		users.push(user)

		//broadcast status
		socket.broadcast.to("main").emit("status", {
			content: "join the chat",
			nick: nick
		})

		//emit logged
		socket.emit("rcode", "015")

		//emit motd
		if (config.motd) {
			socket.emit("motd", config.motd)
		}

		//join
		socket.join("main")

		return
	}
}

//test port
function testPort(port, host, cb) {
	var client = net.createConnection(port, host).on("connect", function (e) {
		cb("success", e)
		client.destroy()
	}).on("error", function (e) {
		cb("failure", e)
	})
}