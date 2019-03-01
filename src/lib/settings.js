const fs = require("fs")
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
		var config = JSON.parse(fs.readFileSync(path, "utf8"))
		config["nick"] = nick
		fs.writeFileSync(path, JSON.stringify(config), function (err) {
			if (err) return console.log(err)
		})
		load()
	}
}

function load() {
	if (!fs.existsSync(home + "/lanchat")) {
		fs.mkdirSync(home + "/lanchat")
		// eslint-disable-next-line quotes
		fs.writeFileSync(path, '{"nick":"default","port":"2137","notify":"mention","motd":""}')
	}

	try {
		config = JSON.parse(fs.readFileSync(path, "utf8"))
		module.exports.nick = config.nick
		module.exports.motd = config.motd
		module.exports.notify = config.notify
		module.exports.port = config.port
	} catch (err) {
		return false
	}
}