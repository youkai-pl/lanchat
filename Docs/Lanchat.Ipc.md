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

Commands arguments are separated by `/`. Use UTF-8 encoding.

## Commands

### General

| Command    | Description                | Arguments    | Example                     |
|------------|----------------------------|--------------|-----------------------------|
| broadcast  | Send message to all nodes. | *message*    | `broadcast/message-content` |
| connect    | Connect with node.         | *IP address* | `connect/127.0.0.1`         |
| -          | -                          | *hostname*   | `connect/localhost`         |
| disconnect | Disconnect from node.      | *GUID*       | `disconnect/node-id`        |
| nick       | Change nickname.           | *nickname*   | `nick/test`                 |

## Output

### Node events
| Event           | Description                 | Arguments          | Example                                   |
|-----------------|-----------------------------|--------------------|-------------------------------------------|
| connected       | Node connected.             | *GUID*             | `node-id/connected`                       |
| disconnected    | Node disconnected.          | *GUID*             | `node-id/disconnected`                    |
| message         | Broadcast message received. | *GUID*; *message*  | `node-id/message/message-content`         |
| private_message | Private message received.   | *GUID*; *message*  | `node-id/private_message/message-content` |
| nickname_update | Node nickname changed.      | *GUID*; *nickname* | `node-id/nickname_update/new-nickname`    |
| status_update   | Node status changed.        | *GUID*; *status*   | `node-id/status_update/new-status`        |