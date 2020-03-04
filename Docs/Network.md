# Lancaht 2 network API and guide

Version 2.0.0.2007

## How it works

### Node

Lanchat instances are called nodes. Every node has separated TCP client and host. 
They also use UDP broadcasts to detect other nodes on the network.

<img src="./scheme.svg" width="350">

### Connection

There are three ways to establish connection between two nodes.

#### Broadcast and handshake

 1. First node (A) starts host on random or specified port
 2. Node A starts broadcasting self port host number and id
 3. Second node (B) detects node A
 4. Node B connects to A host and sends handshake
 5. Node B timer starts
 6. Node A timer starts when a connection with B is detected.
 7. Node A connects to B on port received in handshake
 8. Node A sends handshake to B
 9. Node A sends list of nodes connected with him
10. Node A generate AES key and sends it to B encrypted with B public key
11. After handshake receive B do points 9 and 10 for A
12. If all steps are done correctly before timers elapse connection is established
13. Nodes begin to send heartbeat.

If node A detect B before receive TCP connection and heartbeat from it, A just start from point 1.
A will change to B, and B into A (wtf).
Node won't create duplicated connection because library check is connection with IP alredy exist before begin process.

#### List

If for some reason UDP broadcast doesn't work properly exchange of connected nondes list has been added.
After receive list (sended in point 9) node try to connect with each item from it. Points 1-3 was skipped.

#### Manual

When the IP node and host port are known, the connection can be established manually. Points 1-3 was skipped.

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