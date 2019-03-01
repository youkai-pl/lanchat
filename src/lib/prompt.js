//PROMPT
const colors = require("colors")
const client = require("./client")
const commands = require("./commands")
const setTitle = require("console-title")
const settings = require("./settings")
var rl = require("./interface").rl
var readline = require("./interface").readline

module.exports = {
	run: function () {
		//init
		initui()
		settings.load()
		//prompt
		rl.on("line", function (line) {
			wrapper(line)
			rl.prompt(true)
		})
		//exit
		rl.on("close", function () {
			process.stdout.write("\033c")
			process.exit(0)
		})
	}
}

//initui
function initui() {
	process.stdout.write("\033c")
	setTitle("lanchat")
	console.log("LANCHAT 1.0.0".green)
	console.log("")
	rl.prompt(true)
}

//user input wrapper
function wrapper(message) {
	//if it message
	if (message.charAt(0) !== "/") {
		//send
		readline.moveCursor(process.stdout, 0, -1)
		client.send(message)

	} else {
		//if it command
		const args = message.split(" ")
		if (typeof commands[args[0].substr(1)] !== "undefined") {
			answer = commands[args[0].substr(1)](args.slice(1))
		}

		process.stdout.clearLine()
		process.stdout.cursorTo(0)
	}
}