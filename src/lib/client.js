//CLIENT
const fn = require("./common")
const out = require("./out")
const notify = require("./notify")
const colors = require("colors")
var settings = require("./settings")
var global = require("./global")

//variables
var trycount = 0
var attemps = 5

module.exports = {

	//send
	send: function (content) {
		var msg = {
			content: content,
			nick: settings.nick
		}
		out.message(msg)
		if (global.lock) {
			socket.emit("message", msg)
		}
	},

	//mention
	mention: function (nick) {
		socket.emit("mention", nick, settings.nick)
	},

	//status
	status: function (msg) {
		status(msg)
	},

	//nick
	nick: function () {
		socket.emit("nick", settings.nick)
	},

	//long list
	long_list: function () {
		socket.emit("long_list")
	},

	//connect
	connect: function (ip) {
		if (fn.isEmptyOrSpaces(ip)) {
			out.blank("Try: /connect <ip>")
		} else {
			if (global.lock) {
				out.alert("you already connected")
			} else {
				out.status("connecting")
				global.lock = true

				//create socket
				socket = require("socket.io-client")("http://" + ip + ":" + settings.port,
					{
						"reconnection": true,
						"reconnectionDelay": 500,
						"reconnectionDelayMax": 500,
						"reconnectionAttempts": attemps
					}
				)

				//connect
				socket.on("connect", function () {
					if (!global.reconnect) {
						global.lock = true
						global.safe_disconnect = false
						out.status("connected")
						listen()
						login()
						global.connection_status = true
						global.first = true
					} else {
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
					if (global.reconnection) {
						out.status("trying recconect")
					} else {
						out.status("connecting")
					}
					global.lock = true
					trycount++
				})
			}
		}
	},

	//disconnect
	disconnect: function () {
		if (global.lock) {
			socket.emit("status", settings.nick)
			global.safe_disconnect = true
			socket.disconnect()
			global.lock = false
			if (global.server_status) {
				out.status("you are disconnected but server is still working")
			} else {
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
		socket.emit("afk", settings.nick)
	},

	//online
	online: function () {
		global.dnd = false
		socket.emit("online", settings.nick)
	},

	//online
	dnd: function () {
		global.dnd = true
		socket.emit("dnd", settings.nick)
	}
}

//FUNCTIONS

//listen
function listen() {

	//message
	socket.on("message", function (msg) {
		if (!global.dnd) {
			out.message(msg)
			notify.message(msg)
		}
	})

	//mention
	socket.on("mentioned", function (msg) {
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
}

//login
function login() {
	socket.emit("login", settings.nick)
}