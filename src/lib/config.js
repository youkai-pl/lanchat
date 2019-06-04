// import
const fs = require("fs")

// variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME)
const path = home + "/.lanchat/"
var config = {}

module.exports = {

	// CONFIG//
	// load config
	load: function () {

		// check dir
		if (!fs.existsSync(home + "/.lanchat")) {
			fs.mkdirSync(home + "/.lanchat")
		}

		// check config
		if (!fs.existsSync(path + "config.json")) {
			// eslint-disable-next-line quotes
			fs.writeFileSync(path + "config.json", '{}')
		}

		// check host database
		if (!fs.existsSync(path + "db.json")) {
			// eslint-disable-next-line quotes
			fs.writeFileSync(path + "db.json", '[]')
		}

		// load config
		try {

			// load file
			config = JSON.parse(fs.readFileSync(path + "config.json", "utf8"))
			config.validate = true
			var save

			// valdate
			if (!config.hasOwnProperty("nick")) {
				config.nick = "default"
				save = true
			}
			if (!config.hasOwnProperty("port")) {
				config.port = 2137
				save = true
			}
			if (!config.hasOwnProperty("notify")) {
				config.notify = "mention"
				save = true
			}
			if (!config.hasOwnProperty("log")) {
				config.log = false
				save = true
			}
			if (!config.hasOwnProperty("ratelimit")) {
				config.ratelimit = 15
				save = true
			}
			if (!config.hasOwnProperty("attemps")) {
				config.attemps = 5
				save = true
			}
			if (!config.hasOwnProperty("socketlimit")) {
				config.socketlimit = 100
				save = true
			}
			if (!config.hasOwnProperty("lenghtlimit")) {
				config.lenghtlimit = 1500
				save = true
			}
			if (save) {
				saveConfig()
				config.validate = true
			}

			// load motd
			if (fs.existsSync(home + "/.lanchat/motd.txt")) {
				config.motd = fs.readFileSync(home + "/.lanchat/motd.txt", "utf8")
			}

			// set status
			config.status = "online"

			// export
			for (i in config) {
				module.exports[i] = config[i]
			}

			return config.validate

		} catch (err) {
			// return
			config.validate = false
		}
	},

	// config write
	write: function (key, value) {

		// change value
		config[key] = value

		// export
		module.exports[key] = config[key]
		saveConfig()
	}
}

function saveConfig() {
	var toWrtie = config
	delete toWrtie["validate"]
	delete toWrtie["motd"]
	delete toWrtie["status"]
	fs.writeFileSync(path + "config.json", JSON.stringify(toWrtie), (err) => {
		console.log(err)
	})
}