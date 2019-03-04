//import
const fs = require("fs")
var global = require("./global")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME + "/.local/share")
const path = home + "/lanchat/"
var config = {}

//SETTINGS
module.exports = {

	//load config
	load: function () {
		load()
	},

	//change nick
	nickChange: function (nick) {
		config["nick"] = nick
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		load()
	},

	//change notify settings
	notifyChange: function (value) {
		config["notify"] = value
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		load()
	}
}

//load config file
function load() {

	//create dir
	if (!fs.existsSync(home + "/lanchat")) {
		fs.mkdirSync(home + "/lanchat")
	}

	//create config
	if (!fs.existsSync(path + "config.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "config.json", '{"nick":"default","port":"2137","notify":"mention"}')
	}

	//create host config
	if (!fs.existsSync(path + "host.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "host.json", '{"rateLimit": "15"}')
	}

	//load and export config
	try {

		//load files
		config = JSON.parse(fs.readFileSync(path + "config.json", "utf8"))
		host = JSON.parse(fs.readFileSync(path + "host.json", "utf8"))

		//export config
		global.nick = config.nick
		global.notify = config.notify
		global.port = config.port

		//export host config
		global.motd = motd()
		global.rateLimit = host.rateLimit

	} catch (err) {
		return false
	}
}

//load motd file
function motd() {
	if (fs.existsSync(home + "/lanchat/motd.txt")) {
		return fs.readFileSync(home + "/lanchat/motd.txt", "utf8")
	}
}
