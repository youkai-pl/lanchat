//import
const client = require("./client")
const out = require("./out")
const dwn = require("./dwn")
const host = require("./host")
const config = require("./config")
const c = require("./colors")

//COMMANDS
module.exports = {

	//clear
	clear: function () {
		//clear window
		process.stdout.write("\033c")
	},

	//exit
	exit: function () {
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
			config.write("nick", args[0])

			//change nick on host
			if (client.connection) {
				client.nick()
			} else {
				out.blank("Your nickname is now " + c.blue + args[0] + c.reset)
			}
		}
	},

	//help
	help: function () {
		var help = []
		help.push("")
		help.push("/connect    - connect")
		help.push("/disconnect - disconnect")
		help.push("/host       - create server")
		help.push("/clear      - clear screen")
		help.push("/exit       - exit lanchat")
		help.push("/nick       - change nick")
		help.push("/list       - list users")
		help.push("/online     - change status")
		help.push("/afk        - change status")
		help.push("/dnd        - change status")
		help.push("/notify     - change notifications settings")
		help.push("/mention    - mention user")
		help.push("/login      - login on locked account")
		help.push("/lock       - lock account")
		help.push("/kick       - kick user")
		help.push("/ban        - ban user")
		help.push("/unban      - unban user")
		help.push("/mute       - mute user")
		help.push("/unmute     - unmute user")
		help.push("/level      - change user permission")
		help.push("/dwn        - donwload plugin")
		help.push("/dwd        - delete plugin")
		out.blank(help.join("\n"))
	},

	//connect
	connect: function (args) {
		client.connect(args[0])
	},

	//host
	host: function () {
		if (client.connection) {
			out.alert("disconnect from current server first")
		} else {
			host.start()
			client.connect("127.0.0.1")
		}
	},

	//login
	login: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.auth(args[0])
			} else {
				out.blank("try /login <password>")
			}
		}
	},

	//register
	lock: function (args) {
		if (checkConnection()) {
			client.lock(args)
		}
	},

	//disconnect
	disconnect: function () {
		if (checkConnection()) {
			client.disconnect()
		}
	},

	//list
	list: function () {
		if (checkConnection()) {
			client.list()
		}
	},

	//afk
	afk: function () {
		if (checkConnection()) {
			config.write("status", "afk")
			client.changeStatus("afk")
			out.status("you are afk")
		}
	},

	//online
	online: function () {
		if (checkConnection()) {
			config.write("status", "online")
			client.changeStatus("online")
			out.status("you are online")
		}
	},

	//dnd
	dnd: function () {
		if (checkConnection()) {
			config.write("status", "dnd")
			client.changeStatus("dnd")
			out.status("you are dnd")
		}
	},

	//notify
	notify: function (args) {
		if ((args[0] === "all") || (args[0] === "mention") || (args[0] === "none")) {
			config.write("notify", args[0])
			out.status("notify setting changed")
		} else {
			out.blank("try /notify <all / mention / none>")
		}
	},

	//mention
	mention: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.mention(args[0])
			} else {
				out.blank("try /m <nick>")
			}
		}
	},

	//kick
	kick: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.kick(args[0])
			} else {
				out.blank("try /kick <nick>")
			}
		}
	},

	//ban
	ban: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.ban(args[0])
			} else {
				out.blank("try /ban <nick>")
			}
		}
	},

	//unban
	unban: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.unban(args[0])
			} else {
				out.blank("try /unban <nick>")
			}
		}
	},

	//mute
	mute: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.mute(args[0])
			} else {
				out.blank("try /mute <nick>")
			}
		}
	},

	//unmute
	unmute: function (args) {
		if (checkConnection()) {
			if (args[0]) {
				client.unmute(args[0])
			} else {
				out.blank("try /mute <nick>")
			}
		}
	},

	//chane permission
	level: function (args) {
		if (checkConnection()) {
			if (args[0] && args[1]) {
				client.level(args)
			} else {
				out.blank("try /level <nick> <1-5>")
			}
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

function checkConnection() {
	var check
	if (client.connection) {
		check = true
	} else {
		check = false
		out.alert("you must be connected")
	}
	return check
}