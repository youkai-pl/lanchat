//import
const client = require("./client")
const out = require("./out")
const dwn = require("./dwn")
const db = require("./db")
var global = require("./global")

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
			db.change("nick", args[0])
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
		help[1] = "/connect <ip> - connect to server"
		help[2] = "/disconnect - disconnect from server"
		help[3] = "/clear - clear window"
		help[4] = "/exit - exit Lan Chat"
		help[5] = "/nick <nickname> - set nickname"
		help[6] = "/list - connected users list"
		help[7] = "/afk - change status to afk"
		help[8] = "/online - change status to online"
		help[9] = "/dnd - do not disturb, mute all messages"
		help[10] = "/notify <all / mention / none> - change notify setting"
		help[11] = "/m <nick> - mention user"
		help[12] = "/login <password> - login"
		help[13] = "/lock - add or change password on host"
		help[14] = "/kick <nick> - kick user"
		help[15] = "/ban <nick> - ban user"
		help[16] = "/unban <nick> - unban user"
		help[17] = "/mute <nick> - mute user"
		help[18] = "/unmute <nick> - unmute user"
		help[19] = "/level <nick> <1-5> - change user permission level"
		help[20] = "/dwn <plugin name> - download plugin"
		help[21] = "/dwd <plugin name> - delete plugin"
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
	lock: function (args) {
		if (global.connection_status) {
			client.lock(args)
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
			db.change("notify", args[0])
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

	//plugin install
	dwn: function (args) {
		if (args[0]) {
			dwn.donwload(args[0])
		} else {
			out.blank("try /dwn <plugin name>")
		}
	},

	//plugin delet
	dwd: function (args) {
		if (args[0]) {
			dwn.delete(args[0])
		} else {
			out.blank("try /dwd <plugin name>")
		}
	}
}