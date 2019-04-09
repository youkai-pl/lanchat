//import
const client = require("./client")
const commands = require("./commands")
const pkg = require("../package.json")
const rl = require("./interface").rl
const readline = require("./interface").readline
const plugins = require("require-all")(__dirname + "../../plugins")
var global = require("./global")

//PROMPT
module.exports = {
	run: function () {

		//init
		process.stdout.write("\033c")
		process.stdout.write(
			String.fromCharCode(27) + "]0;" + "Lanchat" + String.fromCharCode(7)
		)
		console.log("Lanchat " + pkg.version)
		console.log("")

		if (Object.keys(plugins).length) {
			console.log("Loaded " + Object.keys(plugins).length + " plugin(s)")
		}
		if (plugins.hasOwnProperty("host")) {
			console.log("Host plugin loaded")
		}
		console.log("Nickname: " + global.nick)
		console.log("")

		rl.prompt(true)

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

//user input wrapper
function wrapper(message) {
	if (message) {
		//check prefix
		if (message.charAt(0) !== "/") {

			//clear line
			readline.moveCursor(process.stdout, 0, -1)

			//send message
			client.send(message)
		} else {

			//clear line
			readline.moveCursor(process.stdout, 0, -1)

			//execute command
			const args = message.split(" ")
			if (typeof commands[args[0].substr(1)] !== "undefined") {
				commands[args[0].substr(1)](args.slice(1))
			}

			//try execute commands from plugins
			for (i in plugins) {
				if (typeof plugins[i][args[0].substr(1)] !== "undefined") {
					plugins[i][args[0].substr(1)](args.slice(1))
				}
			}

			//reset cursor
			process.stdout.clearLine()
			process.stdout.cursorTo(0)
		}
	} else {
		//clear line
		readline.moveCursor(process.stdout, 0, -1)
	}
}