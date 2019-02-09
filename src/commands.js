//COMMANDS
const colors = require('colors');
const client = require('./client');
var settings = require('./settings')

module.exports = {
    clear: function () {
        process.stdout.write('\033c');
    },

    exit: function () {
        process.stdout.write('\033c');
        process.exit(0);
    },

    host: function () {
        client.host()
    },

    nick: function (args) {
        var message
        if (typeof args[0] === "undefined") {
            message = "Try: /nick <new_nick>"
        } else {
            client.set_nick(args[0])
            message = "Your nickname is now " + args[0]
        }
        return message;
    },

    help: function () {
        var help = []
        help[0] = ""
        help[1] = "HELP".green
        help[2] = "/host - create server"
        help[3] = "/connect <ip> - connect to server"
        help[4] = "/disconnect - disconnect from server"
        help[5] = "/clear - clear window"
        help[6] = "/exit - exit Lan Chat"
        help[7] = "/nick <nickname> - set nickname"
        help[8] = ""
        return help.join("\n");
    },

    connect: function (args) {
        client.connect(args[0]);
    },

    disconnect: function () {
        client.disconnect();
    }
}