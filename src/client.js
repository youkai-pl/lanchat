//CLIENT
var http = require('http').Server();
var io = require('socket.io')(http);
var net = require('net');
const ip = require("ip");
const rl = require("./rl").rl
const fn = require("./common")
const out = require("./out")
var settings = require("./settings")
var socket
var connection_status
var server_status

module.exports = {

    send: function (message) {
        if (connection_status) {
            socket.emit('message', message);
        }
    },

    connect: function (ip) {
        if (isEmptyOrSpaces(ip)) {
            out.blank("Try: /connect <ip>")
        } else {

            if (connection_status) {
                out.alert("you already connected")
            } else {
                out.status("connecting")
                testPort(settings.port, ip, function (e) {
                    if (e === "failure") {
                        out.alert("connection error")
                    } else {
                        socket = require('socket.io-client')('http://' + ip + ":" + settings.port);
                        listen()
                        connection_status = true;
                        out.status("connected")
                        var msg = { content: " join the chat", nick: settings.nick }
                        send_status(msg);
                    }
                })
            }
        }
    },

    disconnect: function () {
        if (connection_status) {
            var msg = { content: " left the chat", nick: settings.nick }
            send_status(msg);
            socket.disconnect()
            connection_status = false;
            if (server_status) {
                out.status("you are disconnected but server is still working")
            } else {
                out.status("disconnected")
            }
        } else {
            out.alert("you are not connected")
        }
    },

    set_nick: function (nick) {
        settings.nick = nick;
    },

    host: function () {
        out.status("starting server")
        testPort(settings.port, "127.0.0.1", function (e) {
            if (e === "failure") {
                io.on('connection', function (socket) {
                    socket.on('disconnect', function () {
                        //nothing
                    });
                    socket.on('message', function (msg) {
                        if (msg.hasOwnProperty('content') && msg.hasOwnProperty('nick')) {
                            socket.broadcast.emit('message', msg);
                        }
                    });
                });
                http.listen(settings.port, function () {
                    out.status("Server running");
                    out.status("Local network IP: " + ip.address());
                });
                socket = require('socket.io-client')('http://localhost:' + settings.port);
                connection_status = true;
                server_status = true;
                listen();
            } else {
                out.alert("Server is already running on this PC")
            }
        })
    }
}

function testPort(port, host, cb) {
    net.createConnection(port, host).on("connect", function (e) {
        cb("success", e);
    }).on("error", function (e) {
        cb("failure", e);
    });
}

function listen() {
    socket.on('message', function (msg) {
        out.message(msg)
    })
}

function isEmptyOrSpaces(str) {
    return str === null || typeof str === "undefined" || str.match(/^ *$/) !== null;
}

function send_status(msg) {
    socket.emit('message', msg);
}