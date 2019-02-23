//CLIENT
const fn = require("./common")
const out = require("./out")
const colors = require('colors');
var settings = require("./settings")
var global = require("./global")

module.exports = {

    //send
    send: function (content) {
        var msg = {
            content: content,
            nick: settings.nick
        }
        out.message(msg)
        if (global.connection_status) {
            socket.emit('message', msg)
        }
    },

    //status
    status: function (msg) {
        status(msg)
    },

    //nick
    nick: function () {
        socket.emit('nick', settings.nick)
    },

    //connect
    connect: function (ip) {
        if (fn.isEmptyOrSpaces(ip)) {
            out.blank("Try: /connect <ip>")
        } else {
            if (global.connection_status || global.reconnection) {
                out.alert("you already connected")
            } else {
                out.status("connecting")

                //create socket
                socket = require('socket.io-client')('http://' + ip + ":" + settings.port, {
                    reconnection: true
                })

                //connect
                socket.on('connect', function () {
                    if (!global.reconnection) {
                        listen()
                        global.connection_status = true
                        global.safe_disconnect = false
                        out.status("connected")
                        login()
                        status({
                            content: "join the chat",
                            nick: settings.nick
                        })
                    } else {
                        login()
                        global.connection_status = true
                        global.safe_disconnect = false

                    }

                });

                //handle connection error
                socket.on('connect_error', function () {
                    out.alert("connection error")
                })

                //handle disconnect
                socket.on('disconnect', function () {
                    if (global.safe_disconnect !== true) {
                        out.alert("disconnected")
                        global.reconnection = true
                        global.connection_status = false
                    }
                })

                //handle reconnect
                socket.on('reconnect', () => {
                    out.status("reconnected")
                    global.reconnection = true
                });

            }
        }
    },

    //disconnect
    disconnect: function () {
        if (global.connection_status) {
            socket.emit('status', settings.nick)
            global.safe_disconnect = true
            socket.disconnect()
            global.connection_status = false
            status({
                content: "leave the chat",
                nick: settings.nick
            })
            if (global.server_status) {
                out.status("you are disconnected but server is still working")
            } else {
                out.status("disconnected")
            }
        } else {
            out.alert("you are not connected")
        }
    },

    //list
    list: function () {
        socket.emit('list')
    },

    //afk
    afk: function () {
        socket.emit('afk', settings.nick)
    },

    //online
    online: function () {
        socket.emit('online', settings.nick)
    }
}

//FUNCTIONS

//listen
function listen() {
    socket.on('message', function (msg) {
        out.message(msg)
    })

    socket.on('status', function (msg) {
        out.user_status(msg)
    })

    socket.on('return', function (msg) {
        out.blank(msg)
    })
}

//status
function status(msg) {
    if (global.connection_status) {
        socket.emit('status', msg);
    }
}

//login
function login() {
    socket.emit('login', settings.nick)
}