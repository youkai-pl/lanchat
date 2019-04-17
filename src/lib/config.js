//import
const fs = require("fs")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME)
const path = home + "/.lanchat/"
var config = {}

module.exports = {

	//CONFIG//
	//load config
	load: function () {
		if (load()) {
			return true
		}
	},

	//config write
	write: function (key, value) {
		config[key] = value
		fs.writeFileSync(path + "config.json", JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		module.exports[key] = config[key]
	}
}

//load config file
function load() {

	//check dir
	if (!fs.existsSync(home + "/.lanchat")) {
		fs.mkdirSync(home + "/.lanchat")
	}

	//check config
	if (!fs.existsSync(path + "config.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "config.json", '{"nick":"default","port":"2137","notify":"mention", "devlog":false, "attemps": "5", "ratelimit": "15"}')
	}

	//check host database
	if (!fs.existsSync(path + "db.json")) {
		// eslint-disable-next-line quotes
		fs.writeFileSync(path + "db.json", '[]')
	}

	//load config
	try {

		//load file
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

		//load motd
		if (fs.existsSync(home + "/.lanchat/motd.txt")) {
			config.motd = fs.readFileSync(home + "/.lanchat/motd.txt", "utf8")
		}

		//export
		for (i in config) {
			module.exports[i] = config[i]
		}

		//return
		return true

	} catch (err) {

		//return
		return false
	}
}