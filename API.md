# Lanchat API 1.0

* [Client side socket commands](#client-side-socket-commands)
  * [login](#login)
  * [auth](#auth)
  * [register](#register)
  * [list](#list)
  * [changeStatus](#changeStatus)
  * [message](#message)
  * [mention](#mention)
  * [nick](#nick)
  * [ban](#ban)
  * [mute](#mute)
  * [unban](#unban)
  * [unmute](#unmute)
  * [level](#level)
* [Client side socket events](#client-side-socket-events)
  * [message](#message-event)
  * [mentioned](#mentioned)
  * [status](#status)
  * [return](#return)
  * [motd](#motd)
  * [rcode](#rcode)
* [Permissions](#permissions)
* [Return codes](#return-codes)
## Client side socket commands

### login
Login on server
```js
socket.emit("login", nick)
```

### auth
Auth registered user
```js
socket.emit("auth", nick, password)
```

### register
Register user on server
```js
socket.emit("register", nick, password)
```

### list
Get connected user list
```js
socket.emit("list")
```

### changeStatus
Change user status
* online
* afk
* dnd
```js
socket.emit("changeStatus", value)
```

### message
Send message
```js
socket.emit("message", content)
```

### mention
Send mention
```js
socket.emit("mention", nick)
```

### nick
Change nick
```js
socket.emit("nick", nick)
```

### kick
Kick user
```js
socket.emit("kick", nick)
```

### ban
Ban user
```js
socket.emit("ban", nick)
```

### mute
Mute user
```js
socket.emit("kick", mute)
```

### unban
Unban user
```js
socket.emit("unban", nick)
```

### unmute
Unmute user
```js
socket.emit("kick", unmute)
```

### level
Change user permission number
[Permissions](#permissions)
```js
socket.emit("level", nick)
```

## Client side socket events

### message (event)
Message broadcasted on host <br>
Return obcject:
```js
msg {
    nick: "name of the sender"
    content: "message content"
}
```

### mentioned
Mention from other user
Return nickname of sender

### status
Change status of other user <br>
Return object:
```js
msg {
    nick: "name of user"
    content: "content"
}
```

### return
Message from server <br>
Return string

### motd
Text showing after connected <br>
Return string

### rcode
Return code from server <br>
[List](#return-codes)

## Permissions
* 0 - ban
* 1 - mute
* 2 - user
* 3 - moderator (kick, ban, etc)
* 4 - administrator (moderator commands + level)
* 5 - owner (can do anything)

## Return codes
* 001 - blank message
* 002 - selected user not exist
* 003 - already used nickname
* 004 - flood blocked
* 005 - user banned
* 006 - account locked with password
* 007 - already logged
* 008 - wrong password
* 009 - password changed
* 010 - blank password at register
* 011 - user muted
* 012 - long message blocked
* 013 - user don't have permission
* 014 - bad permission ID
* 015 - logged successfully
