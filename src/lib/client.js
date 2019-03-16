//import
const fn = require("./common")
const out = require("./out")
const notify = require("./notify")
const colors = require("colors")
var settings = require("./settings")
var global = require("./global")

//CLIENT

//config
var trycount = 0
var attemps = 5

module.exports = {

	//send
	send: function (content) {
		//out
		out.message({
			content: content,
			nick: global.nick
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
		socket.emit("nick", global.nick)
	},

	//long list
	long_list: function () {
		socket.emit("long_list")
	},

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
				socket = require("socket.io-client")("http://" + ip + ":" + global.port,
					{
						"reconnection": true,
						"reconnectionDelay": 500,
						"reconnectionDelayMax": 500,
						"reconnectionAttempts": attemps
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
						login()
						global.connection_status = true
						global.first = true
					} else {
						//recconect way
						login()
						if (!global.first) {
							listen()
						}
						global.lock = true
						global.safe_disconnect = false
					}
				})

				//handle connection error
				socket.on("connect_error", function () {
					if (trycount === attemps) {
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
					if (trycount === attemps) {
						global.lock = false
					}
				})
			}
		}
	},

	//auth
	auth: function (password) {
		if (password) {
			socket.emit("auth", global.nick, password)
		} else {
			out.blank("try /login <password>")
		}
	},

	//register
	register: function (args) {
		if (args) {
			if (args[0] === args[1]) {
				socket.emit("register", global.nick, args[0])
			} else {
				out.blank("passwords do not match")
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
				socket.emit("status")
				global.safe_disconnect = true
				//socket.disconnect()
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

	//afk
	afk: function () {
		global.dnd = false
		socket.emit("afk")
	},

	//online
	online: function () {
		global.dnd = false
		socket.emit("online")
	},

	//online
	dnd: function () {
		global.dnd = true
		socket.emit("dnd")
	},

	//kick
	kick: function (arg) {
		if (arg !== global.nick) {
			socket.emit("kick", global.nick, arg)
		} else {
			out.status("You can't kick yourself")
		}
	},

	//ban
	ban: function (arg) {
		if (arg !== global.nick) {
			socket.emit("ban", global.nick, arg)
		} else {
			out.status("You can't ban yourself")
		}
	},

	//unban
	unban: function (arg) {
		if (arg !== global.nick) {
			socket.emit("unban", global.nick, arg)
		} else {
			out.status("You can't unban yourself")
		}
	},

	//mute
	mute: function (arg) {
		if (arg !== global.nick) {
			socket.emit("mute", global.nick, arg)
		} else {
			out.status("You can't mute yourself")
		}
	},

	//unmute
	unmute: function (arg) {
		if (arg !== global.nick) {
			socket.emit("unmute", global.nick, arg)
		} else {
			out.status("You can't unmute yourself")
		}
	},

	//chane permission
	level: function (arg) {
		socket.emit("level", global.nick, arg)
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
	socket.on("mentioned", function (msg) {

		//show mention when dnd is disabled
		if (!global.dnd) {
			out.user_status(msg)
			notify.mention(msg)
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

	//nick
	socket.on("nick", function (nick) {
		out.blank("Your nick is now " + nick.blue)
		settings.nickChange(nick)
	})

	//motd
	socket.on("motd", function (motd) {
		if (!global.reconnect) {
			out.blank("\n" + motd + "\n")
		}
	})
}

//login
function login() {
	socket.emit("login", global.nick)
}