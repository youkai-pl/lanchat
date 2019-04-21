//import
const client = require("./client")
const commands = require("./commands")
const pkg = require("../package.json")
const rl = require("./interface").rl
const readline = require("./interface").readline
const config = require("./config")
const dwn = require("./dwn")
const plugins = require("./plugins")
const out = require("./out")

//PROMPT
module.exports = {
	run: function () {

		//init
		rl.pause()
		process.stdout.write("\033c")
		process.stdout.write(
			String.fromCharCode(27) + "]0;" + "Lanchat" + String.fromCharCode(7)
		)

		//welcome screen
		if (process.stdout.columns > 41) {

			//set color
			process.stdout.write("\x1b[95m")

			//ascii art
			console.log("    __                  _           _   ")
			console.log("   / /  __ _ _ __   ___| |__   __ _| |_ ")
			console.log("  / /  / _` | '_ \\ / __| '_ \\ / _` | __|")
			console.log(" / /__| (_| | | | | (__| | | | (_| | |_ ")
			console.log(" \\____/\\__,_|_| |_|\\___|_| |_|\\__,_|\\__|")
		} else {
			console.log(" Lanchat")
		}

		//reset color
		console.log("\x1b[0m")

		//show acutal version
		console.log(" Version " + pkg.version)

		//plugis info
		if (plugins.run) {
			if (Object.keys(plugins.run).length) {
				console.log(" Loaded " + Object.keys(plugins.run).length + " plugin(s)")
			}
		}

		//show user nick
		console.log(" Nickname: " + config.nick)

		//check update
		out.loading("checking updates")
		dwn.selfCheck().then((data) => {
			out.stopLoading()
			process.stdout.clearLine()
			process.stdout.cursorTo(0)
			if (data) {
				console.log(" Update avabile: (" + data + ")")
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

	//clear line
	readline.moveCursor(process.stdout, 0, -1)

	//check prefix
	if (message.charAt(0) !== "/") {

		//send message
		client.send(message)
	} else {

		//execute command
		const args = message.split(" ")
		if (typeof commands[args[0].substr(1)] !== "undefined") {
			commands[args[0].substr(1)](args.slice(1))
		}

		//try execute commands from plugins
		for (i in plugins.run) {
			if (typeof plugins.run[i][args[0].substr(1)] !== "undefined") {
				plugins.run[i][args[0].substr(1)](args.slice(1))
			}
		}

		//reset cursor
		process.stdout.clearLine()
		process.stdout.cursorTo(0)
	}
}
