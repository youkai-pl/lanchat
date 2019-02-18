//PROMPT
const colors = require('colors')
const client = require('./client')
const commands = require('./commands')
const out = require('./out')
const rl = require("./interface").rl
const setTitle = require('console-title');
var settings = require('./settings')

module.exports = {
    run: function () {
        //init
        initui()

        //prompt
        rl.on('line', function (line) {
            wrapper(line);
        });

        //exit
        rl.on('close', function () {
            client.status({
                content: "left the chat",
                nick: settings.nick
            })
            process.stdout.write('\033c');
            process.exit(0);
        });
    }
}

//welcome message
function initui() {
    process.stdout.write('\033c');
    setTitle('lanchat');
    console.log("LANCHAT 0.3.1".green)
    console.log("")
    console.log("type /help".green)
    console.log("")
    rl.prompt(true);
}

//user input wrapper
function wrapper(message) {
    //if it message
    if (message.charAt(0) !== "/") {
        //send
        client.send(message)
    } else {
        //if it command
        var answer
        const args = message.split(" ");
        if (typeof commands[args[0].substr(1)] !== 'undefined') {
            answer = commands[args[0].substr(1)](args.slice(1));
        }
        //if command have response
        if (typeof answer !== 'undefined') {
            out.blank(answer)
        }
        process.stdout.clearLine();
        process.stdout.cursorTo(0);
        rl.prompt(true);
    }
}