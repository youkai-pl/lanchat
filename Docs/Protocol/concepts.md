# Basic concepts

## Node
Lanchat instance is called **node** and consists of several parts:

* TCP server
* TCP clients
* UDP broadcasting
* UDP listening

UDP is used to detecting other nodes in LAN network. All standard communication between nodes uses TCP.
Connections between nodes are direct. The TCP server listens on selected port (**3645** in default) for clients running on ephemeral port.

## Connecting
Nodes have equal responsibilities most of the time. During connection initializing they're other because TCP protocol works in client-server manner. Session node sends [Handshake](api.md#handshake) first. Then client reply with own Handshake and AES key info ([KeyInfo](api.md#keyinfo)) encrypted with RSA public key taken from handshake. Session also sends KeyInfo as reply to client's Handshake.


| #  | Client node                                                       | Session node                                        |
|----|-------------------------------------------------------------------|-----------------------------------------------------|
| 1. | Connect to server.                                                |                                                     |
| 2. |                                                                   | Send handshake.                                     |
| 3. | Send handshake and AES key encrypted with RSA key from handshake. | Send AES key encrypted with RSA key from handshake. |
| 4. | Mark node as ready                                                | Mark node as ready.                                 |
| 5. | Send [NodesList](api.md#nodeslist)                                | Send [NodesList](api.md#nodeslist)                  |

## Encryption
Lanchat uses AES encryption for user data and RSA for encrypting AES keys.

* RSA
    * Key size: **2048**
    * Padding: **Pkcs1**
* AES
    * Key size: **256**
