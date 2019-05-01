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
	mention: function (nick) {

		// send mention notify
		if (config.notify === "mention" || config.notify === "all") {
			notifier.notify(
				{
					title: "Lanchat",
					message: nick + " mentioned you",
					sound: true,
					icon: path.join(__dirname, "../icon.png")
				}
			)
		}
	}
}

