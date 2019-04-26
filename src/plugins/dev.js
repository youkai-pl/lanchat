const client = require("../lib/client")
const config = require("../lib/config")
const db = require("../lib/db")

module.exports = {

	// d1
	d1: function () {
		client.connect("localhost")
	},

	// global
	d2: function () {
		console.log(client.connection)
	},

	// config
	d3: function () {
		console.log(config)
	},

	// db
	d4: function(){
		console.log(db)
	},

	d5: function(){
		console.log(client.inprogress)
	}
}