const out = require("../lib/out")

module.exports = {

	//help
	help: function () {
		out.blank("/dwl - list of avabile plugins")
	},

	//dwn-list
	dwl: function () {
		var l = []
		l.push("")
		l.push(" Version   Name   Description")
		l.push("-----------------------------------------")
		l.push(" 0.11.0   dev    developer commands")
		l.push(" 0.11.0   dwl    list of avabile plugins")
		out.blank(l.join("\n"))
	}
}