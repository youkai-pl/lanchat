//import
const fs = require("fs")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME)
const path = home + "/.lanchat/"
var database = {}

module.exports = {

	//load database
	load: function () {
		database = JSON.parse(fs.readFileSync(path + "db.json", "utf8"))

		//export db
		return database
	},

	//add user to db
	add: function (user) {

		if (!user.hasOwnProperty("level")) {
			user.lock = 2
		}

		if (!user.hasOwnProperty("ip")) {
			user.ip = false
		}

		if (!user.hasOwnProperty("pass")) {
			user.pass = false
		}

		database.push({
			nick: user.nick,
			level: user.level,
			ip: user.ip,
			pass: user.pass
		})
		save()
	},

	//write to db
	write: function (nick, key, value) {
		var index = database.findIndex(x => x.nick === nick)
		database[index][key] = value
		save()
	},

	//get user
	get: function (nick) {
		var index = database.findIndex(x => x.nick === nick)
		if (index === -1) {
			user = false
		} else {
			user = database[index]
		}
		return user
	}
}

function save() {
	fs.writeFileSync(path + "db.json", JSON.stringify(database), function (err) {
		if (err) return console.log(err)
	})
}