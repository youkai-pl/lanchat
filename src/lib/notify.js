//import
const notifier = require("node-notifier")
const path = require("path")
const files = require("./files")

//NOTIFY
module.exports = {

	//message
	message: function () {

		//send all messages notify
		if (files.config.notify === "all") {
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
		if (files.config.notify === "mention" || files.config.notify === "all") {
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

