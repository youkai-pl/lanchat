# Network
Lanchat nodes are connected direcectly. Manually connecting to each node will be inconvenient for user.

## Nodes list exchange
Last step of connection initialization is exchange of [NodesList](/protocol/api#nodeslist). 
Nodes list containts IP addresses of nodes already connected with sender. 
The receiving node will try to connect to these addresses.

## UDP nodes detection
Nodes can detect each other by listetnig and sending UDP broadcasts (default on port **3646**).
Datagram have to contain [Announce](/protocol/api#announce-udp) model. 

## NAT
~~Lanchat is designed to work in LAN networks without NAT between hosts. However connection can be estabilished if one node has external IP. Only two nodes can be connected this way.~~
Bad idea.