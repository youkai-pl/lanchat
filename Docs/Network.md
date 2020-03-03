# Lancaht 2 networking guide

## Structure



## API

### Transmitted data
All data is sent as json with have one root key and one or more childs.

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

| Key           | Value | Description                       | 
| ------------- | ----- | --------------------------------- |
| Id            | Guid  | Used to recognize self broadcasts |
| Port          | int   | TCP host port number              |

#### handshake

Contains information required to create a connection

| Key           | Value  | Description                                             | 
| ------------- | ------ | ------------------------------------------------------- |
| Nickname      | string | Node nickname                                           |
| Port          | int    | TCP host port number                                    |
| PublicKey     | string | RSA public key serialized from RSACryptoServiceProvider |

#### key

AES key encrypted with RSA

| Key           | Value  | Description                                             | 
| ------------- | ------ | :-----------------------------------------------------: |
| AesKey        | string | -                                                       |
| AesIV         | string | -                                                       |

#### heartbeat

Empty packet sent to check connection status

#### message

Message encrypted with AES and base64 (for transport)

| Key           | Value  | Description                                             | 
| ------------- | ------ | ------------------------------------------------------- |
| json root     | string | Encrypted message content                               |

#### nickname

Packet sent after node nickname change or node reconnect

| Key           | Value  | Description                                             | 
| ------------- | ------ | ------------------------------------------------------- |
| json root     | string | Node Nickname                                           |

#### list

List of IP addresses of nodes connected to the sending node. 
Used when UDP broadcast doesn't work for some reason

| Key           | Value         | Description                                              | 
| ------------- | ------------- | -------------------------------------------------------- |
| json array    | { Ip, Port }  | Array with objects containg nodes ip addresses and ports |