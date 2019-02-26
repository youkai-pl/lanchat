//NOTIFY
const notifier = require("node-notifier")
const path = require("path")
var settings = require("./settings")

module.exports = {

	//message
	message: function (msg) {
		if (settings.notify === "all") {
			notifier.notify(
				{
					title: "Lanchat",
					message: "New Message",
					sound: false,
					icon: path.join(__dirname, "icon.png")
				}
			)
		}
	},

	//mention
	mention: function (msg) {
		if (settings.notify === "mention") {
			notifier.notify(
				{
					title: "Lanchat",
					message: "Somebody mentioned you",
					sound: false,
					icon: path.join(__dirname, "icon.png")
				}
			)
		}
	}
}

