# Lanchat 2

Encrypted, P2P, local network chat. 

## Projects
* Lanchat.Terminal (2.1.0-alpha1) - console app for Windows and Linux
* Lancaht.Xamarin (0.1.0-alpha1) - for Android
* Lancaht.Core (2.1.0-alpha1) - library
* Lanchat.Probe - tool for debugging 

Check [release](https://github.com/tofudd/lanchat/releases) section for binaries.

**Lanchat Terminal requires .NET Core 3.1 or newer**

## How it works
In P2P mode nodes starts own server and multiple clients. Each node is connected with other by single TCP connection.
It looks something like that.

1. Node A client connects to Node B server.
2. Nodes exchange RSA public keys.
3. Nodes exchange AES keys encrypted with RSA.
4. Nodes exchange previously connected nodes list.
5. Both nodes trying to connect with nodes from list (if they didn't connected already).
6. All done.

TODO: Place some fancy scheme here.