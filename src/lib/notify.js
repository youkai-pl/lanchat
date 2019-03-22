//import
const notifier = require("node-notifier")
const path = require("path")
var settings = require("./settings")

//NOTIFY
module.exports = {

	//message
	message: function () {

		//send all messages notify
		if (global.notify === "all") {
			notifier.notify(
				{
					title: "Lanchat",
					message: "New Message",
					sound: false,
					icon: path.join(__dirname, "../icon.png")
				}
			)
		}
	},

	//mention
	mention: function () {
		//send mention notify
		if (global.notify === "mention" || global.notify === "all") {
			notifier.notify(
				{
					title: "Lanchat",
					message: "Somebody mentioned you",
					sound: false,
					icon: path.join(__dirname, "../icon.png")
				}
			)
		}
	}
}

