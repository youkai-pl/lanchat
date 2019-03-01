const fs = require("fs")
const path = __dirname + "./../config.json"
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
	if (!fs.existsSync(path)) {
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