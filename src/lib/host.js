//HOST
const fn = require("./common")
const out = require("./out")
const colors = require("colors")
const client = require("./client")
const http = require("http").Server()
const io = require("socket.io")(http)
const shortid = require("shortid")
var settings = require("./settings")
var global = require("./global")

module.exports = {
	start: function () {
		out.status("starting server")
		fn.testPort(settings.port, "127.0.0.1", function (e) {
			if (e === "failure") {
				http.listen(settings.port, function () {
					out.status("Server running")
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
			if (typeof nick === "undefined" || nick === "") {
				nick = "blank"
			}
			if (nick.length > 15) {
				nick = nick.substring(0, 15)
			}
			var index2 = global.users.findIndex(x => x.nickname === nick)
			if (index2 !== -1) {
				nick = nick + global.users.length
			}
			var user = {
				id: socket.id.toString(),
				sid: shortid.generate(),
				nickname: nick,
				status: "online",
				ip: socket.handshake.address
			}
			global.users.push(user)
			socket.broadcast.emit("status", {
				content: "join the chat",
				nick: nick
			})
		})

		//logoff
		socket.on("disconnect", function () {
			var index = global.users.findIndex(x => x.id === socket.id)
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
			list[0] = "\nUser List: \n"

			for (i = 1; i < Object.keys(global.users).length + 1; i++) {
				var a = global.users[Object.keys(global.users)[i - 1]]
				if (a.status === "online") {
					status = a.status.green
				}
				if (a.status === "afk") {
					status = a.status.yellow
				}
				if (a.status === "dnd") {
					status = a.status.red
				}
				list[i] = a.sid.grey + " " + a.nickname.blue + " (" + status + ")"
			}

			var table = list.join("\n")
			socket.emit("return", table)

		})

		//long list
		socket.on("long_list", function () {
			var list = global.users
			socket.emit("return", list)
		})

		//afk
		socket.on("afk", function (nickname) {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "afk"
				var msg = {
					content: "is afk",
					nick: nickname
				}
				socket.broadcast.emit("status", msg)
			}

		})

		//online
		socket.on("online", function (nickname) {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "online"
				var msg = {
					content: "is online",
					nick: nickname
				}
				socket.broadcast.emit("status", msg)
			}
		})

		//dnd
		socket.on("dnd", function (nickname) {
			if (global.users.some(e => e.id === socket.id)) {
				var index = global.users.findIndex(x => x.id === socket.id)
				global.users[index].status = "dnd"
				var msg = {
					content: "is dnd",
					nick: nickname
				}
				socket.broadcast.emit("status", msg)
			}
		})

		//message
		socket.on("message", function (msg) {
			if (msg) {
				if (msg.hasOwnProperty("content") && msg.hasOwnProperty("nick")) {
					if (msg.content !== "") {
						if (msg.nick.length > 15) {
							msg.nick = msg.nick.substring(0, 15)
						}
						socket.broadcast.emit("message", msg)
					}
				}
			}
		})

		//mention
		socket.on("mention", function (nick, mentions) {
			var index = global.users.findIndex(x => x.nickname === nick)
			if (global.users[index]) {
				var msg = {
					nick: mentions,
					content: "mentioned you"
				}
				socket.to(`${global.users[index].id}`).emit("mentioned", msg)
			} else {
				socket.emit("return", "This user not exist")
			}
		})

		//return
		socket.on("return", function (msg) {
			socket.emit("return", msg)
		})
	})
}