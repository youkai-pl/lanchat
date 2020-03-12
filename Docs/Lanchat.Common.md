<a name='assembly'></a>
# Lanchat.Common

## Contents

- [ChangedNicknameEventArgs](#T-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs 'Lanchat.Common.NetworkLib.Events.Args.ChangedNicknameEventArgs')
  - [NewNickname](#P-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs-NewNickname 'Lanchat.Common.NetworkLib.Events.Args.ChangedNicknameEventArgs.NewNickname')
  - [OldNickname](#P-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs-OldNickname 'Lanchat.Common.NetworkLib.Events.Args.ChangedNicknameEventArgs.OldNickname')
- [ConnectionFailedException](#T-Lanchat-Common-NetworkLib-Exceptions-ConnectionFailedException 'Lanchat.Common.NetworkLib.Exceptions.ConnectionFailedException')
- [Events](#T-Lanchat-Common-NetworkLib-Api-Events 'Lanchat.Common.NetworkLib.Api.Events')
  - [OnChangedNickname(oldNickname,newNickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnChangedNickname-System-String,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnChangedNickname(System.String,System.String)')
  - [OnHostStarted(port)](#M-Lanchat-Common-NetworkLib-Api-Events-OnHostStarted-System-Int32- 'Lanchat.Common.NetworkLib.Api.Events.OnHostStarted(System.Int32)')
  - [OnNodeConnected(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeConnected-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeConnected(System.Net.IPAddress,System.String)')
  - [OnNodeDisconnected(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeDisconnected-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeDisconnected(System.Net.IPAddress,System.String)')
  - [OnNodeResumed(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeResumed-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeResumed(System.Net.IPAddress,System.String)')
  - [OnNodeSuspended(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeSuspended-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeSuspended(System.Net.IPAddress,System.String)')
  - [OnReceivedMessage(content,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnReceivedMessage-System-String,System-String,System-Boolean- 'Lanchat.Common.NetworkLib.Api.Events.OnReceivedMessage(System.String,System.String,System.Boolean)')
- [Handshake](#T-Lanchat-Common-Types-Handshake 'Lanchat.Common.Types.Handshake')
  - [#ctor(nickname,publicKey,port)](#M-Lanchat-Common-Types-Handshake-#ctor-System-String,System-String,System-Int32- 'Lanchat.Common.Types.Handshake.#ctor(System.String,System.String,System.Int32)')
  - [Nickname](#P-Lanchat-Common-Types-Handshake-Nickname 'Lanchat.Common.Types.Handshake.Nickname')
  - [Port](#P-Lanchat-Common-Types-Handshake-Port 'Lanchat.Common.Types.Handshake.Port')
  - [PublicKey](#P-Lanchat-Common-Types-Handshake-PublicKey 'Lanchat.Common.Types.Handshake.PublicKey')
- [HostStartedEventArgs](#T-Lanchat-Common-NetworkLib-Events-Args-HostStartedEventArgs 'Lanchat.Common.NetworkLib.Events.Args.HostStartedEventArgs')
  - [Port](#P-Lanchat-Common-NetworkLib-Events-Args-HostStartedEventArgs-Port 'Lanchat.Common.NetworkLib.Events.Args.HostStartedEventArgs.Port')
- [Methods](#T-Lanchat-Common-NetworkLib-Api-Methods 'Lanchat.Common.NetworkLib.Api.Methods')
  - [Connect(ip,port)](#M-Lanchat-Common-NetworkLib-Api-Methods-Connect-System-Net-IPAddress,System-Int32- 'Lanchat.Common.NetworkLib.Api.Methods.Connect(System.Net.IPAddress,System.Int32)')
  - [GetNode(ip)](#M-Lanchat-Common-NetworkLib-Api-Methods-GetNode-System-Net-IPAddress- 'Lanchat.Common.NetworkLib.Api.Methods.GetNode(System.Net.IPAddress)')
  - [GetNode(nickname)](#M-Lanchat-Common-NetworkLib-Api-Methods-GetNode-System-String- 'Lanchat.Common.NetworkLib.Api.Methods.GetNode(System.String)')
  - [SendAll(message)](#M-Lanchat-Common-NetworkLib-Api-Methods-SendAll-System-String- 'Lanchat.Common.NetworkLib.Api.Methods.SendAll(System.String)')
- [Network](#T-Lanchat-Common-NetworkLib-Network 'Lanchat.Common.NetworkLib.Network')
  - [#ctor(broadcastPort,nickname,hostPort,heartbeatTimeout)](#M-Lanchat-Common-NetworkLib-Network-#ctor-System-Int32,System-String,System-Int32,System-Int32- 'Lanchat.Common.NetworkLib.Network.#ctor(System.Int32,System.String,System.Int32,System.Int32)')
  - [Events](#P-Lanchat-Common-NetworkLib-Network-Events 'Lanchat.Common.NetworkLib.Network.Events')
  - [Methods](#P-Lanchat-Common-NetworkLib-Network-Methods 'Lanchat.Common.NetworkLib.Network.Methods')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Network-Nickname 'Lanchat.Common.NetworkLib.Network.Nickname')
  - [NodeList](#P-Lanchat-Common-NetworkLib-Network-NodeList 'Lanchat.Common.NetworkLib.Network.NodeList')
  - [Dispose()](#M-Lanchat-Common-NetworkLib-Network-Dispose 'Lanchat.Common.NetworkLib.Network.Dispose')
  - [Dispose(disposing)](#M-Lanchat-Common-NetworkLib-Network-Dispose-System-Boolean- 'Lanchat.Common.NetworkLib.Network.Dispose(System.Boolean)')
  - [Finalize()](#M-Lanchat-Common-NetworkLib-Network-Finalize 'Lanchat.Common.NetworkLib.Network.Finalize')
  - [Start()](#M-Lanchat-Common-NetworkLib-Network-Start 'Lanchat.Common.NetworkLib.Network.Start')
- [Node](#T-Lanchat-Common-NetworkLib-Node 'Lanchat.Common.NetworkLib.Node')
  - [#ctor(ip,network)](#M-Lanchat-Common-NetworkLib-Node-#ctor-System-Net-IPAddress,Lanchat-Common-NetworkLib-Network- 'Lanchat.Common.NetworkLib.Node.#ctor(System.Net.IPAddress,Lanchat.Common.NetworkLib.Network)')
  - [ClearNickname](#P-Lanchat-Common-NetworkLib-Node-ClearNickname 'Lanchat.Common.NetworkLib.Node.ClearNickname')
  - [Handshake](#P-Lanchat-Common-NetworkLib-Node-Handshake 'Lanchat.Common.NetworkLib.Node.Handshake')
  - [Heartbeat](#P-Lanchat-Common-NetworkLib-Node-Heartbeat 'Lanchat.Common.NetworkLib.Node.Heartbeat')
  - [Ip](#P-Lanchat-Common-NetworkLib-Node-Ip 'Lanchat.Common.NetworkLib.Node.Ip')
  - [Mute](#P-Lanchat-Common-NetworkLib-Node-Mute 'Lanchat.Common.NetworkLib.Node.Mute')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Node-Nickname 'Lanchat.Common.NetworkLib.Node.Nickname')
  - [Port](#P-Lanchat-Common-NetworkLib-Node-Port 'Lanchat.Common.NetworkLib.Node.Port')
  - [State](#P-Lanchat-Common-NetworkLib-Node-State 'Lanchat.Common.NetworkLib.Node.State')
  - [Dispose()](#M-Lanchat-Common-NetworkLib-Node-Dispose 'Lanchat.Common.NetworkLib.Node.Dispose')
  - [Dispose(disposing)](#M-Lanchat-Common-NetworkLib-Node-Dispose-System-Boolean- 'Lanchat.Common.NetworkLib.Node.Dispose(System.Boolean)')
  - [Finalize()](#M-Lanchat-Common-NetworkLib-Node-Finalize 'Lanchat.Common.NetworkLib.Node.Finalize')
  - [SendPrivate(message)](#M-Lanchat-Common-NetworkLib-Node-SendPrivate-System-String- 'Lanchat.Common.NetworkLib.Node.SendPrivate(System.String)')
- [NodeAlreadyExistException](#T-Lanchat-Common-NetworkLib-Exceptions-NodeAlreadyExistException 'Lanchat.Common.NetworkLib.Exceptions.NodeAlreadyExistException')
- [NodeConnectionStatusEventArgs](#T-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs 'Lanchat.Common.NetworkLib.Events.Args.NodeConnectionStatusEventArgs')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs-Nickname 'Lanchat.Common.NetworkLib.Events.Args.NodeConnectionStatusEventArgs.Nickname')
  - [NodeIP](#P-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs-NodeIP 'Lanchat.Common.NetworkLib.Events.Args.NodeConnectionStatusEventArgs.NodeIP')
- [ReceivedMessageEventArgs](#T-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs 'Lanchat.Common.NetworkLib.Events.Args.ReceivedMessageEventArgs')
  - [Content](#P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Content 'Lanchat.Common.NetworkLib.Events.Args.ReceivedMessageEventArgs.Content')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Nickname 'Lanchat.Common.NetworkLib.Events.Args.ReceivedMessageEventArgs.Nickname')
  - [Private](#P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Private 'Lanchat.Common.NetworkLib.Events.Args.ReceivedMessageEventArgs.Private')
- [Status](#T-Lanchat-Common-Types-Status 'Lanchat.Common.Types.Status')
  - [Ready](#F-Lanchat-Common-Types-Status-Ready 'Lanchat.Common.Types.Status.Ready')
  - [Resumed](#F-Lanchat-Common-Types-Status-Resumed 'Lanchat.Common.Types.Status.Resumed')
  - [Suspended](#F-Lanchat-Common-Types-Status-Suspended 'Lanchat.Common.Types.Status.Suspended')
  - [Waiting](#F-Lanchat-Common-Types-Status-Waiting 'Lanchat.Common.Types.Status.Waiting')

<a name='T-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs'></a>
## ChangedNicknameEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib.Events.Args

##### Summary

Changed node nickname event.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs-NewNickname'></a>
### NewNickname `property`

##### Summary

New nickname.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-ChangedNicknameEventArgs-OldNickname'></a>
### OldNickname `property`

##### Summary

Old nickname.

<a name='T-Lanchat-Common-NetworkLib-Exceptions-ConnectionFailedException'></a>
## ConnectionFailedException `type`

##### Namespace

Lanchat.Common.NetworkLib.Exceptions

##### Summary

Cannot establish connection.

<a name='T-Lanchat-Common-NetworkLib-Api-Events'></a>
## Events `type`

##### Namespace

Lanchat.Common.NetworkLib.Api

##### Summary

Network API inputs class.

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnChangedNickname-System-String,System-String-'></a>
### OnChangedNickname(oldNickname,newNickname) `method`

##### Summary

Node nickname change event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldNickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Old node nickname |
| newNickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | New node nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnHostStarted-System-Int32-'></a>
### OnHostStarted(port) `method`

##### Summary

Host properly started event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Host listen port |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnNodeConnected-System-Net-IPAddress,System-String-'></a>
### OnNodeConnected(ip,nickname) `method`

##### Summary

Node connected event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Node nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnNodeDisconnected-System-Net-IPAddress,System-String-'></a>
### OnNodeDisconnected(ip,nickname) `method`

##### Summary

Node disconnected event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Node nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnNodeResumed-System-Net-IPAddress,System-String-'></a>
### OnNodeResumed(ip,nickname) `method`

##### Summary

Node resumed event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Node nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnNodeSuspended-System-Net-IPAddress,System-String-'></a>
### OnNodeSuspended(ip,nickname) `method`

##### Summary

Node suspended event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Node nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnReceivedMessage-System-String,System-String,System-Boolean-'></a>
### OnReceivedMessage(content,nickname) `method`

##### Summary

Received message event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Message content |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Sender nickname |

<a name='T-Lanchat-Common-Types-Handshake'></a>
## Handshake `type`

##### Namespace

Lanchat.Common.Types

##### Summary

Hansshake.

<a name='M-Lanchat-Common-Types-Handshake-#ctor-System-String,System-String,System-Int32-'></a>
### #ctor(nickname,publicKey,port) `constructor`

##### Summary

Handshake constructor.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Node nickname |
| publicKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Public RSA key |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Node host port |

<a name='P-Lanchat-Common-Types-Handshake-Nickname'></a>
### Nickname `property`

##### Summary

Node nickname.

<a name='P-Lanchat-Common-Types-Handshake-Port'></a>
### Port `property`

##### Summary

Node host port.

<a name='P-Lanchat-Common-Types-Handshake-PublicKey'></a>
### PublicKey `property`

##### Summary

Node host port.

<a name='T-Lanchat-Common-NetworkLib-Events-Args-HostStartedEventArgs'></a>
## HostStartedEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib.Events.Args

##### Summary

Host started.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-HostStartedEventArgs-Port'></a>
### Port `property`

##### Summary

Host listening port.

<a name='T-Lanchat-Common-NetworkLib-Api-Methods'></a>
## Methods `type`

##### Namespace

Lanchat.Common.NetworkLib.Api

##### Summary

Network API outputs class.

<a name='M-Lanchat-Common-NetworkLib-Api-Methods-Connect-System-Net-IPAddress,System-Int32-'></a>
### Connect(ip,port) `method`

##### Summary

Manual connect.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Node host port |

<a name='M-Lanchat-Common-NetworkLib-Api-Methods-GetNode-System-Net-IPAddress-'></a>
### GetNode(ip) `method`

##### Summary

Get node by IP.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | IP address |

<a name='M-Lanchat-Common-NetworkLib-Api-Methods-GetNode-System-String-'></a>
### GetNode(nickname) `method`

##### Summary

Get node by nickname.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Nickname |

<a name='M-Lanchat-Common-NetworkLib-Api-Methods-SendAll-System-String-'></a>
### SendAll(message) `method`

##### Summary

Send message to all nodes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | content |

<a name='T-Lanchat-Common-NetworkLib-Network'></a>
## Network `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Main class of network lib.

<a name='M-Lanchat-Common-NetworkLib-Network-#ctor-System-Int32,System-String,System-Int32,System-Int32-'></a>
### #ctor(broadcastPort,nickname,hostPort,heartbeatTimeout) `constructor`

##### Summary

Network constructor.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| broadcastPort | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | UDP broadcast port |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Self nickname |
| hostPort | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | TCP host port. Set to -1 to use free ephemeral port |
| heartbeatTimeout | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Heartbeat lifetime in ms |

<a name='P-Lanchat-Common-NetworkLib-Network-Events'></a>
### Events `property`

##### Summary

Network API inputs class.

<a name='P-Lanchat-Common-NetworkLib-Network-Methods'></a>
### Methods `property`

##### Summary

Network API outputs class.

<a name='P-Lanchat-Common-NetworkLib-Network-Nickname'></a>
### Nickname `property`

##### Summary

Self nickname. On set it sends new nickname to connected client.

<a name='P-Lanchat-Common-NetworkLib-Network-NodeList'></a>
### NodeList `property`

##### Summary

All nodes here.

<a name='M-Lanchat-Common-NetworkLib-Network-Dispose'></a>
### Dispose() `method`

##### Summary

Dispose network.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Common-NetworkLib-Network-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Dispose network.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-Lanchat-Common-NetworkLib-Network-Finalize'></a>
### Finalize() `method`

##### Summary

Dispose network.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Common-NetworkLib-Network-Start'></a>
### Start() `method`

##### Summary

Start host, broadcast and listen.

##### Parameters

This method has no parameters.

<a name='T-Lanchat-Common-NetworkLib-Node'></a>
## Node `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Represents network node.

<a name='M-Lanchat-Common-NetworkLib-Node-#ctor-System-Net-IPAddress,Lanchat-Common-NetworkLib-Network-'></a>
### #ctor(ip,network) `constructor`

##### Summary

Node constructor with known port.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node IP |
| network | [Lanchat.Common.NetworkLib.Network](#T-Lanchat-Common-NetworkLib-Network 'Lanchat.Common.NetworkLib.Network') | Network |

<a name='P-Lanchat-Common-NetworkLib-Node-ClearNickname'></a>
### ClearNickname `property`

##### Summary

Nickname without number.

<a name='P-Lanchat-Common-NetworkLib-Node-Handshake'></a>
### Handshake `property`

##### Summary

Handshake.

<a name='P-Lanchat-Common-NetworkLib-Node-Heartbeat'></a>
### Heartbeat `property`

##### Summary

Last heartbeat status.

<a name='P-Lanchat-Common-NetworkLib-Node-Ip'></a>
### Ip `property`

##### Summary

Node IP.

<a name='P-Lanchat-Common-NetworkLib-Node-Mute'></a>
### Mute `property`

##### Summary

Node mute value.

<a name='P-Lanchat-Common-NetworkLib-Node-Nickname'></a>
### Nickname `property`

##### Summary

Node nickname. If nicknames are duplicated returns nickname with number.

<a name='P-Lanchat-Common-NetworkLib-Node-Port'></a>
### Port `property`

##### Summary

Node TCP port.

<a name='P-Lanchat-Common-NetworkLib-Node-State'></a>
### State `property`

##### Summary

Node [Status](#T-Lanchat-Common-Types-Status 'Lanchat.Common.Types.Status').

<a name='M-Lanchat-Common-NetworkLib-Node-Dispose'></a>
### Dispose() `method`

##### Summary

Node dispose.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Common-NetworkLib-Node-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Node dispose.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Free any other managed objects |

<a name='M-Lanchat-Common-NetworkLib-Node-Finalize'></a>
### Finalize() `method`

##### Summary

Destructor.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Common-NetworkLib-Node-SendPrivate-System-String-'></a>
### SendPrivate(message) `method`

##### Summary

Send private message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | content |

<a name='T-Lanchat-Common-NetworkLib-Exceptions-NodeAlreadyExistException'></a>
## NodeAlreadyExistException `type`

##### Namespace

Lanchat.Common.NetworkLib.Exceptions

##### Summary

Node already exist in list.

<a name='T-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs'></a>
## NodeConnectionStatusEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib.Events.Args

##### Summary

Node connection status.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs-Nickname'></a>
### Nickname `property`

##### Summary

Node nickname.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-NodeConnectionStatusEventArgs-NodeIP'></a>
### NodeIP `property`

##### Summary

Node ip.

<a name='T-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs'></a>
## ReceivedMessageEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib.Events.Args

##### Summary

Received message.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Content'></a>
### Content `property`

##### Summary

Message content.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Nickname'></a>
### Nickname `property`

##### Summary

Sender nickname.

<a name='P-Lanchat-Common-NetworkLib-Events-Args-ReceivedMessageEventArgs-Private'></a>
### Private `property`

##### Summary

Private message.

<a name='T-Lanchat-Common-Types-Status'></a>
## Status `type`

##### Namespace

Lanchat.Common.Types

##### Summary

Possible node states

<a name='F-Lanchat-Common-Types-Status-Ready'></a>
### Ready `constants`

##### Summary

Ready to use.

<a name='F-Lanchat-Common-Types-Status-Resumed'></a>
### Resumed `constants`

##### Summary

Resumed after suspend.

<a name='F-Lanchat-Common-Types-Status-Suspended'></a>
### Suspended `constants`

##### Summary

Doesn't sends heartbeat.

<a name='F-Lanchat-Common-Types-Status-Waiting'></a>
### Waiting `constants`

##### Summary

Waiting for handshake and key exchange.
