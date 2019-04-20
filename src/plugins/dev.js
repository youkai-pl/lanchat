const client = require("../lib/client")
const global = require("../lib/global")
const config = require("../lib/config")
const db = require("../lib/db")
const host = require("../lib/host")

module.exports = {

	//d1
	d1: function () {
		client.connect("localhost")
	},

	//global
	d2: function () {
		console.log(global)
	},

	//config
	d3: function () {
		console.log(config)
	},

	//db
	d4: function(){
		console.log(db)
	},

	//users
	d5: function () {
		console.log(host.stats)
	}
}