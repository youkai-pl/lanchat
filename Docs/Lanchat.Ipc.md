# Lanchat.Ipc

## Intro
Lanchat.Ipc is a Core library based client. It is not intended for the end user.
Instead of a user interface, it creates a Unix socket to receive commands and deliver output.
It can be used to develop non-.NET applications that use Lanchat.Core. For example, bash scripts.

## Usage
Commands arguments are separated by `/`.

## Commands

### General

| Command    | Description                | Syntax       | Example                                           |
|------------|----------------------------|--------------|---------------------------------------------------|
| broadcast  | Send message to all nodes. | *string*     | `broadcast/hello`                                 |
| connect    | Connect with node.         | *IP address* | `connect/127.0.0.1`                               |
| -          | -                          | *hostname*   | `connect/localhost`                               |
| disconnect | Disconnect from node.      | *GUID*       | `disconnect/00000000-0000-0000-0000-000000000000` |
| nick       | Change nickname.           | *string*     | `nick/test`                                       |