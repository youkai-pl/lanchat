//PROMPT
const colors = require('colors');
const readline = require('readline');
const rl = readline.createInterface(process.stdin, process.stdout);
const client = require('./client')
const commands = require('./commands')
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
            process.stdout.write('\033c');
            process.exit(0);
        });
    }
}

function initui() {
    process.stdout.write('\033c');
    console.log("LANCHAT v0.1".green)
    console.log("")
    console.log("type /help".green)
    console.log("")
    rl.prompt(true);
}

//write to console
function console_out(msg) {
    process.stdout.clearLine();
    process.stdout.cursorTo(0);
    console.log(msg);
    rl.prompt(true);
}

//user input wrapper
function wrapper(msg) {
    //if it message
    if (msg.charAt(0) !== "/") {
        //get time
        var date = new Date();
        var time = (("0" + date.getHours()).slice(-2) + ":" +
            ("0" + date.getMinutes()).slice(-2) + ":" +
            ("0" + date.getSeconds()).slice(-2));

        //send
        var message = "[" + time.green + "] "  + settings.nick.blue + ": " + msg
        console_out(message)
        client.handle(message)
    } else {
        //if it command
        var answer
        const args = msg.split(" ");
        if (typeof commands[args[0].substr(1)] !== 'undefined') {
            answer = commands[args[0].substr(1)](args.slice(1));
        }
        //if command have response
        if (typeof answer !== 'undefined') {
            console_out(answer)
        }
        process.stdout.clearLine();
        process.stdout.cursorTo(0);
        rl.prompt(true);
    }
}