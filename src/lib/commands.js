//import
const colors = require("colors")
const client = require("./client")
const out = require("./out")
const fs = require("fs")
var global = require("./global")
var settings = require("./settings")

//COMMANDS
module.exports = {

	//clear
	clear: function () {
		//clear window
		process.stdout.write("\033c")
	},

	//exit
	exit: function () {
		//exit
		process.stdout.write("\033c")
		process.exit(0)
	},

	//nick
	nick: function (args) {

		//handle blank input
		if (!args[0]) {
			out.blank("Try: /nick <new_nick>")
		} else {

			//change local nick
			settings.change("nick", args[0])
			out.blank("Your nickname is now " + args[0].blue)

			//change nick on host
			if (global.connection_status) {
				client.nick()
			}
		}
	},

	//help
	help: function () {
		var help = []
		help[0] = ""
		help[1] = "/host - create server"
		help[2] = "/connect <ip> - connect to server"
		help[3] = "/disconnect - disconnect from server"
		help[4] = "/clear - clear window"
		help[5] = "/exit - exit Lan Chat"
		help[6] = "/nick <nickname> - set nickname"
		help[7] = "/list - connected users list"
		help[8] = "/afk - change status to afk"
		help[9] = "/online - change status to online"
		help[10] = "/dnd - do not disturb, mute all messages"
		help[11] = "/notify <all / mention / none> - change notify setting"
		help[12] = "/m <nick> - mention user"
		help[13] = "/login <password> - login"
		help[14] = "/register <password> <password> - protect account on server,\n password will be change with same command"
		help[15] = "/kick <nick> - kick user"
		help[16] = "/ban <nick> - ban user"
		help[17] = "/unban <nick> - unban user"
		help[18] = "/mute <nick> - mute user"
		help[19] = "/unmute <nick> - unmute user"
		help[20] = "/level <nick> <1-5> - change user permission level"
		help[21] = ""
		out.blank(help.join("\n"))
	},

	//connect
	connect: function (args) {
		client.connect(args[0])
	},

	//login
	login: function (args) {
		if (global.connection_status) {
			client.auth(args[0])
		} else {
			out.alert("you must be connected")
		}
	},

	//register
	register: function (args) {
		if (global.connection_status) {
			client.register(args)
		} else {
			out.alert("you must be connected")
		}
	},

	//disconnect
	disconnect: function () {
		client.disconnect()
	},

	//list
	list: function () {
		//get connected user list
		if (global.connection_status) {
			client.list()
		} else {
			out.alert("you must be connected")
		}
	},

	//afk
	afk: function () {
		//set afk status
		if (global.connection_status) {
			client.changeStatus("afk")
			out.status("you are afk")
		} else {
			out.alert("you must be connected")
		}
	},

	//online
	online: function () {
		//set online status
		if (global.connection_status) {
			client.changeStatus("online")
			out.status("you are online")
		} else {
			out.alert("you must be connected")
		}
	},

	//dnd
	dnd: function () {
		//set dnd status
		if (global.connection_status) {
			client.changeStatus("dnd")
			out.status("you are dnd")
		} else {
			out.alert("you must be connected")
		}
	},

	//rainbow
	rb: function (args) {
		//set rainbow message
		var content = args.join(" ")
		client.send(content.rainbow)
	},

	//notify
	notify: function (args) {
		//change notify setting
		if ((args[0] === "all") || (args[0] === "mention") || (args[0] === "none")) {
			settings.change("notify", args[0])
			out.status("notify setting changed")
		} else {
			out.blank("try /notify <all / mention / none>")
		}
	},

	//mention
	m: function (args) {
		//mention user
		if (global.connection_status) {
			if (args[0]) {
				client.mention(args[0])
			} else {
				out.blank("try /m <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//kick
	kick: function (args) {
		//kick user
		if (global.connection_status) {
			if (args[0]) {
				client.kick(args[0])
			} else {
				out.blank("try /kick <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//ban
	ban: function (args) {
		//ban user
		if (global.connection_status) {
			if (args[0]) {
				client.ban(args[0])
			} else {
				out.blank("try /ban <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//unban
	unban: function (args) {
		//unban user
		if (global.connection_status) {
			if (args[0]) {
				client.unban(args[0])
			} else {
				out.blank("try /unban <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//mute
	mute: function (args) {
		//mute user
		if (global.connection_status) {
			if (args[0]) {
				client.mute(args[0])
			} else {
				out.blank("try /mute <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//unmute
	unmute: function (args) {
		//mute user
		if (global.connection_status) {
			if (args[0]) {
				client.unmute(args[0])
			} else {
				out.blank("try /mute <nick>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//chane permission
	level: function (args) {
		if (global.connection_status) {
			if (args[0] && args[1]) {
				client.level(args)
			} else {
				out.blank("try /level <nick> <1-5>")
			}
		} else {
			out.alert("you must be connected")
		}
	},

	//d1
	d1: function () {
		client.connect("localhost")
	},

	//d2
	d2: function () {
		if (global.connection_status) {
			client.long_list()
		} else {
			out.alert("you must be connected")
		}
	},
}