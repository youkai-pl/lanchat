// import
const out = require("./out")
const c = require("./colors")
const notify = require("./notify")
const config = require("./config")
const udp = require("./udp")

// CLIENT
// variables
var trycount = 0
var connection = false
var reconnect = false
var safeDisconnect = false
var inprogress = false

module.exports = {

	// connect
	connect: function (ip) {

		// check ip
		if (!ip) {
			out.blank("Try: /connect <ip>")
		} else {

			// block double connect
			if (connection || inprogress) {
				if (connection) {
					out.alert("you already connected")
				}
				if (inprogress) {
					out.alert("you are under connecting")
				}
			} else {

				out.loading("connecting")

				// stop udp listening
				udp.close()

				// lock
				inprogress = true
				module.exports.inprogress = inprogress

				// create socket
				socket = require("socket.io-client")("http://" + ip + ":" + config.port,
					{
						"reconnection": true,
						"reconnectionDelay": 500,
						"reconnectionDelayMax": 500,
						"reconnectionAttempts": config.attemps,
						"secure": true
					}
				)

				// connected
				socket.on("connect", () => {
					out.stopLoading()
					connection = true
					module.exports.connection = connection
					inprogress = false
					module.exports.inprogress = inprogress
					trycount = 0
					if (reconnect) {
						out.status("reconnected")
					} else {
						listen()
						out.status("connected")
					}
					socket.emit("login", config.nick)
				})

				// handle disconnect
				socket.on("disconnect", () => {
					out.stopLoading()
					connection = false
					module.exports.connection = connection
					inprogress = false
					module.exports.inprogress = inprogress
					udp.listen()
					if (!safeDisconnect) {
						out.alert("disconnected")
					}
				})

				// handle reconnect
				socket.on("reconnect", () => {
					reconnect = true
					inprogress = false
					module.exports.inprogress = inprogress
				})

				// handle conecting
				socket.on("connecting", () => {
				})

				// handle recconecting
				socket.on("reconnecting", () => {
					out.loading("connecting")

					trycount++
					inprogress = true
					module.exports.inprogress = inprogress
					// out.status("connecting")
					if (trycount == config.attemps) {
						out.alert("connection error")
						out.stopLoading()
						inprogress = false
						module.exports.inprogress = inprogress
					}
				})
			}
		}
	},

	// send
	send: function (content) {

		if (connection) {
			out.message({
				content: content,
				nick: config.nick
			})
			socket.emit("message", content)
		}
	},

	// mention
	mention: function (nick) {
		socket.emit("mention", nick)
	},

	// nick
	nick: function () {
		socket.emit("changeNick", config.nick)
	},

	// auth
	auth: function (password) {
		socket.emit("auth", config.nick, password)
	},

	// lock
	lock: function (pass) {
		socket.emit("setPassword", config.nick, pass)
	},

	// disconnect
	disconnect: function () {
		reconnect = false
		safeDisconnect = true
		socket.disconnect()
		out.status("disconnected")
	},

	// list
	list: function () {
		socket.emit("list")
	},

	// changeStatus
	changeStatus: function (value) {
		socket.emit("changeStatus", value)
	},

	// kick
	kick: function (arg) {
		if (arg !== config.nick) {
			socket.emit("kick", arg)
		} else {
			out.status("You can't kick yourself")
		}
	},

	// ban
	ban: function (arg) {
		if (arg !== config.nick) {
			socket.emit("ban", arg)
		} else {
			out.status("You can't ban yourself")
		}
	},

	// unban
	unban: function (arg) {
		if (arg !== config.nick) {
			socket.emit("unban", arg)
		} else {
			out.status("You can't unban yourself")
		}
	},

	// mute
	mute: function (arg) {
		if (arg !== config.nick) {
			socket.emit("mute", arg)
		} else {
			out.status("You can't mute yourself")
		}
	},

	// unmute
	unmute: function (arg) {
		if (arg !== config.nick) {
			socket.emit("unmute", arg)
		} else {
			out.status("You can't unmute yourself")
		}
	},

	// change permission
	level: function (arg) {
		socket.emit("setPermission", arg[0], arg[1])
	},
}

// listen
function listen() {

	// message
	socket.on("message", (msg) => {

		// show message when dnd is disabled
		if (config.status !== "dnd") {
			out.message(msg)
			notify.message(msg)
		}
	})

	// mention
	socket.on("mention", (nick) => {

		// show mention when dnd is disabled
		if (config.status !== "dnd") {
			out.mention(nick)
			notify.mention()
		}
	})

	// motd
	socket.on("motd", (motd) => {
		if (!reconnect) {
			out.blank("\n" + motd + "\n")
		}
	})

	// joined
	socket.on("join", (nick) => {
		out.user_status(nick, "joined")
	})

	// left
	socket.on("left", (nick) => {
		out.user_status(nick, "left")
	})

	// online
	socket.on("online", (nick) => {
		out.user_status(nick, "is online")
	})

	// dnd
	socket.on("dnd", (nick) => {
		out.user_status(nick, "dnd")
	})

	// afk
	socket.on("afk", (nick) => {
		out.user_status(nick, "is afk")
	})

	// needAuth
	socket.on("needAuth", () => {
		out.status("login required")
	})

	// wrongPass
	socket.on("wrongPass", () => {
		out.warning("wrong password")
	})

	// nickTaken
	socket.on("nickTaken", () => {
		out.warning("nick already taken, change it and try again")
	})

	// passChanged
	socket.on("passChanged", () => {
		out.warning("password changed")
	})

	// muted
	socket.on("clientMuted", () => {
		out.warning("you are muted")
	})

	// tooLong
	socket.on("tooLong", () => {
		out.warning("message too long")
	})

	// flood
	socket.on("flood", () => {
		out.warning("flood blocked by server")
	})

	// notSigned
	socket.on("notSigned", () => {
		out.warning("you must be logged in")
	})

	// notExist
	socket.on("notExist", () => {
		out.warning("user doesn't exist")
	})

	// loginSucces
	socket.on("loginSucces", () => {
		out.status("logged succesfull")
	})

	// alreadySigned
	socket.on("alreadySigned", () => {
		out.warning("alredy signed")
	})

	// nickShortened
	socket.on("nickShortened", () => {
		out.warning("server has shortened your nickname")
	})

	// userChangeNick
	socket.on("userChangeNick", (old, nick) => {
		out.nickChange(old, nick)
	})

	// nickChanged
	socket.on("nickChanged", () => {
		out.blank("Your nickname is now " + c.blue + args[0] + c.reset)
	})

	// usersList
	socket.on("list", (users) => {
		out.list(users)
	})

	// incorrectValue
	socket.on("incorrectValue", () => {
		out.alert("incorrect value")
	})

	// statusChanged
	socket.on("statusChanged", () => { })

	// noPermission
	socket.on("noPermission", () => {
		out.warning("no permission")
	})

	// doneMute
	socket.on("doneMute", (nick) => {
		out.user_status(nick, "is muted")
	})

	// doneMute
	socket.on("doneUnmute", (nick) => {
		out.user_status(nick, "is unmuted")
	})

	// doneBan
	socket.on("doneBan", (nick) => {
		out.user_status(nick, "is banned")
	})

	// doneUnBan
	socket.on("doneUnban", (nick) => {
		out.user_status(nick, "is unbanned")
	})

	// doneSetPermission
	socket.on("doneSetPermission", (nick, level) => {
		out.user_status(nick, "permissions changed to " + level)
	})

	// /socketLimit
	socket.on("socketLimit", () => {
		out.alert("all sockets is taken")
	})
}