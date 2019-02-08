//COMMANDS

module.exports = {
    clear: function () {
        process.stdout.write('\033c');
    },

    exit: function () {
        process.stdout.write('\033c');
        process.exit(0);
    },

    help: function () {
        var help = []
        help[0] = ""
        help[1] = "HELP"
        help[2] = "/clear - clear window"
        help[3] = "/exit - exit Lan Chat"
        help[4] = ""
        return help.join("\n");
    }
}