//HOST
const fn = require("./common")
const out = require("./out")
const colors = require('colors')
const client = require("./client")
const http = require('http').Server()
const io = require('socket.io')(http)
var settings = require("./settings")
var global = require("./global")

module.exports = {
    start: function () {
        out.status("starting server")
        fn.testPort(settings.port, "127.0.0.1", function (e) {
            if (e === "failure") {
                http.listen(settings.port, function () {
                    out.status("Server running");
                });
                run();
                global.server_status = true;
                client.connect("localhost")
            } else {
                out.alert("Server is already running on this PC")
            }
        })
    }
}

//ALL SERVER THINGS
function run() {

    io.on('connection', function (socket) {

        //login
        socket.on('login', function (nick) {
            var id = socket.id.toString()
            if (nick.length > 15) {
                nick = nick.substring(0, 15)
            }
            var user = {
                nickname: nick,
                status: "online"
            }
            global.users[id] = user
        })

        //logoff
        socket.on("disconnect", function () {
            var id = socket.id.toString()
            delete global.users[id]
        })

        //change nick
        socket.on("nick", function (nick) {
            var id = socket.id.toString()
            var old = global.users[id].nickname
            if (global.users.hasOwnProperty(id)) {
                if (nick.length > 15) {
                    nick = nick.substring(0, 15)
                }
                global.users[id].nickname = nick;
                socket.broadcast.emit('return', old.blue + " change nick to " + nick.blue)
            }
        })

        //list
        socket.on("list", function () {
            var list = []
            var status
            list[0] = "\nUSER LIST"

            for (i = 1; i < Object.keys(global.users).length + 1; i++) {
                var a = global.users[Object.keys(global.users)[i - 1]]
                if (a.status === "online") {
                    status = a.status.green
                }
                if (a.status === "afk") {
                    status = a.status.yellow
                }
                list[i] = a.nickname.blue + " (" + status + ")"
            }

            var table = list.join("\n")
            socket.emit('return', table)

        })


        //afk
        socket.on('afk', function (nickname) {
            var id = socket.id.toString()
            if (global.users.hasOwnProperty(id)) {
                global.users[id].status = "afk"
                var msg = {
                    content: "is afk",
                    nick: nickname
                }
                socket.broadcast.emit('status', msg)
            }

        })

        //online
        socket.on('online', function (nickname) {
            var id = socket.id.toString()
            if (global.users.hasOwnProperty(id)) {
                global.users[id].status = "online"
                var msg = {
                    content: "is online",
                    nick: nickname
                }
                socket.broadcast.emit('status', msg)
            }
        })

        //message
        socket.on('message', function (msg) {
            if (msg) {
                if (msg.hasOwnProperty('content') && msg.hasOwnProperty('nick')) {
                    if (msg.content !== "") {
                        if (msg.nick.length > 15) {
                            msg.nick = msg.nick.substring(0, 15)
                        }
                        socket.broadcast.emit('message', msg)
                    }
                }
            }
        });

        //status
        socket.on('status', function (msg) {
            if (msg) {
                if (msg.hasOwnProperty('content') && msg.hasOwnProperty('nick')) {
                    if (msg.nick.length > 15) {
                        msg.nick = msg.nick.substring(0, 15)
                    }
                    socket.broadcast.emit('status', msg)
                }
            }
        });

        //return
        socket.on('return', function (msg) {
            socket.emit('return', msg)
        });
    });
}