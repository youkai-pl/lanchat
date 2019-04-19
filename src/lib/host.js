//import
const http = require("http").Server()
const io = require("socket.io")(http)
const { RateLimiterMemory } = require("rate-limiter-flexible")
const out = require("./out")
const db = require("./db")
const config = require("./config")

//variables
var database
var users = []
var status

module.exports.start = function () {

	//start server
	if (status) {
		out.alert("server already running")
	} else {

		//load database
		out.status("loading database")
		database = db.load()

		if (!db.get(config.nick)) {
			db.add({
				nickname: config.nick,
				level: 5,
				ip: "127.0.0.1"
			})
		}

		//set permissions
		db.write(config.nick, "level", 5)

		out.status("starting server")

		//rate limiter
		rateLimiter = new RateLimiterMemory(
			{
				points: config.ratelimit,
				duration: 1,
			})

		//check motd
		if (!config.motd) {
			out.status("motd not found")
		}

		//start listen
		http.listen(config.port, function () {
			out.status("server started")
			status = true
		})

		//SOCKET EVENTS
		io.on("connection", function (socket) {

			//login
			socket.on("login", function (nick) {

				log(nick + " connected")

				//detect blank nick
				if (!nick) {
					nick = "default"
				}

				//shortening long nick
				if (nick.length > 15) {
					nick = nick.substring(0, 15)
				}

				//add user to database
				if (!db.get(nick)) {
					db.add({
						nickname: nick,
						level: 2,
					})
				}

				//save user ip adress
				db.write(nick, "ip", socket.handshake.address)

				//check ban
				var ban
				for (var i = 0; i < database.length; i++) {
					if (database[i].level === 0) {
						if (database[i].ip === socket.handshake.address) {
							ban = true
						}
					}
				}
				if (db.get(nick).level === 0) {
					ban = true
				}
				if (ban) {
					log(nick + "is banne")
					socket.emit("banned")
					io.sockets.connected[socket.id].disconnect()
				}

				if (db.get(nick).pass) {
					log(nick + "need auth")
					socket.emit("needAuth")
				} else {
					login(nick, socket)
				}
			})
		})
	}
}

//log
function log(content) {
	if (config.log) {
		out.blank(content)
	}
}

//login
function login(nick, socket) {

	if (users.findIndex(x => x.nickname === nick) !== -1) {

		log(nick + " alredy taken")
		socket.emit("taken")
		io.sockets.connected[socket.id].disconnect()
	} else {
		users.push({
			nickname: nick,
			status: "online"
		})

		log(nick + " joined")
		socket.join("main")
		socket.emit("joined")

		if (config.emit) {
			socket.emit("motd", config.motd)
		}
	}
}