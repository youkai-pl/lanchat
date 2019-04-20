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
var usersLimit = 0

module.exports.stats = { users, usersLimit }

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

				//check sockets limit
				if (config.socketlimit <= usersLimit) {
					log("socket limit reached")
					socket.emit("socketLimit")
					io.sockets.connected[socket.id].disconnect()
				} else {

					//check the availability of a nickname
					if (getId(nick)) {

						//if nick is take emit alert
						log(nick + " alredy taken")
						socket.emit("taken")
						io.sockets.connected[socket.id].disconnect()
					} else {

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
							log(nick + " is banned")
							socket.emit("banned")
							io.sockets.connected[socket.id].disconnect()
						}

						//check password
						if (db.get(nick).pass) {
							log(nick + " need auth")
							socket.emit("needAuth")
						} else {
							login(nick, socket)
						}
					}
				}
			})

			//disconnect
			socket.on("disconnect", function () {

				delUser(socket.id)
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

	addUser({
		id: socket.id,
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

//add user
function addUser(user) {

	//find empty space
	var index = users.findIndex(x => x.id === null)
	if (index !== -1) {
		users[index] = user
	} else {
		users.push(user)
	}
	usersLimit++
}

//get user
function getUser(id) {
	var user
	var index = users.findIndex(x => x.id === id)
	if (index === -1) {
		user = false
	} else {
		user = users[index]
	}
	return user
}

//get user index
function getIndex(id) {
	var index = users.findIndex(x => x.id === id)
	if (index === -1) {
		index = false
	}
	return index
}

//get user id
function getId(nick) {
	var userId
	var index = users.findIndex(x => x.nickname === nick)
	if (index === -1) {
		userId = false
	} else {
		userId = users[index].id
	}
	return userId
}

//delete user
function delUser(id) {
	var index = getIndex(id)
	if (index) {
		Object.keys(users[index]).forEach(v => users[index][v] = null)
		usersLimit--
	} else {
		log(id + " not exist")
	}
}

//emit user status
function emitStatus(socket) {

}