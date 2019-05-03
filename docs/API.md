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
    * [afk](#afk)
    * [alreadySigned](#alreadySigned)
    * [clientMuted](#clientMuted)
    * [dnd](#dnd)
    * [doneBan](#doneBan)
    * [doneSetPermission](#doneSetPermission)
    * [doneUnmute](#doneUnMute)
    * [doneUnban](#doneUnBan)
    * [doneMute](#doneMute)
    * [flood](#flood)
    * [incorrectValue](#incorrectValue)
    * [join](#join)
    * [left](#left)
    * [list](#list)
    * [loginSucces](#loginSucces)
    * [mention](#mention)
    * [message](#message)
    * [motd](#motd)
    * [neetAuth](#neetAuth)
    * [noPermission](#noPermission)
    * [nickChanged](#nickChanged)
    * [nickShortened](#nickShortened)
    * [nickTaken](#nickTaken)
    * [notExist](#notExist)
    * [notSigned](#notSigned)
    * [online](#online)
    * [passChanged](#passChanged)
    * [socketLimit](#socketLimit)
    * [statusChanged](#statusChanged)
    * [tooLong](#tooLong)
    * [userChangeNick](#userChangeNick)
    * [wrongPass](#wrongPass)

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

### afk
Returns nickname of user at status change

### alreadySigned
Returns when user trying auth second time

### clientMuted
Returns when muted user trying send message

### dnd
Returns nickname of user at status change

### doneBan
Returns when moderation action is done succesfully

### doneSetPermission
Returns when moderation action is done succesfully

### doneUnmute
Returns when moderation action is done succesfully

### doneUnban
Returns when moderation action is done succesfully

### doneMute
Returns when moderation action is done succesfully

### flood
Returns when user flooding whith messages or commands

### incorrectValue
Returns when parametr of command isn't valid

### join
Returns someone's nickname when he signed

### left
Retruns someone's nickname when he leave server

### loginSucces
Returns after succesfull login

### mention
Returns someone's nickname when he mentioning you

### message
Returns nickname and message content

### motd
Returns when user joining server with motd

### neetAuth
Returns when user have to login with password

### noPermission
Returns when the user tries to do something for which he is not authorized

### nickChanged
Returns after succesfully nick change

### nickShortened
Returns after nick change when it's too long

### nickTaken
Returns when nick is alredy taken

### notExist
Returns when paramentr user doesn't exist

### online
Returns someone's nickname when he changed status to online

### passChanged
Returns after successfully password change

### socketLimit
Returns when all server sockets is taken.

### statusChanged
Returns after successfully status change

### tooLong
Returns when message is too long

### userChangeNick
Returns old and new nickname when someone changed it

### wrongPass
Returns when password is not valid

## Permissions
* 0 - ban
* 1 - user
* 2 - moderator (kick, ban, etc)
* 3 - administrator (moderator commands + level)
* 4 - owner (can do anything)