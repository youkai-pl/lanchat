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
var rateLimiter

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
		db.write(config.nick, "level", 4)

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
					socket.emit("nickShortened")
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
								level: 1,
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
						} else {
							//check password
							if (db.get(nick).pass) {
								socket.emit("needAuth")
							} else {
								login(nick, socket)
							}
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
				if (!getUser(socket.id)) {
					//check password
					if (db.get(nick).pass === password) {
						//check nick avability
						if (checkNickAvabile(nick, socket)) {
							login(nick, socket)
							socket.emit("loginSucces")
						}
					} else {
						socket.emit("wrongPass")
					}
				} else {
					socket.emit("alreadySigned")
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
			socket.on("message", function (content) {
				verify(socket, function () {
					//check message
					if (typeof content === "string" || content instanceof String) {

						//change permission
						if (!checkMute(socket.id)) {

							//block long messages
							if (content.length < config.lenghtlimit) {

								//emit message
								socket.broadcast.to("main").emit("message", {
									nick: getUser(socket.id).nick,
									content: content
								})

							} else {
								socket.emit("tooLong")
							}

						} else {
							socket.emit("muted")
						}
					}
				})
			})

			//mention
			socket.on("mention", function (nick) {
				verify(socket, function () {
					if (checkMute(socket.id)) {
						socket.emit("muted")
					} else {
						var id = getByNick(nick).id
						if (id) {
							socket.to(`${id}`).emit("mentioned", getUser(socket.id).nick)
						} else {
							socket.emit("userNotExist")
						}
					}
				})
			})

			//nick
			socket.on("nick", function (nick) {
				verify(socket, function () {
					//shorten the long nick
					if (nick.length > 15) {
						nick = nick.substring(0, 15)
						socket.emit("nickShortened")
					}

					if (db.get(nick)) {
						socket.emit("nickTaken")
					} else {
						if (getByNick(nick)) {
							socket.emit("nickTaken")
						} else {
							var old = getUser(socket.id).nick
							writeUser(socket.id, "nick, nick")
							db.write(old, "nick", nick)
							socket.broadcast.to("main").emit("userChangeNick", old, nick)
							socket.emit("nickChanged")
						}
					}
				})
			})

			//list
			socket.on("list", function () {
				verify(socket, function () {
					socket.emit("usersList", users)
				})
			})

			//change status
			socket.on("changeStatus", function (status) {
				verify(socket, function () {
					var acceptable = ["online", "afk", "dnd"]
					if (acceptable.indexOf(status) !== -1) {
						writeUser(socket.id, "status", status)
						socket.emit("statusChanged")
						emitStatus(status, socket)
					} else {
						socket.emit("incorrectValue")
					}
				})
			})

			//mute
			socket.on("mute", function (nick) {
				checkPermission("mute", socket, nick, function () {
					db.write(nick, "mute", true)
					socket.emit("doneMute", nick)
				})
			})

			//unmute
			socket.on("unmute", function (nick) {
				checkPermission("unmute", socket, nick, function () {
					db.write(nick, "mute", false)
					socket.emit("doneUnMute", nick)
				})
			})

			//kick
			socket.on("kick", function (nick) {
				checkPermission("kick", socket, nick, function () {
					if (getByNick(nick)) {
						io.sockets.connected[getByNick(nick).id].disconnect()
					} else {
						socket.emit("userNotExist")
					}
				})
			})

			//ban
			socket.on("ban", function (nick) {
				checkPermission("ban", socket, nick, function () {
					db.write(nick, "level", 0)
					io.sockets.connected[getByNick(nick).id].disconnect()
					socket.emit("doneBan", nick)
				})
			})

			//unban
			socket.on("unban", function (nick) {
				checkPermission("unban", socket, nick, function () {
					db.write(nick, "level", 1)
					socket.emit("doneUnBan", nick)
				})
			})

			//unban
			socket.on("setPermission", function (nick, level) {
				level = parseInt(level)
				if (level >= 1 && level <= 4) {
					checkPermission("level", socket, nick, function () {
						db.write(nick, "level", level)
						socket.emit("doneSetPermission", nick, level)
					})
				} else {
					socket.emit("incorrectValue")
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

//write to user
function writeUser(id, key, value) {
	var back
	var index = getIndex(id)
	if (index) {
		users[index][key] = value
		back = true
	} else {
		back = false
	}
	return back
}

//delete user
function delUser(id) {
	var index = getIndex(id)
	if (index) {
		Object.keys(users[index]).forEach(v => users[index][v] = null)
		usersLimit--
	}
}

//check permission
function checkPermission(type, socket, nick, callback) {
	verify(socket, function () {
		var permission
		var check
		if (db.get(nick)) {
			if (type === "mute" || type === "unmute" || type === "ban" || type === "unban" || type === "kick") {
				permission = 2
			}
			if (type === "level") {
				permission = 3
			}

			var level = db.get(getUser(socket.id).nick).level
			if (level >= permission && level > db.get(nick).level) {
				check = true
			} else {
				check = false
				socket.emit("noPermission")
			}
		} else {
			check = false
			socket.emit("userNotExist")
		}
		if (check) {
			callback()
		}
	})
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

//check mute
function checkMute(id) {
	return db.get(getUser(id).nick).mute
}

//verify
async function verify(socket, callback) {
	if (getUser(socket.id)) {
		try {

			//flood block
			await rateLimiter.consume(socket.handshake.address)

			callback()

		} catch (rejRes) {
			socket.emit("flood")
		}
	} else {
		socket.emit("notSigned")
	}
}