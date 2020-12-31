# Lanchat 2

Encrypted, P2P, local network chat. 

[API documentation](https://github.com/tofudd/lanchat/blob/master/Docs/API.md)

## Get Lanchat
Download self-contained binary from [releases](https://github.com/tofudd/lanchat/releases).

## Lanchat.Terminal
You can start terminal client with the following arguments:

| Argument    | Short | Description                                     |
|-------------|-------|-------------------------------------------------|
| --debug     | -d    | Show logs.                                      |
| --loopback  | -l    | Connect with localhost after start (for debug). |
| --server    | -s    | Start only server without UI.                   |
| --no-server | -n    | Start without server.                           |

## How it works
In P2P mode nodes starts own server and multiple clients. Each node is connected with other by single TCP connection.
It looks something like that.

1. Node A client connects to Node B server.
2. Nodes exchange RSA public keys.
3. Nodes exchange AES keys encrypted with RSA.
4. Nodes exchange previously connected nodes list.
5. Both nodes trying to connect with nodes from list (if they didn't connected already).
6. All done.

TODO: Put some fancy scheme here.