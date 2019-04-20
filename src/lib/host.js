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
				nick: config.nick,
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
					socket.emit("socketLimit")
					io.sockets.connected[socket.id].disconnect()
				} else {

					//check the availability of a nickname
					if (checkNickAvabile(nick, socket)) {

						//add user to database
						if (!db.get(nick)) {
							db.add({
								nick: nick,
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
							socket.emit("banned")
							io.sockets.connected[socket.id].disconnect()
						}

						//check password
						if (db.get(nick).pass) {
							socket.emit("needAuth")
						} else {
							login(nick, socket)
						}
					}
				}
			})

			//disconnect
			socket.on("disconnect", function () {
				emitStatus("left", socket)
				delUser(socket.id)
			})

			//auth
			socket.on("auth", function (nick, password) {
				//check password
				if (db.get(nick).pass === password) {
					//check nick avability
					if (checkNickAvabile(nick, socket)) {
						login(nick, socket)
					}
				} else {
					socket.emit("wrongPass")
				}
			})

			//register
			socket.on("register", function (nick, password) {
				if (getByNick(nick)) {
					if (password) {
						db.write(nick, "pass", password)
						socket.emit("passChanged")
					}
				}
			})

			//message
			socket.on("message", async (content) => {
				//check message
				if (typeof content === "string" || content instanceof String) {

					if (db.get(getUser(socket.id).nick).level !== 1) {
						try {

							//flood block
							await rateLimiter.consume(socket.handshake.address)

							//block long messages
							if (content.length < config.lenghtlimit) {

								//emit message
								socket.broadcast.to("main").emit("message",{
									nick: getUser(socket.id).nick,
									content: content
								})

							} else {
								socket.emit("tooLong")

							}
						} catch (rejRes) {
							socket.emit("flood")
						}
					} else {
						socket.emit("muted")
					}
				}
			})
		})
	}
}

//login
function login(nick, socket) {

	addUser({
		id: socket.id,
		nick: nick,
		status: "online"
	})

	socket.join("main")
	socket.emit("joined")

	if (config.motd) {
		socket.emit("motd", config.motd)
	}

	emitStatus("join", socket)
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

//get user by id
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

//get user by id
function getByNick(nick) {
	var user
	var index = users.findIndex(x => x.nick === nick)
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

//delete user
function delUser(id) {
	var index = getIndex(id)
	if (index) {
		Object.keys(users[index]).forEach(v => users[index][v] = null)
		usersLimit--
	}
}

//emit user status
function emitStatus(type, socket) {

	if (getUser(socket.id)) {

		if (type === "join") {
			socket.broadcast.to("main").emit("isJoin", getUser(socket.id).nick)
		}

		if (type === "left") {
			socket.broadcast.to("main").emit("isLeft", getUser(socket.id).nick)
		}

		if (type === "online") {
			socket.broadcast.to("main").emit("isOnline", getUser(socket.id).nick)
		}

		if (type === "dnd") {
			socket.broadcast.to("main").emit("isDnd", getUser(socket.id).nick)
		}

		if (type === "afk") {
			socket.broadcast.to("main").emit("isAfk", getUser(socket.id).nick)
		}
	}
}

//chech nick avability
function checkNickAvabile(nick, socket) {
	var value
	if (getByNick(nick)) {
		socket.emit("nickTaken")
		io.sockets.connected[socket.id].disconnect()
		value = false
	} else {
		value = true
	}
	return value
}