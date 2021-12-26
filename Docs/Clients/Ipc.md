# Lanchat.Ipc

## Intro
Lanchat.Ipc is a Core library based client. It is not intended for the end user.
Instead of a user interface, it creates a Unix socket to receive commands and deliver output.
It can be used to develop non-.NET applications that use Lanchat.Core. For example, bash scripts.

## Usage

`./Lanchat.Ipc [socket path] [options]`

Options:
* `-d` - debug mode
* `-a` - don't connect with saved nodes
* `-n` - disable server
* `-b` - disable broadcasting

## API

Commands arguments are separated by `/`.
Data is encoded in `UTF-8`.

### Commands

#### General

| Command    | Description                                          | Arguments       | Example                     |
| ---------- | ---------------------------------------------------- | --------------- | --------------------------- |
| broadcast  | Send message to all nodes.                           | *message*       | `broadcast/message-content` |
| private    | Send private message.                                | *ID*; *message* | `private/1/message-content` |
| connect    | Connect with node.                                   | *IP address*    | `connect/127.0.0.1`         |
| -          | -                                                    | *hostname*      | `connect/localhost`         |
| disconnect | Disconnect from node.                                | *ID*            | `disconnect/1`              |
| list       | Get list of connected nodes. Returns *nodes-list*.   |                 | `list`                      |
| whois      | Info about conencted node. Returns *whois*.          | *ID*            | `info/1`                    |

#### Config

| Command | Description      | Arguments  | Example      |
| ------- | ---------------- | ---------- | ------------ |
| nick    | Change nickname. | *nickname* | `nick/test`  |
| status  | Change status.   | *status*   | `status/afk` |

#### Blocking

| Command  | Description                                      | Arguments    | Example           |
| -------- | -------------------------------------------------| -----------  | ----------------  |
| block/id | Block node by short ID.                          | *ID*         | `block/1`         |
| block/ip | Block node by IP address.                        | *IP address* | `block/127.0.0.1` |                          
| unblock  | Unblock node.                                    | *ID*         | `unblock/1`       |
| blocked  | Get list of blocked nodes. Returns *nodes-list*. |              | `blocked`         |

### Events

#### General

| Event | Description             | Arguments | Example                 |
| ----- | ----------------------- | --------- | ----------------------- |
| error | Command returned error. | *error*   | `error/invalid_command` |


#### Node

| Event           | Description                 | Arguments        | Example                                   |
| --------------- | --------------------------- | ---------------- | ----------------------------------------- |
| connected       | Node connected.             | *ID*             | `node-id/connected`                       |
| disconnected    | Node disconnected.          | *ID*             | `node-id/disconnected`                    |
| message         | Broadcast message received. | *ID*; *message*  | `node-id/message/message-content`         |
| private_message | Private message received.   | *ID*; *message*  | `node-id/private_message/message-content` |
| nickname_update | Node nickname changed.      | *ID*; *nickname* | `node-id/nickname_update/new-nickname`    |
| status_update   | Node status changed.        | *ID*; *status*   | `node-id/status_update/new-status`        |

### Types

#### Simple types
* *ID* - Node short id
* *message* - message string
* *hostname* - domain name
* *IP address* - node IP address
  
#### error (enum)

| Value              |
|--------------------|
| `invalid_command`  |
| `invalid_argument` |
| `missing_argument` |
| `node_not_found`   |
| `cannot_connect`   |

#### status (enum)

| Value    |
|----------|
| `online` |
| `afk`    |
| `dnd`    |

#### ndoes-list (string)

List of connected nodes short IDs separated by `,`:

`1,2,3;`

#### whois-info (string)

Whois command result in single string:

`nickname,short-id,status,ip-address,port,host-id,is-session,fingerprint;`