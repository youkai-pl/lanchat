//COMMANDS
const colors = require('colors');
const client = require('./client');

module.exports = {
    clear: function () {
        process.stdout.write('\033c');
    },

    exit: function () {
        process.stdout.write('\033c');
        process.exit(0);
    },

    nick: function (args) {
        var message
        if (typeof args[0] === "undefined"){
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
        help[2] = ""
        help[3] = "/clear - clear window"
        help[4] = "/exit - exit Lan Chat"
        help[5] = "/nick <nickname> - set nickname"
        help[6] = ""
        return help.join("\n");
    }
}