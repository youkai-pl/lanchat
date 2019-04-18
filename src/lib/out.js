//import
const rl = require("./interface").rl
const c = require("./colors")

//OUT
module.exports = {

	//view message
	message: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(c.green + "[" + time() + "] " + c.blue + msg.nick + c.reset + ": " + msg.content)
		rl.prompt(true)
	},

	//user status
	user_status: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(c.blue + msg.nick + " " + c.reset + msg.content)
		rl.prompt(true)
	},

	//mention
	mention: function (nick) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(c.blue + nick + c.reset + " mentioned you")
		rl.prompt(true)
	},

	//view alert
	alert: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(c.red + "[!] " + msg + c.reset)
		rl.prompt(true)
	},

	//view status
	status: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(c.green + "[#] " + msg + c.reset)
		rl.prompt(true)
	},

	//normal console text
	blank: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log(msg)
		rl.prompt(true)
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