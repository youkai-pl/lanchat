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

	//kick
	kick: function (args) {
		if (!global.server_status) {
			out.alert("You not a host")
		} else {
			//check user
			var index = global.users.findIndex(x => x.nickname === args)
			if (!global.users[index]) {
				out.alert("This user not exist")
			} else {
				//kick user
				out.status("Kicked " + global.users[index].nickname)
				io.sockets.connected[global.users[index].id].disconnect()
			}
		}
	}
}

//ALL SERVER THINGS
function run() {
	io.on("connection", function (socket) {

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
			//detect already used nick
			var index2 = global.users.findIndex(x => x.nickname === nick)
			if (index2 !== -1) {
				nick = nick + global.users.length
			}
			//create user objcet
			var user = {
				id: socket.id,
				nickname: nick,
				status: "online",
				ip: socket.handshake.address
			}
			//add user to array
			global.users.push(user)
			socket.broadcast.emit("status", {
				content: "join the chat",
				nick: nick
			})
			//emit motd
			if (motd) {
				socket.emit("motd", motd)
			}
			return
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
			socket.broadcast.emit("status", {
				content: "left the chat",
				nick: global.users[index].nickname
			})
			//delete user from table
			global.users.splice(global.users.indexOf(index), 1)
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
			var index2 = global.users.findIndex(x => x.nickname === nick)
			if (index2 !== -1) {
				nick = nick + index
			}
			var old = global.users[index].nickname
			//save new nick
			global.users[index].nickname = nick
			//send return to user
			socket.broadcast.emit("return", old.blue + " change nick to " + nick.blue)
			socket.emit("nick", nick)

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
			socket.broadcast.emit("status", {
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
			socket.broadcast.emit("status", {
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
			socket.broadcast.emit("status", {
				content: "is dnd",
				nick: global.users[index].nickname
			})

		})

		//message
		socket.on("message", async (content) => {
			//find user
			var index = global.users.findIndex(x => x.id === socket.id)
			//error handle
			if (!global.users[index]) {
				return
			}
			try {
				//flood block
				await rateLimiter.consume(socket.handshake.address)
				//validate message
				if (content) {
					//check lenght
					if (content.length > 1500) {
						socket.emit("return", "FLOOD BLOCKED".red)
					} else {
						//emit message
						socket.broadcast.emit("message", {
							nick: global.users[index].nickname,
							content: content
						})
					}
				}
			} catch (rejRes) {
				//emit alert
				socket.emit("return", "FLOOD BLOCKED".red)
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

		//long list
		socket.on("long_list", function () {
			var list = global.users
			socket.emit("return", list)
		})

		//return
		socket.on("return", function (msg) {
			socket.emit("return", msg)
		})
	})
}