//CONSOLE
const rl = require("./interface").rl
const fn = require("./common")
const colors = require("colors")

module.exports = {

	//view message
	message: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log("[" + fn.time().green + "] " + msg.nick.blue + ": " + msg.content)
		rl.prompt(true)
	},

	//user status
	user_status: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log("[" + fn.time().green + "] " + msg.nick.blue + " " + msg.content.green)
		rl.prompt(true)
	},

	//view alert
	alert: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log("[!] ".red + msg.red)
		rl.prompt(true)
	},

	//view status
	status: function (msg) {
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
		console.log("[#] ".green + msg.green)
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

