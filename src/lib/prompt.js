//PROMPT
const colors = require("colors")
const client = require("./client")
const commands = require("./commands")
const out = require("./out")
const setTitle = require("console-title")
var rl = require("./interface").rl
var readline = require("./interface").readline
var settings = require("./settings")

module.exports = {
	run: function () {
		//init
		initui()

		//prompt
		rl.on("line", function (line) {
			readline.moveCursor(process.stdout, 0,-1)
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
	console.log("LANCHAT 0.4.2".green)
	console.log("")
	rl.prompt(true)
}

//user input wrapper
function wrapper(message) {

	//if it message
	if (message.charAt(0) !== "/") {
		//send
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