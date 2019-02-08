//CLIENT
var settings = require('./settings')

module.exports = {
    handle: function (msg) {

    },

    set_nick: function (nick) {
        settings.nick = nick;
    },

    command: function (command) {
        if (command === "/clear") {
            return
        }
    }
}