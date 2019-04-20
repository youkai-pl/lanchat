# Lanchat API 1.0

* [Client side socket commands](#client-side-socket-commands)
    * [auth](#auth)
    * [ban](#ban)
    * [changeNick](#changeNick)
    * [changeStatus](#changeStatus)
    * [kick](#kick)
    * [mention](#mention)
    * [message](#message)
    * [mute](#mute)
    * [list](#list)
    * [login](#login)
    * [setPassword](#setPassword)
    * [setPermission](#setPermission)
    * [unban](#unban)
    * [unmute](#unmute)

* [Client side socket events](#client-side-socket-events)

* [Permissions](#permissions)
## Client side socket commands

### auth
Auth registered user
```js
socket.emit("auth", nick, password)
```

### ban
Ban user
```js
socket.emit("ban", nick)
```

### changeNick
Change nick
```js
socket.emit("changeNick", nick)
```

### changeStatus
Change user status
* online
* afk
* dnd
```js
socket.emit("changeStatus", value)
```

### kick
Kick user
```js
socket.emit("kick", nick)
```

### mention
Send mention
```js
socket.emit("mention", nick)
```

### message
Send message
```js
socket.emit("message", content)
```

### mute
Mute user
```js
socket.emit("kick", mute)
```

### list
Get connected user list
```js
socket.emit("list")
```

### login
Login on server
```js
socket.emit("login", nick)
```

### setPassword
Register user on server or change his password
```js
socket.emit("register", setPassword)
```

### setPermission
Change user permission number
[Permissions](#permissions)

```js
socket.emit("setPermission", nick, level)
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

### banned
Emited when user has been banned

### needAuth

### joined

## Permissions
* 0 - ban
* 1 - user
* 2 - moderator (kick, ban, etc)
* 3 - administrator (moderator commands + level)
* 4 - owner (can do anything)