//import
const out = require("./out")
const notify = require("./notify")
const config = require("./config")
var global = require("./global")

//CLIENT

//variables
var trycount = 0

module.exports = {

	//connect
	connect: function (ip) {

		//check ip
		if (!ip) {
			out.blank("Try: /connect <ip>")
		} else {

			//block double connect
			if (global.lock) {
				out.alert("you already connected")
			} else {

				//connet
				out.status("connecting")
				global.lock = true
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

				//connect
				socket.on("connect", function () {
					//normal way
					if (!global.reconnect) {
						global.lock = true
						global.safe_disconnect = false
						out.status("connected")
						listen()
						socket.emit("login", config.nick)
						global.connection_status = true
						global.first = true
					} else {
						//recconect way
						socket.emit("login", config.nick)
						if (!global.first) {
							listen()
						}
						global.lock = true
						global.safe_disconnect = false
					}
				})

				//handle connection error
				socket.on("connect_error", function () {
					if (trycount === config.attemps) {
						out.alert("connection error")
					}
				})

				//handle disconnect
				socket.on("disconnect", function () {
					if (global.safe_disconnect !== true) {
						out.alert("disconnected")
						global.reconnection = true
						global.lock = false
						global.connection_status = false
					}
				})

				//handle reconnect
				socket.on("reconnect", () => {
					if (global.reconnection) {
						out.status("reconnected")
					} else {
						out.status("connected")
					}
					global.lock = true
					global.reconnect = true
					global.connection_status = true
				})

				//handle conecting
				socket.on("connecting", () => {
					global.lock = true
				})

				//handle recconecting
				socket.on("reconnecting", () => {

					//show status
					if (global.reconnection) {
						out.status("trying recconect")
					} else {
						out.status("connecting")
					}
					global.lock = true

					//count attemps
					trycount++
					if (trycount === config.attemps) {
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
		if (global.lock) {
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
				out.blank("try /register <password> <password>")
			}
		} else {
			out.blank("try /register <password> <password>")
		}
	},

	//disconnect
	disconnect: function () {
		if (global.lock) {
			if (global.server_status) {
				out.status("host cannot be disconnect")
			} else {
				//disconnect
				global.safe_disconnect = true
				socket.disconnect()
				global.lock = false
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
		if (value === "dnd") {
			global.dnd = true
		} else {
			global.dnd = false
		}
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
		if (!global.dnd) {
			out.message(msg)
			notify.message(msg)
		}
	})

	//mention
	socket.on("mentioned", function (nick) {

		//show mention when dnd is disabled
		if (!global.dnd) {
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
		if (!global.reconnect) {
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