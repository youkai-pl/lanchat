const fn = require("./common")
const out = require("./out")
const colors = require("colors")
const client = require("./client")
const http = require("http").Server()
const io = require("socket.io")(http)
const { RateLimiterMemory } = require("rate-limiter-flexible")
var settings = require("./settings")
var global = require("./global")
var motd

const rateLimiter = new RateLimiterMemory(
	{
		points: 10,
		duration: 1,
	})

//HOST
module.exports = {
	start: function () {
		out.status("starting server")
		motd = settings.motd
		if (!motd) {
			out.status("motd not found")
		}
		fn.testPort(settings.port, "127.0.0.1", function (e) {
			if (e === "failure") {
				http.listen(settings.port, function () {
					out.status("server running")
				})
				run()
				global.server_status = true
				client.connect("localhost")
			} else {
				out.alert("Server is already running on this PC")
			}
		})
	}
}

//ALL SERVER THINGS
function run() {

	io.on("connection", function (socket) {
		//login
		socket.on("login", function (nick) {
			//detect blank nick
			if (!nick) {
				nick = "blank"
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
			var user = {
				id: socket.id,
				nickname: nick,
				status: "online",
				ip: socket.handshake.address
			}
			global.users.push(user)
			socket.broadcast.emit("status", {
				content: "join the chat",
				nick: nick
			})
			if (motd) {
				socket.emit("motd", motd)
			}
		})

		//logoff
		socket.on("disconnect", function () {
			var index = global.users.findIndex(x => x.id === socket.id)
			if (!global.users[index]) {
				return
			}
			socket.broadcast.emit("status", {
				content: "left the chat",
				nick: global.users[index].nickname
			})
			global.users.splice(global.users.indexOf(index), 1)
		})

		//change nick
		socket.on("nick", function (nick) {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				if (nick.length > 15) {
					nick = nick.substring(0, 15)
				}
				var index2 = global.users.findIndex(x => x.nickname === nick)
				if (index2 !== -1) {
					nick = nick + index
				}
				var old = global.users[index].nickname
				global.users[index].nickname = nick
				socket.broadcast.emit("return", old.blue + " change nick to " + nick.blue)
				socket.emit("nick", nick)
			}
		})

		//list
		socket.on("list", function () {
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

			var table = list.join("\n")
			socket.emit("return", table)

		})

		//afk
		socket.on("afk", function () {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "afk"
				var msg = {
					content: "is afk",
					nick: global.users[index].nickname
				}
				socket.broadcast.emit("status", msg)
			}

		})

		//online
		socket.on("online", function () {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "online"
				var msg = {
					content: "is online",
					nick: global.users[index].nickname
				}
				socket.broadcast.emit("status", msg)
			}
		})

		//dnd
		socket.on("dnd", function () {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "dnd"
				var msg = {
					content: "is dnd",
					nick: global.users[index].nickname
				}
				socket.broadcast.emit("status", msg)
			}
		})

		//message
		socket.on("message", async (content) => {
			var index = global.users.findIndex(x => x.id === socket.id)
			if (!global.users[index]) {
				return
			}
			try {
				await rateLimiter.consume(socket.handshake.address)
				if (content.length > 1500) {
					content = content.substring(0, 1500)
				}
				if (content) {
					var msg = {
						nick: global.users[index].nickname,
						content: content
					}
					socket.broadcast.emit("message", msg)
				}
			} catch (rejRes) {
				socket.emit("return", "FLOOD BLOCKED")
			}
		})

		//mention
		socket.on("mention", function (nick) {
			var index = global.users.findIndex(x => x.nickname === nick)
			var index2 = global.users.findIndex(x => x.id === socket.id)
			if (global.users[index]) {
				var msg = {
					nick: global.users[index2].nickname,
					content: "mentioned you"
				}
				socket.to(`${global.users[index].id}`).emit("mentioned", msg)
			} else {
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