<p align="center">
<img src="https://www.tofu.ovh/files/lanchat2_logo.png" width="500">
</p>

## Beta Release yeeeee
### What's inside?
* Major features of *Lanchat.Common* is ready.
* And all works fine on Debian VM's and Debian/Windows connections.
* *Lanchat.Cli* client app is still have some bugs but can be use.

### So why is this in master branch?
* Lanchat *1.x* is no longer supported.
* [Archived branch](https://github.com/tofudd/lanchat/tree/1.x).

### What abut the client
* I know it's still shitty.
* I'll work on WPF app right now.

## Get started

Requirements:

* For Windows: `.NET 4.7.2` (If your Windows is updated you probably already have it)
* For Linux or macOS `.NET Core 3.0`

For Windows:

* Get last [release](https://github.com/tofudd/lanchat/releases) and uznip files. No need to install.

For Linux & MacOS:

* I currently don't provide build for other systems, but you can compile it yourself.

## Docs
* [Lanchat.Common](https://github.com/tofudd/lanchat/blob/master/docs/Lanchat.Common.md)

## Directories guide
* `Lanchat.Cli` - Lanchat client for terminal
* `Lanchat.Common` - Library with classes common for other projects

## Versioning
>MAJOR.MINOR.PATCH.BUILD

Build:
* 1xxx - Alpha
* 2xxx - Beta
* 3xxx - Release

## Planned changes
* change language to C#
* moderation will be replaced with client-side features
* working without central host in lan mode
* rooms
* encrytpion

## Todo
### Client:
- [x] Basic prompt
- [x] Linux support
- [ ] Separated prompts for rooms
- [ ] Themes support
- [ ] Multi languages
- [ ] ~~Plugins~~ I think it' s unnecessary for now

### Protocol:
- [x] Working encrypted chat
- [ ] Rooms
- [ ] Sending files