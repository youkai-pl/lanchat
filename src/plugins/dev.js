const client = require("../lib/client")
var global = require("../lib/global")

module.exports = {

	//global
	global: function () {
		console.log(global)
	},

	//d1
	d1: function () {
		client.connect("localhost")
	}
}