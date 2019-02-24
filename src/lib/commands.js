//COMMANDS
const colors = require("colors")
const client = require("./client")
const host = require("./host")
const out = require("./out")
var global = require("./global")
var settings = require("./settings")

module.exports = {

	//clear
	clear: function () {
		process.stdout.write("\033c")
	},

	//exit
	exit: function () {
		process.stdout.write("\033c")
		process.exit(0)
	},

	//host
	host: function () {
		host.start()
	},

	//nick
	nick: function (args) {
		var message
		if (typeof args[0] === "undefined" || args[0] === "") {
			message = "Try: /nick <new_nick>"
		} else {
			settings.nick = (args[0])
			message = "Your nickname is now " + args[0].blue
			if (global.connection_status) {
				client.nick()
			}
		}
		out.blank(message)
	},

	//help
	help: function () {
		var help = []
		help[0] = ""
		help[1] = "HELP\n".green
		help[2] = "/host - create server"
		help[3] = "/connect <ip> - connect to server"
		help[4] = "/disconnect - disconnect from server"
		help[5] = "/clear - clear window"
		help[6] = "/exit - exit Lan Chat"
		help[7] = "/nick <nickname> - set nickname"
		help[8] = "/list - connected users list"
		help[9] = "/afk - change status to afk"
		help[10] = "/online - change status to online"
		help[11] = ""
		out.blank(help.join("\n"))
	},

	//connect
	connect: function (args) {
		client.connect(args[0])
	},

	//disconnect
	disconnect: function () {
		client.disconnect()
	},

	//list
	list: function () {
		if (global.connection_status) {
			client.list()
		} else {
			out.alert("you must be connected")
		}
	},

	//afk
	afk: function () {
		if (global.connection_status) {
			client.afk()
			out.status("you are afk")
		} else {
			out.alert("you must be connected")
		}
	},

	//online
	online: function () {
		if (global.connection_status) {
			client.online()
			out.status("you are online")
		} else {
			out.alert("you must be connected")
		}
	},

	//rainbow
	rb: function (args) {
		var content = args.join(" ")
		client.send(content.rainbow)
	},

	//whois
	long_list: function () {
		if (global.connection_status) {
			client.long_list()
		} else {
			out.alert("you must be connected")
		}
	},

	//dev commands
	d1: function () {
		client.connect("localhost")
	}
}