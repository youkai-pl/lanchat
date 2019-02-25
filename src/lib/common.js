const net = require("net")

//COMMON
module.exports = {

	//get time
	time: function () {
		var date = new Date()
		var time = (("0" + date.getHours()).slice(-2) + ":" +
            ("0" + date.getMinutes()).slice(-2) + ":" +
            ("0" + date.getSeconds()).slice(-2))
		return time
	},

	testPort: function (port, host, cb) {
		net.createConnection(port, host).on("connect", function (e) {
			cb("success", e)
		}).on("error", function (e) {
			cb("failure", e)
		})
	},

	isEmptyOrSpaces: function (str) {
		return str === null || typeof str === "undefined" || str.match(/^ *$/) !== null
	}
}

