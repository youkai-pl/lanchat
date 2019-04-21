//import
const rl = require("./interface").rl
const c = require("./colors")
var loading

//OUT
module.exports = {

	//view message
	message: function (msg) {
		out(c.gray + "[" + time() + "] " + c.blue + msg.nick + c.reset + ": " + msg.content)
	},

	//user status
	user_status: function (nick, content) {
		out(c.blue + nick + " " + c.reset + content)
	},

	//mention
	mention: function (nick) {
		out(c.blue + nick + c.reset + " mentioned you")
	},

	//view alert
	alert: function (msg) {
		out(c.red + "[!] " + msg + c.reset)
	},

	//warning
	warning: function (msg) {
		out(c.yellow + "[!] " + msg + c.reset)
	},

	//view status
	status: function (msg) {
		out(c.blue + "[#] " + msg + c.reset)
	},

	//normal console text
	blank: function (msg) {
		out(msg)
	},

	//nick change
	nickChange: function (old, nick) {
		out(c.blue + old + c.reset + " is now " + c.blue + nick + c.reset)
	},

	//list of users
	list: function (user, status) {
		var color
		if (status === "online") {
			color = c.green
		}
		if (status === "afk") {
			color = c.yellow
		}
		if (status === "dnd") {
			color = c.red
		}
		if (!status) {
			color = c.reset
		}

		out("(" + color + status + c.reset + ") " + user)
	},

	//loading
	loading: function (content) {
		clearInterval(loading)
		process.stderr.write("\x1B[?25l")
		animation(content)
	},

	//stop loading
	stopLoading: function () {
		clearInterval(loading)
		process.stderr.write("\x1B[?25h")
	}
}

//get time
function time() {
	var date = new Date()
	var time = (("0" + date.getHours()).slice(-2) + ":" +
		("0" + date.getMinutes()).slice(-2) + ":" +
		("0" + date.getSeconds()).slice(-2))
	return time
}

function out(content) {
	process.stdout.clearLine()
	process.stdout.cursorTo(0)
	console.log(content)
	rl.prompt(true)
}

function animation(content) {
	loading = (function () {
		var P = ["\\", "|", "/", "-"]
		var x = 0
		return setInterval(function () {
			process.stdout.clearLine()
			process.stdout.write(c.blue + "\r" + P[x++] + " " + content + c.reset)
			x &= 3
		}, 100)
	})()
}
