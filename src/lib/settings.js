const fs = require("fs")

//SETTINGS
module.exports = {
	motd: function () {
		try {
			var motd = fs.readFileSync(__dirname + "./../motd.txt")
			return motd
		} catch (err) {
			return false
		}
	}
}

module.exports.nick = "default"
module.exports.port = "2137"
module.exports.notify = "mention"