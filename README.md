<p align="center">
<img src="https://www.tofu.ovh/files/lanchat2_logo.png" width="500">
</p>

## Get started

Requirements:

* For Windows: `.NET 4.7.2` (If your Windows is updated you probably already have it)
* For Linux or MacOS `.NET Core 3.0`

Get last [release](https://github.com/tofudd/lanchat/releases) and uznip files. No need to install.

## Directories guide
* Lanchat.Cli - Terminal Lanchat client app
* Lanchat.Common - Library with classes common for other projects
* Lanchat.Core - .Net Core project for Lanchat Cli
* Lanchat.Windows - .Net Framework project for Lanchat Cli

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