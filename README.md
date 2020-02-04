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

* For Windows: `.NET 4.7.2`
* For Linux or macOS: `.NET Core 3.0`

[Download here](https://github.com/tofudd/lanchat/releases)

## Contribute

### Docs
* [Lanchat.Common](https://github.com/tofudd/lanchat/blob/master/docs/Lanchat.Common.md)

### Directories guide
* `Lanchat.Console` - Terminal Lanchat client
* `Lanchat.Common` - Common library

### Versioning
>MAJOR.MINOR.PATCH.BUILD

Build:
* 1xxx - Alpha
* 2xxx - Beta
* 3xxx - Release

### Todo
#### Common library:
- [x] Working encrypted chat
- [ ] Rooms
- [ ] Sending files

#### Console client:
- [x] Basic prompt
- [x] Linux support
- [ ] Separated prompts for rooms

#### Desktop client:
- [ ] All possibilities of the console version
- [ ] Themes