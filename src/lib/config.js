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
			config.validate = true

			//valdate
			if (!config.hasOwnProperty("nick")) {
				config.validate = false
			}
			if (!config.hasOwnProperty("port")) {
				config.validate = false
			}
			if (!config.hasOwnProperty("notify")) {
				config.validate = false
			}
			if (!config.hasOwnProperty("log")) {
				config.validate = false
			}
			if (!config.hasOwnProperty("ratelimit")) {
				config.validate = false
			}
			if (!config.hasOwnProperty("attemps")) {
				config.validate = false
			}

			//load motd
			if (fs.existsSync(home + "/.lanchat/motd.txt")) {
				config.motd = fs.readFileSync(home + "/.lanchat/motd.txt", "utf8")
			}

			//set status
			config.status = "online"

			//export
			for (i in config) {
				module.exports[i] = config[i]
			}

		} catch (err) {

			//return
			config.validate = false
		}
	},

	//config write
	write: function (key, value) {

		//change value
		config[key] = value

		//delete temporary keys
		var toWrtie = config
		delete toWrtie["validate"]
		delete toWrtie["motd"]
		delete toWrtie["status"]

		//save to file
		fs.writeFileSync(path + "config.json", JSON.stringify(toWrtie), function (err) {
			if (err) return console.log(err)
		})

		//export
		module.exports[key] = config[key]
	}
}