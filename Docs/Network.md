# Lancaht 2 network API and guide

Version 2.0.0.2007

## How it works

### Node

Lanchat instances are called nodes. Every node has separated TCP client and host. 
They also use UDP broadcasts to detect other nodes on the network.

<img src="./Assets/Network.svg" width="350">

### Connection

There are three ways to establish connection between two nodes.

* Broadcast
* Nodes list
* Manual

<img src="./Assets/Connection.svg">

### Heartbeat

Nodes send blank package every second for detect broken connections. 
If node doesn't receive heartbeat within the time specified in config (5 seconds in default) it closes socket.

## API

### Transmitted data

#### Examples
```json
{
    "message": "W60NmRuhifxSLM7V9sPtOg=="
}
```

```json
{
    "list": 
    [
        {
            "Ip": "172.17.255.183",
            "Port": 46417
        }
    ]
}
```

```json
{
    "heartbeat": null
}
```

### List

#### paperplane

Used for detecting nodes by UDP broadcasts

| Key  | Value | Description                       | 
| ---- | ----- | --------------------------------- |
| Id   | Guid  | Used to recognize self broadcasts |
| Port | int   | TCP host port number              |

#### handshake

Contains information required to create a connection

| Key       | Value  | Description                                             | 
| --------- | ------ | ------------------------------------------------------- |
| Nickname  | string | Node nickname                                           |
| Port      | int    | TCP host port number                                    |
| PublicKey | string | RSA public key serialized from RSACryptoServiceProvider |

#### key

AES key encrypted with RSA

| Key    | Value  | Description                                             | 
| ------ | ------ | :-----------------------------------------------------: |
| AesKey | string | -                                                       |
| AesIV  | string | -                                                       |

#### heartbeat

Empty packet sent to check connection status

#### message

Message encrypted with AES and base64 (for transport)

| Key  | Value  | Description                                             | 
| ---- | ------ | ------------------------------------------------------- |
| root | string | Encrypted message content                               |

#### nickname

Packet sent after node nickname change or node reconnect

| Key      | Value  | Description                                             | 
| -------- | ------ | ------------------------------------------------------- |
| root     | string | Node Nickname                                           |

#### list

List of IP addresses of nodes connected to the sending node. 
Used when UDP broadcast doesn't work for some reason

| Key           | Value         | Description                                              | 
| ------------- | ------------- | -------------------------------------------------------- |
| array in root | { Ip, Port }  | Array with objects containg nodes ip addresses and ports |