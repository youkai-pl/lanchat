//import
const fs = require("fs")
var global = require("./global")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME)
const path = home + "/.lanchat/"
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
	change: function (type, nick) {
		config[type] = nick
		global[type] = nick
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
	},

	//load database
	loadDb: function () {
		db = JSON.parse(fs.readFileSync(path + "db.json", "utf8"))
		//export db
		global.db = db
	},

	//add user to db
	addUser: function (nick) {
		global.db.push({
			nickname: nick,
		})

		//write to file
		fs.writeFileSync(path + "db.json", JSON.stringify(db), function (err) {
			if (err) return console.log(err)
		})
	},

	//write to db
	writedb: function (nick, key, value) {
		var index = global.db.findIndex(x => x.nickname === nick)
		global.db[index][key] = value
		fs.writeFileSync(path + "db.json", JSON.stringify(db), function (err) {
			if (err) return console.log(err)
		})
	}
}

//load config file
function load() {

	//create dir
	if (!fs.existsSync(home + "/.lanchat")) {
		fs.mkdirSync(home + "/.lanchat")
	}

	//create config
	if (!fs.existsSync(path + "config.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "config.json", '{"nick":"default","port":"2137","notify":"mention", "devlog":false, "attemps": "5", "ratelimit": "15"}')
	}

	//create host database
	if (!fs.existsSync(path + "db.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "db.json", '[]')
	}

	//load
	try {

		//load files
		config = JSON.parse(fs.readFileSync(path + "config.json", "utf8"))

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
		if (!config.hasOwnProperty("devlog")) {
			return false
		}
		if (!config.hasOwnProperty("ratelimit")) {
			return false
		}
		if (!config.hasOwnProperty("attemps")) {
			return false
		}

		//export config
		global = Object.assign(global, config)
		global.motd = motd()

		//return
		return true

	} catch (err) {

		//return
		return false
	}
}

//load motd file
function motd() {
	if (fs.existsSync(home + "/.lanchat/motd.txt")) {
		return fs.readFileSync(home + "/.lanchat/motd.txt", "utf8")
	}
}