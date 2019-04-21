<p align="center">
<img src="https://i.imgur.com/sL3PaoX.png" width="500">
<br>
<img src="https://img.shields.io/github/tag-date/akira202/lanchat.svg?label=last%20version">
<img src="https://img.shields.io/npm/v/lanchat-npm.svg">
<img src="https://img.shields.io/github/downloads/akira202/lanchat/total.svg?label=github%20downloads">
<img src="https://img.shields.io/npm/dt/lanchat-npm.svg?label=npm%20downloads">
<img src ="https://img.shields.io/github/last-commit/akira202/lanchat/dev.svg">
<img src="https://img.shields.io/github/release-date/akira202/lanchat.svg">

</p>

# Lanchat
#### IRC like chat app works in LAN network

* [Instalation](#instalation)
* [Config](#config)
* [Plugins](#plugins)
* [Host](#host)
* [Changelog](#changelog)
* [API documentation](API.md)

## Instalation
You have several options

#### NPM version **(reccomended)**
* download node.js from [here](https://nodejs.org/en/download/)
* `npm install -g lanchat-npm`
* `lanchat` to start

**When `npm update` doesn't work try uninstall and install manualy**

#### Portable Release
* [Download last version](https://github.com/akira202/lanchat/releases)

#### Git Repository
* `git clone https://github.com/akira202/lanchat.git`
* `cd lanchat/src`
* `yarn` or `npm install`
* `node main`

## Config
Configuration files are located in these folders
* Windows `AppData\Roaming\.lanchat`
* Linux `/home/.lanchat`

```js
    "nick": "qwerty123", //user nickname
    "port": "2137",      //connection port
    "notify": "mention", //notify setting
    "log": true,         //log setting
    "attemps": 10,       //max connection attemps
```

## Plugins
Plugins from official repository can be download with `/dwn <plugin name>` command.
Also you can place they in plugins folder manualy.
Windows - `%USERPROFILE%\AppData\Roaming\npm\node_modules\lanchat-npm\plugins`
Linux - `/usr/lib/node_modules/lanchat-npm/plugins`
**Portable version doesn't support plugins**

## Host
Use `/host` command to start lanchat server. Your pseudonym will automatically be assigned administrator privileges.
**I recommend clearing `db.json` file after `1.0.0` update because permission has been changed in the new api.**

### Host configuration and database
You can create a `motd.txt` file for the host to display the motd message.
Users settings and permission is storage in `db.json`
Host settings are storage in `config.json`

```js
    "ratelimit": 15,    //message per second limit
    "socketlimit": 20,  //max user limit
    "lenghtlimit": 1000 //message lenght limit
```

## Changelog

### 1.0.0
* host in portable version
* plugin manager
* new welcome screen
* new colors
* rewritten host
* checks the update at startup
* new API
* loading animation
* code improvements
* bugfixes

### 0.11.0 RC4
* code improvements

### 0.10.1 RC3
* bugfix

### 0.10.0 RC3
* small changes in prompt

### 0.9.0
* plugins support

### 0.8.1 RC2
* code improvements
* bugfixes

### 0.8.0 RC1
* code improvements
* bugfixes

### 0.7.0
* permissions
* ban
* mute
* login and register

### 0.6.1
* bugfix

### 0.6.0
* kick command
* host motd
* save config
* flood block
* code improvements
* bugfixes

### 0.5.0
* mentions
* notifications
* preventing the use of the same nicks
* better api
* code improvements
* bugfixes

### 0.4.0
* auto reconnect
* bugfixes
* code improvements

### 0.3.2
* possibility of downloading from npm
* rainbow text
* bugfixes

### 0.3.1
* bugixes
* code improvements

### 0.3.0
* user list
* status
* blocking of spam with empty messages
* bugfixes
* code improvements

### 0.2.1
* server crashing bugfix

### 0.2.0
* bugfixes
* code improvements
* linux binary

### 0.1.0 beta
* First release