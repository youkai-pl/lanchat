//CLIENT
var settings = require("./settings")
var socket = require('socket.io-client')('http://localhost:' + settings.port);

module.exports = {
    send: function (msg) {
        socket.emit('message', msg);
    },

    set_nick: function (nick) {
        settings.nick = nick;
    },

    listen: function () {
        out("connected")
        socket.on('message', function (msg) {
            out(msg);
        });
    }
}

function out(msg) {
    process.stdout.clearLine();
    process.stdout.cursorTo(0);
    console.log(msg);
}