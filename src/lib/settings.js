//import
const fs = require("fs")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME + "/.local/share")
const path = home + "/lanchat/config.json"
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
		fs.writeFileSync(path, JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		load()
	},

	//change notify settings
	notifyChange: function (value) {
		config["notify"] = value
		fs.writeFileSync(path, JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		load()
	}
}

//load config file
function load() {
	if (!fs.existsSync(home + "/lanchat")) {
		fs.mkdirSync(home + "/lanchat")
		// eslint-disable-next-line quotes
		fs.writeFileSync(path, '{"nick":"default","port":"2137","notify":"mention","motd":""}')
	}

	try {
		//export config
		config = JSON.parse(fs.readFileSync(path, "utf8"))
		module.exports.nick = config.nick
		module.exports.motd = config.motd
		module.exports.notify = config.notify
		module.exports.port = config.port
	} catch (err) {
		return false
	}
}