//plugins
const fs = require("fs")
const path = __dirname + "../../plugins"
const self = __dirname + "../../main.js"
var run

//load plugins
module.exports.load = function () {
	if (fs.existsSync(path) && fs.existsSync(self)) {
		run = require("require-all")(path)
	} else {
		run = false
	}
	module.exports.run = run
}

