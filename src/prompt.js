//PROMPT

const colors = require('colors');
const readline = require('readline');
const rl = readline.createInterface(process.stdin, process.stdout);
const client = require('./client')

module.exports = {
    run: function () {
        //init
        initui()
        //prompt
        rl.on('line', function (line) {
            send(line);
        });

        //exit
        rl.on('close', function () {
            process.stdout.write('\033c');
            process.exit(0);
        });
    },

    answer: function (content) {
        console_out(content);
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

function console_out(msg) {
    process.stdout.clearLine();
    process.stdout.cursorTo(0);
    console.log(msg);
    rl.prompt(true);
}

function send(msg) {
    var out = client.handle(msg)
    if (out !== true) {
        console_out(out)
    } else {
        process.stdout.clearLine();
        process.stdout.cursorTo(0);
        rl.prompt(true);
    }
}