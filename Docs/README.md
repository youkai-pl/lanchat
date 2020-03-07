<p align="center">
<img src="https://www.tofu.ovh/files/lanchat2_logo.png" width="500">
</p>


## Lanchat 1.x note
* Lanchat *1.x* is no longer supported.
* [Archived branch](https://github.com/tofudd/lanchat/tree/1.x).

## Get started

Requirements:

* For Windows: `.NET 4.7.2`
* For Linux or macOS: `.NET Core 3.0`

[Get Lanchat here](https://github.com/tofudd/lanchat/releases)

### How to run Lanchat on linux

First install .NET Core runtime. For Debian you can use commands bellow.
[Microsoft refference](https://docs.microsoft.com/en-us/dotnet/core/install/)

```
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
wget -q https://packages.microsoft.com/config/debian/10/prod.list
sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list

sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-runtime-3.1
```

If .NET Core runtime is installed get newest Lanachat release for linux. Unzip folder and start with `./Lanchat` command.

## Contribute

### Docs
* [Lanchat.Common](https://github.com/tofudd/lanchat/blob/master/Docs/Lanchat.Common.md)
* [Guide](https://github.com/tofudd/lanchat/blob/master/Docs/Guide.md)
* [API](https://github.com/tofudd/lanchat/blob/master/Docs/API.md)

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