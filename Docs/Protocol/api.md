# API
Lanchat uses JSONs with one root key which specifies the type of value.

For example:

```json
{
    "Message":
    {
        "Content": "HaQ\u002BpFLc\u002Bv6R2cjKP\u002BpB1A==",
        "Private": false
    }
} 
```

```json
{
    "NodesList":
    [
        "192.168.1.3",
        "192.168.1.4"
    ]
} 
```

# Connection

## Handshake
| Key       | C# type                 | Max length | Description   |
|-----------|-------------------------|------------|---------------|
| Nickname  | string                  | 20         | User nickname |
| Status    | [Status](#status)       |            | User status   |
| PublicKey | [PublicKey](#publickey) |            | RSA key info  |

## KeyInfo
| Key    | C# type | Description       |
|--------|---------|-------------------|
| AesKey | byte[]  | AES key in base64 |
| AesIv  | byte[]  | AES IV in base64  |

## NodesList
| Key  | C# type         | Description                                          |
|------|-----------------|------------------------------------------------------|
| root | List<IPAddress> | Array of IP addresses previously connected with node |

## ConnectionControl
| Key    | C# type                              | Description                        |
|--------|--------------------------------------|------------------------------------|
| Status | [ConnectionStatus](#connectionstatus) | Information about connection state |

## Announce (UDP)
| Key      | C# type | Description                             |
|----------|---------|-----------------------------------------|
| Guid     | string  | Random GUID for ignoring own broadcasts |
| Nickname | string  | User nickname                           |

# Chat

## Message
| Key     | C# type | Max length | Description                  |
|---------|---------|------------|------------------------------|
| Content | string  | 1500       | Encrypted message in base64  |
| Private | bool    |            | Direct message to single user|

## NicknameUpdate
| Key         | C# type | Max length | Description       |
|-------------|---------|------------|-------------------|
| NewNickname | string  | 20         | New user nickname |

## UserStatusUpdate
| Key       | C# type           | Description     |
|-----------|-------------------|-----------------|
| NewUserStatus | [UserStatus](#userstatus) | New user status |

# FileTransfer

## FileReceiveRequest
| Key         | C# type | Max length | Description                        |
|-------------|---------|------------|------------------------------------|
| FileName    | string  | 100        | Name and extension of sending file |
| PartsCount  | long    |            | Size of file in chunks             |

## FilePart
| Key  | C# type | Max length | Description                    |
|------|---------|------------|--------------------------------|
| Data | string  | 1398102    | Encrypted file chunk in base64 |

## FileTransferControl
| Key    | C# type                                   | Description                           |
|--------|-------------------------------------------|---------------------------------------|
| Status | [FileTransferStatus](#filetransferstatus) | Information about file transfer state |

# Structs

## PublicKey
| Key        | C# type |
|------------|---------|
| RsaModulus | byte[]  |
| RsaExponent| byte[]  |

# Enums

## UserStatus
* Online
* AwayFromKeyboard
* DoNotDisturb

## ConnectionStatus
* RemoteDisconnect

## FileTransferStatus
* Accepted
* Rejected
* ReceiverError
* SenderError
* Finished
