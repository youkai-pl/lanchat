<a name='assembly'></a>
# Lanchat.Common

## Contents

- [ChangedNicknameEventArgs](#T-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs 'Lanchat.Common.NetworkLib.ChangedNicknameEventArgs')
  - [NewNickname](#P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-NewNickname 'Lanchat.Common.NetworkLib.ChangedNicknameEventArgs.NewNickname')
  - [OldNickname](#P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-OldNickname 'Lanchat.Common.NetworkLib.ChangedNicknameEventArgs.OldNickname')
  - [SenderIP](#P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-SenderIP 'Lanchat.Common.NetworkLib.ChangedNicknameEventArgs.SenderIP')
- [ConnectionFailedException](#T-Lanchat-Common-NetworkLib-ConnectionFailedException 'Lanchat.Common.NetworkLib.ConnectionFailedException')
- [Events](#T-Lanchat-Common-NetworkLib-Api-Events 'Lanchat.Common.NetworkLib.Api.Events')
  - [OnChangedNickname(oldNickname,newNickname,senderIP)](#M-Lanchat-Common-NetworkLib-Api-Events-OnChangedNickname-System-String,System-String,System-Net-IPAddress- 'Lanchat.Common.NetworkLib.Api.Events.OnChangedNickname(System.String,System.String,System.Net.IPAddress)')
  - [OnHostStarted(port)](#M-Lanchat-Common-NetworkLib-Api-Events-OnHostStarted-System-Int32- 'Lanchat.Common.NetworkLib.Api.Events.OnHostStarted(System.Int32)')
  - [OnNodeConnected(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeConnected-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeConnected(System.Net.IPAddress,System.String)')
  - [OnNodeDisconnected(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeDisconnected-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeDisconnected(System.Net.IPAddress,System.String)')
  - [OnNodeResumed(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeResumed-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeResumed(System.Net.IPAddress,System.String)')
  - [OnNodeSuspended(ip,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnNodeSuspended-System-Net-IPAddress,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnNodeSuspended(System.Net.IPAddress,System.String)')
  - [OnReceivedMessage(content,nickname)](#M-Lanchat-Common-NetworkLib-Api-Events-OnReceivedMessage-System-String,System-String- 'Lanchat.Common.NetworkLib.Api.Events.OnReceivedMessage(System.String,System.String)')
- [HostStartedEventArgs](#T-Lanchat-Common-NetworkLib-HostStartedEventArgs 'Lanchat.Common.NetworkLib.HostStartedEventArgs')
  - [Port](#P-Lanchat-Common-NetworkLib-HostStartedEventArgs-Port 'Lanchat.Common.NetworkLib.HostStartedEventArgs.Port')
- [Methods](#T-Lanchat-Common-NetworkLib-Api-Methods 'Lanchat.Common.NetworkLib.Api.Methods')
  - [Connect(ip,port)](#M-Lanchat-Common-NetworkLib-Api-Methods-Connect-System-Net-IPAddress,System-Int32- 'Lanchat.Common.NetworkLib.Api.Methods.Connect(System.Net.IPAddress,System.Int32)')
  - [SendAll(message)](#M-Lanchat-Common-NetworkLib-Api-Methods-SendAll-System-String- 'Lanchat.Common.NetworkLib.Api.Methods.SendAll(System.String)')
- [Network](#T-Lanchat-Common-NetworkLib-Network 'Lanchat.Common.NetworkLib.Network')
  - [#ctor(broadcastPort,nickname,hostPort)](#M-Lanchat-Common-NetworkLib-Network-#ctor-System-Int32,System-String,System-Int32- 'Lanchat.Common.NetworkLib.Network.#ctor(System.Int32,System.String,System.Int32)')
  - [BroadcastPort](#P-Lanchat-Common-NetworkLib-Network-BroadcastPort 'Lanchat.Common.NetworkLib.Network.BroadcastPort')
  - [Events](#P-Lanchat-Common-NetworkLib-Network-Events 'Lanchat.Common.NetworkLib.Network.Events')
  - [HostPort](#P-Lanchat-Common-NetworkLib-Network-HostPort 'Lanchat.Common.NetworkLib.Network.HostPort')
  - [Id](#P-Lanchat-Common-NetworkLib-Network-Id 'Lanchat.Common.NetworkLib.Network.Id')
  - [Methods](#P-Lanchat-Common-NetworkLib-Network-Methods 'Lanchat.Common.NetworkLib.Network.Methods')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Network-Nickname 'Lanchat.Common.NetworkLib.Network.Nickname')
  - [NodeList](#P-Lanchat-Common-NetworkLib-Network-NodeList 'Lanchat.Common.NetworkLib.Network.NodeList')
  - [PublicKey](#P-Lanchat-Common-NetworkLib-Network-PublicKey 'Lanchat.Common.NetworkLib.Network.PublicKey')
  - [Rsa](#P-Lanchat-Common-NetworkLib-Network-Rsa 'Lanchat.Common.NetworkLib.Network.Rsa')
  - [Dispose(disposing)](#M-Lanchat-Common-NetworkLib-Network-Dispose-System-Boolean- 'Lanchat.Common.NetworkLib.Network.Dispose(System.Boolean)')
  - [Dispose()](#M-Lanchat-Common-NetworkLib-Network-Dispose 'Lanchat.Common.NetworkLib.Network.Dispose')
  - [Finalize()](#M-Lanchat-Common-NetworkLib-Network-Finalize 'Lanchat.Common.NetworkLib.Network.Finalize')
  - [Start()](#M-Lanchat-Common-NetworkLib-Network-Start 'Lanchat.Common.NetworkLib.Network.Start')
- [Node](#T-Lanchat-Common-NetworkLib-Node 'Lanchat.Common.NetworkLib.Node')
  - [#ctor(id,port,ip)](#M-Lanchat-Common-NetworkLib-Node-#ctor-System-Guid,System-Int32,System-Net-IPAddress- 'Lanchat.Common.NetworkLib.Node.#ctor(System.Guid,System.Int32,System.Net.IPAddress)')
  - [#ctor(port,ip)](#M-Lanchat-Common-NetworkLib-Node-#ctor-System-Int32,System-Net-IPAddress- 'Lanchat.Common.NetworkLib.Node.#ctor(System.Int32,System.Net.IPAddress)')
  - [ClearNickname](#P-Lanchat-Common-NetworkLib-Node-ClearNickname 'Lanchat.Common.NetworkLib.Node.ClearNickname')
  - [HearbeatCount](#P-Lanchat-Common-NetworkLib-Node-HearbeatCount 'Lanchat.Common.NetworkLib.Node.HearbeatCount')
  - [Heartbeat](#P-Lanchat-Common-NetworkLib-Node-Heartbeat 'Lanchat.Common.NetworkLib.Node.Heartbeat')
  - [Id](#P-Lanchat-Common-NetworkLib-Node-Id 'Lanchat.Common.NetworkLib.Node.Id')
  - [Ip](#P-Lanchat-Common-NetworkLib-Node-Ip 'Lanchat.Common.NetworkLib.Node.Ip')
  - [Mute](#P-Lanchat-Common-NetworkLib-Node-Mute 'Lanchat.Common.NetworkLib.Node.Mute')
  - [Nickname](#P-Lanchat-Common-NetworkLib-Node-Nickname 'Lanchat.Common.NetworkLib.Node.Nickname')
  - [Port](#P-Lanchat-Common-NetworkLib-Node-Port 'Lanchat.Common.NetworkLib.Node.Port')
  - [PublicKey](#P-Lanchat-Common-NetworkLib-Node-PublicKey 'Lanchat.Common.NetworkLib.Node.PublicKey')
  - [State](#P-Lanchat-Common-NetworkLib-Node-State 'Lanchat.Common.NetworkLib.Node.State')
  - [Dispose()](#M-Lanchat-Common-NetworkLib-Node-Dispose 'Lanchat.Common.NetworkLib.Node.Dispose')
  - [Dispose(disposing)](#M-Lanchat-Common-NetworkLib-Node-Dispose-System-Boolean- 'Lanchat.Common.NetworkLib.Node.Dispose(System.Boolean)')
  - [Finalize()](#M-Lanchat-Common-NetworkLib-Node-Finalize 'Lanchat.Common.NetworkLib.Node.Finalize')
  - [OnStateChange()](#M-Lanchat-Common-NetworkLib-Node-OnStateChange 'Lanchat.Common.NetworkLib.Node.OnStateChange')
- [NodeAlreadyExistException](#T-Lanchat-Common-NetworkLib-NodeAlreadyExistException 'Lanchat.Common.NetworkLib.NodeAlreadyExistException')
- [NodeConnectionStatusEventArgs](#T-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs 'Lanchat.Common.NetworkLib.NodeConnectionStatusEventArgs')
  - [Nickname](#P-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs-Nickname 'Lanchat.Common.NetworkLib.NodeConnectionStatusEventArgs.Nickname')
  - [NodeIP](#P-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs-NodeIP 'Lanchat.Common.NetworkLib.NodeConnectionStatusEventArgs.NodeIP')
- [ReceivedMessageEventArgs](#T-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs 'Lanchat.Common.NetworkLib.ReceivedMessageEventArgs')
  - [Content](#P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-Content 'Lanchat.Common.NetworkLib.ReceivedMessageEventArgs.Content')
  - [Nickname](#P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-Nickname 'Lanchat.Common.NetworkLib.ReceivedMessageEventArgs.Nickname')
  - [SenderIP](#P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-SenderIP 'Lanchat.Common.NetworkLib.ReceivedMessageEventArgs.SenderIP')
- [Status](#T-Lanchat-Common-Types-Status 'Lanchat.Common.Types.Status')
  - [Ready](#F-Lanchat-Common-Types-Status-Ready 'Lanchat.Common.Types.Status.Ready')
  - [Resumed](#F-Lanchat-Common-Types-Status-Resumed 'Lanchat.Common.Types.Status.Resumed')
  - [Suspended](#F-Lanchat-Common-Types-Status-Suspended 'Lanchat.Common.Types.Status.Suspended')
  - [Waiting](#F-Lanchat-Common-Types-Status-Waiting 'Lanchat.Common.Types.Status.Waiting')

<a name='T-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs'></a>
## ChangedNicknameEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Changed node nickname event.

<a name='P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-NewNickname'></a>
### NewNickname `property`

##### Summary

New nickname.

<a name='P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-OldNickname'></a>
### OldNickname `property`

##### Summary

Old nickname.

<a name='P-Lanchat-Common-NetworkLib-ChangedNicknameEventArgs-SenderIP'></a>
### SenderIP `property`

##### Summary

IP of the sending node.

<a name='T-Lanchat-Common-NetworkLib-ConnectionFailedException'></a>
## ConnectionFailedException `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Cannot establish connection.

<a name='T-Lanchat-Common-NetworkLib-Api-Events'></a>
## Events `type`

##### Namespace

Lanchat.Common.NetworkLib.Api

##### Summary

Network API inputs class.

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnChangedNickname-System-String,System-String,System-Net-IPAddress-'></a>
### OnChangedNickname(oldNickname,newNickname,senderIP) `method`

##### Summary

Node nickname change event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldNickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Old node nickname |
| newNickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | New node nickname |
| senderIP | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node ip |

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

<a name='M-Lanchat-Common-NetworkLib-Api-Events-OnReceivedMessage-System-String,System-String-'></a>
### OnReceivedMessage(content,nickname) `method`

##### Summary

Received message event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Message content |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Sender nickname |

<a name='T-Lanchat-Common-NetworkLib-HostStartedEventArgs'></a>
## HostStartedEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Host started.

<a name='P-Lanchat-Common-NetworkLib-HostStartedEventArgs-Port'></a>
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

<a name='M-Lanchat-Common-NetworkLib-Network-#ctor-System-Int32,System-String,System-Int32-'></a>
### #ctor(broadcastPort,nickname,hostPort) `constructor`

##### Summary

Network constructor.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| broadcastPort | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | UDP broadcast port |
| nickname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Self nickname |
| hostPort | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | TCP host port. Set to -1 to use free ephemeral port |

<a name='P-Lanchat-Common-NetworkLib-Network-BroadcastPort'></a>
### BroadcastPort `property`

##### Summary

UDP broadcast port.

<a name='P-Lanchat-Common-NetworkLib-Network-Events'></a>
### Events `property`

##### Summary

Network API inputs class.

<a name='P-Lanchat-Common-NetworkLib-Network-HostPort'></a>
### HostPort `property`

##### Summary

TCP host port. Set to -1 for use free ephemeral port.

<a name='P-Lanchat-Common-NetworkLib-Network-Id'></a>
### Id `property`

##### Summary

Self ID. Used for checking udp broadcast duplicates.

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

<a name='P-Lanchat-Common-NetworkLib-Network-PublicKey'></a>
### PublicKey `property`

##### Summary

Self RSA public key.

<a name='P-Lanchat-Common-NetworkLib-Network-Rsa'></a>
### Rsa `property`

##### Summary

RSA provider.

<a name='M-Lanchat-Common-NetworkLib-Network-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Dispose network.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-Lanchat-Common-NetworkLib-Network-Dispose'></a>
### Dispose() `method`

##### Summary

Dispose network.

##### Parameters

This method has no parameters.

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

<a name='M-Lanchat-Common-NetworkLib-Node-#ctor-System-Guid,System-Int32,System-Net-IPAddress-'></a>
### #ctor(id,port,ip) `constructor`

##### Summary

Node constructor.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | Node ID |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Node TCP port |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node IP |

<a name='M-Lanchat-Common-NetworkLib-Node-#ctor-System-Int32,System-Net-IPAddress-'></a>
### #ctor(port,ip) `constructor`

##### Summary

Node constructor without id.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Node TCP port |
| ip | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node IP |

<a name='P-Lanchat-Common-NetworkLib-Node-ClearNickname'></a>
### ClearNickname `property`

##### Summary

Nickname without number.

<a name='P-Lanchat-Common-NetworkLib-Node-HearbeatCount'></a>
### HearbeatCount `property`

##### Summary

Heartbeat counter.

<a name='P-Lanchat-Common-NetworkLib-Node-Heartbeat'></a>
### Heartbeat `property`

##### Summary

Last heartbeat status.

<a name='P-Lanchat-Common-NetworkLib-Node-Id'></a>
### Id `property`

##### Summary

Node ID.

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

<a name='P-Lanchat-Common-NetworkLib-Node-PublicKey'></a>
### PublicKey `property`

##### Summary

Node public RSA key.

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

<a name='M-Lanchat-Common-NetworkLib-Node-OnStateChange'></a>
### OnStateChange() `method`

##### Summary

State change event.

##### Parameters

This method has no parameters.

<a name='T-Lanchat-Common-NetworkLib-NodeAlreadyExistException'></a>
## NodeAlreadyExistException `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Node already exist in list.

<a name='T-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs'></a>
## NodeConnectionStatusEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Node connection status.

<a name='P-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs-Nickname'></a>
### Nickname `property`

##### Summary

Node nickname.

<a name='P-Lanchat-Common-NetworkLib-NodeConnectionStatusEventArgs-NodeIP'></a>
### NodeIP `property`

##### Summary

Node ip.

<a name='T-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs'></a>
## ReceivedMessageEventArgs `type`

##### Namespace

Lanchat.Common.NetworkLib

##### Summary

Received message.

<a name='P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-Content'></a>
### Content `property`

##### Summary

Message content.

<a name='P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-Nickname'></a>
### Nickname `property`

##### Summary

Sender nickname.

<a name='P-Lanchat-Common-NetworkLib-ReceivedMessageEventArgs-SenderIP'></a>
### SenderIP `property`

##### Summary

IP of the sending node.

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
