// import
const notifier = require("node-notifier")
const path = require("path")
const config = require("./config")

// NOTIFY
module.exports = {

	// message
	message: function () {

		// send all messages notify
		if (config.notify === "all") {
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

	// mention
	mention: function () {
		// send mention notify
		if (config.notify === "mention" || config.notify === "all") {
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

