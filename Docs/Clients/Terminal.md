# Lanchat.Terminal

Main client of Lanchat based on Lanchat.Core. Available for Windows, Linux and MacOS.

## Installation

### Windows, Linux and MacOS

Get zip file for your OS from [Github releases](https://github.com/tof4/lanchat/releases/latest/) and extract it
wherever you want.

### Flatpak

```sh
flatpak install flathub io.github.tofudd.lanchat.terminal
flatpak run io.github.tofudd.lanchat.terminal
```

### Source code

For running Lanchat.Terminal from source you will need [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

```sh
git clone https://github.com/tof4/lanchat
cd lanchat/Lanchat.Terminal
dotnet publish -c Release -p:PublishSingleFile=true --self-contained -r <runtime id>
cd bin/Release/net5.0/linux-x64/publish/<runtime id>
./Lanchat
```

Runtime ID for your operating system can be found [here](https://docs.microsoft.com/pl-pl/dotnet/core/rid-catalog).

## Usage

### Keys

* `Tab` - Switch to next tab
* `Shift + Tab` - Switch to previous tab
* `Up Arrow` - Previous item in input history
* `Down Arrow` - Next item in input history
* `PageUp` - Scroll content up
* `PageDown` - Scroll content down

### Connecting

Use `/connect <IP address>` command to connect with other user in network. You can use IP or domain name
like `/connect pc.lan`. Lanchat also can be used outside LAN network but host you are connecting to must have public IP
and forwarded server port.

Lanchat uses UDP broadcast to detect and connect with other nodes. If for some reason this doesn't work, there's a second way. After establishing connection, nodes exchange the list of nodes addresses with which they connected earlier. 

### Commands

Check list of commands with `/help`. Detailed help for each command can be get by calling `/help <commandName>`.

### Commands with user in parameter

Commands like `/send` or `/block` takes four digits ID in argument. ID of user can be read from nickname after `#`.
Like `User#1321`. IDs are assigned randomly upon connection and are different on each node. Commands that takes user ID
in argument can be executed in user tab without it.

### CLI arguments

You can start terminal client with the following arguments:

| Argument    | Short | Description                                |
| ----------- | ----- | ------------------------------------------ |
| --debug     | -d    | Show debug tab.                            |
| --no-saved  | -a    | Don't connect to addresses saved in config |
| --localhost | -l    | Connect to localhost.                      |
| --no-server | -n    | Start without server.                      |
| --no-udp    | -b    | Start without broadcasting.                |

### Configuration

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
    * `"pt"`

#### Example

```json
{
  "Language": "default",
  "Status": "Online",
  "ServerPort": 3645,
  "BroadcastPort": 3646,
  "Nickname": "Reisen",
  "ConnectToReceivedList": true,
  "ReceivedFilesDirectory": "/home/user/Downloads",
  "UseIPv6": false
}
```