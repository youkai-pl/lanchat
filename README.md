# Lan-Chat
#### IRC like chat app works in LAN network

<div style="text-align: center;">

![](https://img.shields.io/github/repo-size/akira202/lanchat.svg) ![](https://img.shields.io/github/languages/top/akira202/lanchat.svg) ![](https://img.shields.io/github/license/akira202/lanchat.svg) ![](https://img.shields.io/npm/v/lanchat-npm.svg) ![](https://img.shields.io/github/last-commit/akira202/lanchat.svg) ![](https://img.shields.io/github/release-date/akira202/lanchat.svg)

</div>

<p style="text-align: center;">
    <img src="https://i.ibb.co/8DmL1FM/test.gif" width="500">
</p>


## Instalation
You have three options

#### NPM version **(reccomended)**
* download node.js from [here](https://nodejs.org/en/download/)
* `npm install -g lanchat-npm`
* `lanchat` to start

#### Portable Release
* [Download last version](https://github.com/akira202/lanchat/releases)

#### Git Repository
* `git clone https://github.com/akira202/lanchat.git`
* `cd lanchat/src`
* `yarn` or `npm install`
* `node main`

## Commands
* `/host` - create server
* `/connect` <ip> - connect to server
* `/disconnect` - disconnect from server
* `/clear` - clear window
* `/exit` - exit Lan Chat
* `/nick` <nickname> - set nickname
* `/list` - connected users list
* `/afk` - change status to afk
* `/online` - change status to online
* `/rb` - rainbow text

## Config
Configuration files are located in these folders
* Windows `AppData/Roaming/lanchat`
* Linux `/.local/share`
* MacOS `Library/Preferences`

### Host configuration
Host settings can be changed in `host.json` file.
You can create a `motd.txt` file for the host to display the motd message.

## Changelog

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