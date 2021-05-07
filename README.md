# Lanchat 2

Encrypted, P2P, local network chat. 

[Documentation](https://github.com/tofudd/lanchat/wiki)

## Installation

### All platforms

* [Releases](https://github.com/tofudd/lanchat/releases)

### Linux

* Flathub: `flatpak install io.github.tofudd.lanchat.terminal`

## Usage

### Connecting
Use `/connect <IP address>` command to connect with other user in network. You also can use domain name like `/connect pc.lan`. Lanchat also can be used outside LAN network but host you are connecting to must have public IP and forwarded server port.

Lanchat exchange lists of connected nodes address during connecting with new user. If new connected users have not disabled `ConnectToReceivedList` they automatically connect with other nodes.

### Commands
Check list of commands with `/help`. Detailed help for each command can be get by calling `/help <commandName>`.

### Commands with user in parameter
Commands like `/send` or `/block` takes four digits ID in argument. ID of user can be read from nickname after `#`. Like `User#1321`. IDs are assigned randomly upon connection and are different on each node.


### CLI arguments

You can start terminal client with the following arguments:

| Argument    | Short | Description                                |
| ----------- | ----- | ------------------------------------------ |
| --debug     | -d    | Show logs.                                 |
| --no-saved  | -a    | Don't connect to addresses saved in config |
| --no-server | -n    | Start without server.                      |
| --no-udp    | -b    | Start without broadcasting.                |

## Configuration

#### Config file path

* Windows: `%AppData%/Lanchat2/config.js`
* Linux: `~/.Lanchat2/config.json`
* Linux (flatpak): `~/.var/app/io.github.tofudd.lanchat.terminal/config/config.json`
* Mac OS: `~/Library/Preferences/.Lancaht2/config.json`

#### Options

* Nickname
  * `"no_spaces_max_20_characters"`

* Status
  * `"Online"`
  * `"AwayFromKeyboard"`
  * `"DoNotDisturb"`
  
* ServerPort
  * `Number`
  
* BroadcastPort
  * `Number`

* ConnectToReceivedList
  * `true`
  * `false`

* UseIPv6
  * `true`
  * `false`

* ReceivedFilesDirectory
  * `Writable directory path`

* Language
  * `"default"`
  * `"pl"`

#### Example

```json
{
  "Language": "default",
  "BlockedAddresses": [
    "192.168.1.5"
  ],
  "SavedAddresses": [
    "192.168.1.3",
    "192.168.1.4"
  ],
  "Status": "Online",
  "ServerPort": 3645,
  "BroadcastPort": 3646,
  "Nickname": "Reisen",
  "ConnectToReceivedList": true,
  "ReceivedFilesDirectory": "/home/user/Downloads",
  "UseIPv6": false
}
```
