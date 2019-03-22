# Lanchat API 1.0

## Client side socket commands

#### login
Login on server
```js
socket.emit("login", nick)
```

#### auth
Auth registered user
```js
socket.emit("auth", nick, password)
```

#### register
Register user on server
```js
socket.emit("register", nick, password)
```

#### list
Get connected user list
```js
socket.emit("list")
```

#### changeStatus
Change user status
* online
* afk
* dnd
```js
socket.emit("changeStatus", value)
```

#### message
Send message
```js
socket.emit("message", content)
```

#### nick
Change nick
```js
socket.emit("nick", global.nick)
```

#### kick
Kick user by nick
**works only for users with moderator persmission**
```js
socket.emit("kick", nick)
```

#### ban
Ban user by nick
**works only for users with moderator persmission**
```js
socket.emit("ban", nick)
```

#### mute
Mute user by nick
**works only for users with moderator persmission**
```js
socket.emit("kick", mute)
```


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
