//CLIENT
var settings = require("./settings")
var http = require('http').Server();
var io = require('socket.io')(http);
var net = require('net');
var socket
var connection_status
const read = require("./rl")
const rl = read.rl

module.exports = {
    connect: function (ip) {
        out("[#] connecting".green)
        testPort(settings.port, ip, function (e) {
            if (e === "failure") {
                out("[#] connection error".red)
            } else {
                socket = require('socket.io-client')('http://' + ip + ":" + settings.port);
                listen()
                connection_status = true;
                out("[#] connected".green)
            }
        })
    },

    send: function (msg) {
        if (connection_status) {
            socket.emit('message', msg);
        }
    },

    set_nick: function (nick) {
        settings.nick = nick;
    },

    host: function () {
        testPort(settings.port, "127.0.0.1", function (e) {
            if (e === "failure") {
                io.on('connection', function (socket) {
                    out('[#] user connected'.green);
                    socket.on('disconnect', function () {
                        out('[#] user disconnected'.red);
                    });
                    socket.on('message', function (msg) {
                        socket.broadcast.emit('message', msg);
                    });
                });
                http.listen(settings.port, function () {
                    out("[#] server created".green);
                });
                socket = require('socket.io-client')('http://localhost:' + settings.port);
                connection_status = true;
                listen();
            } else {
                out("[!] Server is already running on this PC".red)
            }
        })
    }
}

function out(msg) {
    process.stdout.clearLine();
    process.stdout.cursorTo(0);
    console.log(msg);
    rl.prompt(true);
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
        out(msg);
    })
}