//PROMPT
const colors = require('colors');
const readline = require('readline');
const rl = readline.createInterface(process.stdin, process.stdout);
const client = require('./client')
const commands = require('./commands')

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
        console_out(msg)
        client.handle(msg)
    } else {
        //if it command
        var answer
        if (typeof commands[msg.substr(1)] !== 'undefined') {
            answer = commands[msg.substr(1)]();
        }
        //if command have response send it
        if (typeof answer !== 'undefined') {
            console_out(answer)
        }
        process.stdout.clearLine();
        process.stdout.cursorTo(0);
        rl.prompt(true);
    }
}