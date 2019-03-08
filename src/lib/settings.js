//import
const fs = require("fs")
const out = require("./out")
var global = require("./global")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME + "/.local/share")
const path = home + "/lanchat/"
var config = {}

//SETTINGS
module.exports = {

	//load config
	load: function () {
		if (load()) {
			return true
		}
	},

	//change nick
	nickChange: function (nick) {
		config["nick"] = nick
		global.nick = nick
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
	},

	//change notify settings
	notifyChange: function (value) {
		config["notify"] = value
		global.notify = notify
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
	},

	//write to db
	writedb: function (ip, key, value) {
		global.db[ip] = {[key]: value}
		fs.writeFileSync(path + "db.json", JSON.stringify(db), function (err) {
			if (err) return console.log(err)
		})
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

	//create host database
	if (!fs.existsSync(path + "db.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "db.json", '{}')
	}

	//load
	try {

		//load files
		config = JSON.parse(fs.readFileSync(path + "config.json", "utf8"))
		host = JSON.parse(fs.readFileSync(path + "host.json", "utf8"))
		db = JSON.parse(fs.readFileSync(path + "db.json", "utf8"))

		//valdate
		if (!config.hasOwnProperty("nick")) {
			return false
		}
		if (!config.hasOwnProperty("port")) {
			return false
		}
		if (!config.hasOwnProperty("notify")) {
			return false
		}
		if (!host.hasOwnProperty("rateLimit")) {
			return false
		}

		//export config
		global.nick = config.nick
		global.notify = config.notify
		global.port = config.port

		//export host config
		global.motd = motd()
		global.rateLimit = host.rateLimit

		//export db
		global.db = db

		//return
		return true

	} catch (err) {

		//return
		return false
	}
}

//load motd file
function motd() {
	if (fs.existsSync(home + "/lanchat/motd.txt")) {
		return fs.readFileSync(home + "/lanchat/motd.txt", "utf8")
	}
}