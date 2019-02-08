// LANCHAT BY AKIRA

//modules require
const client = require('./client')
const prompt = require('./prompt');
var settings = require('./settings')

//init
var http = require('http').Server();
var io = require('socket.io')(http);
var net = require('net');

testPort(settings.port, "127.0.0.1", function (e) {
    if (e === "failure") {
        io.on('connection', function (socket) {
            prompt.answer('user connected' .red);
            socket.on('disconnect', function () {
                prompt.answer('user disconnected' .red);
            });
            socket.on('message', function (msg) {
                socket.broadcast.emit('message', msg);
            });
        });
        http.listen(settings.port, function () {
            prompt.answer("server created" .red);
        });
    } else {
        prompt.answer("connected" .red)
    }
})


client.listen();
prompt.run();

function testPort(port, host, cb) {
    net.createConnection(port, host).on("connect", function (e) {
        cb("success", e);
    }).on("error", function (e) {
        cb("failure", e);
    });
}