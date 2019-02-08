//CLIENT

module.exports = {
    connect: function () {
        process.stdout.write('\033c');
        console.log("LANCHAT v0.1".green)
        console.log("")
        console.log("loading...")
        setTimeout(function () { return true }, 1000);
    },

    handle: function (msg) {

    },

    command: function (command) {
        if (command === "/clear") {
            return
        }
    }
}