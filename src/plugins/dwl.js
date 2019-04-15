const out = require("../lib/out")

module.exports = {

	//help
	help: function () {
		out.blank("/dwl - list of avabile plugins")
	},

	//dwn-list
	dwl: function(){
		var l = []
		l[0] = ""
		l[1] = "Version   Name   Description"
		l[2] = "-----------------------------------------"
		l[3] = " 0.11.0   host   lanchat server"
		l[4] = " 0.11.0   dev    developer commands"
		l[5] = " 0.11.0   dwl    list of avabile plugins"
		out.blank(l.join("\n"))
	}
}