const dgram = require("dgram")
const listen = dgram.createSocket("udp4")
const broadcast = dgram.createSocket("udp4")
const out = require("./out")

// variables
var list = []
var status

module.exports = {

	// listen
	listen: function () {

		listen.on("error", (err) => {
			if (err.code === "EADDRINUSE") {
				out.warning("Port 2138 is busy. Lanchat can't listen for hosts.")
			}
		})

		listen.on("listening", () => {
			status = true
		})

		listen.on("message", (msg, rinfo) => {
			if (list.indexOf(rinfo.address) === -1) {
				out.status("Host detected in LAN: " + rinfo.address)
				list.push(rinfo.address)
			}
		})

		try {
			listen.bind({ port: 2138 })
		} catch (err) { }
	},

	// stop listen
	close: function () {
		if (status) {
			listen.close()
			status = false
		}
	},

	// broadcast
	broadcast: function () {
		broadcast.on("listening", () => {
			broadcast.setBroadcast(true)
		})

		const message = "test"

		setInterval(() => {
			broadcast.send(message, 0, message.length, 2138, "255.255.255.255")
		}, 500)
	},

	// list
	list: function () {
		for (var i = 0; i < list.length; i++) {
			out.hostsList({ n: i + 1, ip: list[i] })
		}
	}
}
