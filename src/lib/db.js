//import
const fs = require("fs")

//variables
const home = process.env.APPDATA || (process.platform == "darwin" ? process.env.HOME + "Library/Preferences" : process.env.HOME)
const path = home + "/.lanchat/"
var db = {}

module.exports = {

	//load database
	load: function () {
		db = JSON.parse(fs.readFileSync(path + "db.json", "utf8"))

		//export db
		return db
	},

	//add user to db
	add: function (nick) {
		db.push({
			nickname: nick,
		})

		//write to file
		fs.writeFileSync(path + "db.json", JSON.stringify(db), function (err) {
			if (err) return console.log(err)
		})
	},

	//write to db
	write: function (nick, key, value) {
		var index = db.findIndex(x => x.nickname === nick)
		db[index][key] = value
		fs.writeFileSync(path + "db.json", JSON.stringify(db), function (err) {
			if (err) return console.log(err)
		})
	}
}