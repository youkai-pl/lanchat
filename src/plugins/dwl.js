const out = require("../lib/out")

module.exports = {

	// help
	help: function () {
		out.blank("/dwl        - list avabile plugins")
	},

	// dwn-list
	dwl: function () {
		var l = []
		l.push("Name            Description               Version")
		l.push("-------------------------------------------------")
		l.push("dev           - developer commands      - 1.0.0+")
		l.push("dwl           - list of avabile plugins - 1.0.0+")
		l.push("theme_example - example theme plugin    - 1.1.0+")
		out.blank(l.join("\n"))
	}
}