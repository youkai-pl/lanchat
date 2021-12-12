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

| Command    | Description                | Arguments    | Example                     |
| ---------- | -------------------------- | ------------ | --------------------------- |
| broadcast  | Send message to all nodes. | *message*    | `broadcast/message-content` |
| connect    | Connect with node.         | *IP address* | `connect/127.0.0.1`         |
| -          | -                          | *hostname*   | `connect/localhost`         |
| disconnect | Disconnect from node.      | *ID*         | `disconnect/node-id`        |

#### Config

| Command | Description      | Arguments  | Example      |
| ------- | ---------------- | ---------- | ------------ |
| nick    | Change nickname. | *nickname* | `nick/test`  |
| status  | Change status.   | *status*   | `status/afk` |

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

#### Error (enum)

| Value              |
|--------------------|
| `invalid_command`  |
| `invalid_argument` |
| `missing_argument` |
| `node_not_found`   |
| `cannot_connect`   |

#### Status (enum)

| Value    |
|----------|
| `online` |
| `afk`    |
| `dnd`    |


#### String types

* message
* hostname
* IP address