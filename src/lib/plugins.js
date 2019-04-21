//plugins
const fs = require("fs")
const path = __dirname + "../../plugins"
var run

//load plugins
module.exports.load = function () {
	if (fs.existsSync(path)) {
		run = require("require-all")(path)
		console.log("test")
	} else {
		console.log("test2")
		run = false
	}
	module.exports.run = run
}

