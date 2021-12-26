# PLEASE UPDATE TO 2.9.3
**Previous versions has security bug. With a modified client is possible to send a file that will be saved outside of downloads directory.**
**For example:**
1. User A with modified client sent a file with filename set to `../test`.
2. User B sees it as `test` and accepts file that should go into `~/Downloads/test`.
3. File is saved into `~/test` instead.

**Existing files cannot be overwritten this way. If file with specified name already exists new file with name like test(1), test(2) etc will be created.**

# Lanchat

## Introduction

Lanchat is app and library for communication in P2P manner.

Initially designed for use in LAN networks also can works over internet (if you have public address and port forwarded).
Main purpose of Lanchat is messaging but Lanchat.Core library is extendable (see [Lanpaint](https://github.com/tof4/Lanpaint)).

[Documentation](https://youkai.pl/lanchat/)

## Features

* P2P communication
* Messaging
* File transfer
* Encryption
* Automatic conenction
* Terminal UI
* Works on Windows, Mac and Linux

## Clients

* [Lanchat.Terminal](Docs/Clients/Terminal.md)
* [Lanchat.Ipc](Docs/Clients/Ipc.md)

## Apps using Lanchat.Core

* [Lanpaint](https://github.com/tof4/Lanpaint)

## Security

Lanchat uses combination of RSA public key and AES encryption.
Lanchat saves public key of connected users and compare them in next connections. Keys are assigned to IP address.
Thanks to that the possibility of a man in the middle attack is somewhat limited.

**On first connection you should compare fingerprints by yourself**

If public key was changed Lanchat will give error message on each connection. If you are sure the keys are correct (for
example ip address is used by more than one user)
you should remove corresponding PEM file in Lanchat config directory.

**Messages and transferred files are encrypted. Other data, like nicknames or status changes, is sent in plain JSON format.**