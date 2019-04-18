//import
const out = require("./out")
const notify = require("./notify")
const config = require("./config")
var global = require("./global")

//CLIENT
//variables
var trycount = 0
global.connected = false
reconnect = false
safeDisconnect = false

module.exports = {

	//connect
	connect: function (ip) {

		//check ip
		if (!ip) {
			out.blank("Try: /connect <ip>")
		} else {

			//block double connect
			if (global.connected) {
				out.alert("you already connected")
			} else {

				//create socket
				socket = require("socket.io-client")("http://" + ip + ":" + config.port,
					{
						"reconnection": true,
						"reconnectionDelay": 500,
						"reconnectionDelayMax": 500,
						"reconnectionAttempts": config.attemps,
						"secure": true
					}
				)

				//connected
				socket.on("connect", function () {
					global.connected = true
					trycount = 0
					if (reconnect) {
						out.status("reconnected")
					} else {
						listen()
						out.status("connected")
					}
					socket.emit("login", config.nick)
				})

				//handle disconnect
				socket.on("disconnect", function () {
					global.connected = false
					if (!safeDisconnect) {
						global.lock = false
						out.alert("disconnected")
					}
				})

				//handle reconnect
				socket.on("reconnect", () => {
					reconnect = true
				})

				//handle conecting
				socket.on("connecting", () => {
					out.status("connecting")
				})

				//handle recconecting
				socket.on("reconnecting", () => {
					trycount++
					out.status("connecting")
					if (trycount == config.attemps) {
						global.lock = false
						out.alert("connection error")
					}
				})
			}
		}
	},

	//send
	send: function (content) {
		//out
		out.message({
			content: content,
			nick: config.nick
		})
		//send
		if (global.connected) {
			socket.emit("message", content)
		}
	},

	//mention
	mention: function (nick) {
		socket.emit("mention", nick)
	},

	//nick
	nick: function () {
		socket.emit("nick", config.nick)
	},

	//auth
	auth: function (password) {
		if (password) {
			socket.emit("auth", config.nick, password)
		} else {
			out.blank("try /login <password>")
		}
	},

	//lock
	lock: function (args) {
		if (args) {
			if (args[0] === args[1]) {
				socket.emit("register", config.nick, args[0])
			} else {
				out.blank("try /lock <password> <password>")
			}
		} else {
			out.blank("try /lock <password> <password>")
		}
	},

	//disconnect
	disconnect: function () {
		if (global.connected) {
			if (global.host) {
				out.status("host cannot be disconnect")
			} else {

				//disconnect
				reconnect = false
				safeDisconnect = true
				socket.disconnect()
				out.status("disconnected")
			}
		} else {
			out.alert("you are not connected")
		}
	},

	//list
	list: function () {
		socket.emit("list")
	},

	//changeStatus
	changeStatus: function (value) {
		socket.emit("changeStatus", value)
	},

	//kick
	kick: function (arg) {
		if (arg !== config.nick) {
			socket.emit("kick", arg)
		} else {
			out.status("You can't kick yourself")
		}
	},

	//ban
	ban: function (arg) {
		if (arg !== config.nick) {
			socket.emit("ban", arg)
		} else {
			out.status("You can't ban yourself")
		}
	},

	//unban
	unban: function (arg) {
		if (arg !== config.nick) {
			socket.emit("unban", arg)
		} else {
			out.status("You can't unban yourself")
		}
	},

	//mute
	mute: function (arg) {
		if (arg !== config.nick) {
			socket.emit("mute", arg)
		} else {
			out.status("You can't mute yourself")
		}
	},

	//unmute
	unmute: function (arg) {
		if (arg !== config.nick) {
			socket.emit("unmute", arg)
		} else {
			out.status("You can't unmute yourself")
		}
	},

	//change permission
	level: function (arg) {
		socket.emit("level", arg)
	},
}

//listen
function listen() {

	//message
	socket.on("message", function (msg) {

		//show message when dnd is disabled
		if (config.status !== "dnd") {
			out.message(msg)
			notify.message(msg)
		}
	})

	//mention
	socket.on("mentioned", function (nick) {

		//show mention when dnd is disabled
		if (config.status !== "dnd") {
			out.mention(nick)
			notify.mention()
		}
	})

	//status
	socket.on("status", function (msg) {
		out.user_status(msg)
	})

	//return
	socket.on("return", function (msg) {
		out.blank(msg)
	})

	//motd
	socket.on("motd", function (motd) {
		if (!reconnect) {
			out.blank("\n" + motd + "\n")
		}
	})

	//return code
	socket.on("rcode", function (value) {
		if (config.devlog) {
			out.blank("RETURN: " + value)
		}
	})
}