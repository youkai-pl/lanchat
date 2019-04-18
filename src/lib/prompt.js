//import
const client = require("./client")
const commands = require("./commands")
const pkg = require("../package.json")
const rl = require("./interface").rl
const readline = require("./interface").readline
const config = require("./config")
const dwn = require("./dwn")
var global = require("./global")

//PROMPT
module.exports = {
	run: function () {
		rl.pause()
		//init
		process.stdout.write("\033c")
		process.stdout.write(
			String.fromCharCode(27) + "]0;" + "Lanchat" + String.fromCharCode(7)
		)

		//welcome screen
		if (process.stdout.columns > 41) {

			//make it green
			console.log("\x1b[92m")

			//shitty ascii art
			console.log(" █     █████ ███ █ █████ █   █ █████ █████")
			console.log(" █     █   █ █ █ █ █     █   █ █   █   █")
			console.log(" █     █████ █ █ █ █     █████ █████   █")
			console.log(" █     █   █ █ █ █ █     █   █ █   █   █")
			console.log(" █████ █   █ █ ███ █████ █   █ █   █   █")
		} else {
			console.log("Lanchat")
		}

		//reset color
		console.log("\x1b[0m")

		//show acutal version
		console.log(" Version " + pkg.version)

		//plugis info
		if (global.plugins) {
			if (Object.keys(global.plugins).length) {
				console.log(" Loaded " + Object.keys(global.plugins).length + " plugin(s)")
			}
		}

		//show user nick
		console.log(" Nickname: " + config.nick)

		//check update
		console.log(" Checking updates")
		dwn.selfCheck().then((data) => {
			readline.moveCursor(process.stdout, 0, -1)
			if (data) {
				console.log(" Update avabile: (" + data + ")")
			} else {
				process.stdout.clearLine()
				process.stdout.cursorTo(0)
			}
			console.log("")
			rl.prompt(true)
		})

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

			//execute command
			const args = message.split(" ")
			if (typeof commands[args[0].substr(1)] !== "undefined") {
				commands[args[0].substr(1)](args.slice(1))
			}

			//try execute commands from plugins
			for (i in global.plugins) {
				if (typeof global.plugins[i][args[0].substr(1)] !== "undefined") {
					global.plugins[i][args[0].substr(1)](args.slice(1))
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