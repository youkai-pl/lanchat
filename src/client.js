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
        if (msg.charAt(0) === "/") {
            return "komenda";
        } else {
            return true;
        }
    }
}