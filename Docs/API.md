# Lanchat 2.1 API

Lanchat use JSON formatting and UTF-8 encoding to transfer data.

## Data exchange

All packets contains jsons with two keys in root (*Type*, *Data*) and multiple keys in *Data* key.

<table>
    <tr>
        <th>
            Type
        </th>
        <th>
            Keys
        </th>
        <th>
            Description
        </th>
        <th>
            Example
        </th>
    </tr>
    <tr>
        <td>
            Message
        </td>
        <td>
            <ul>
                <li>Data</li>
            </ul>
        </td>
        <td>
            Encrypted message sent to all nodes.
        </td>
        <td>
            <pre lang="json">
                {
                    "Type":"Message",
                    "Data":"559iuSOtpMZZLrmmMTXp9w=="
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Handshake
        </td>
        <td>
            <ul>
                <li>
                    Data
                    <ul>
                        <li>Nickname</li>
                        <li>
                            PublicKey
                            <ul>
                                <li>RsaModulus</li>
                                <li>RsaExpotent</li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            Node nickname and public key.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"Handshake",
                   "Data":{
                      "Nickname":"test",
                      "PublicKey":{
                         "RsaModulus":"very long string",
                         "RsaExponent":"AQAB"
                      }
                   }
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            NodesList
        </td>
        <td>
            <ul>
                <li>
                    Data
                    <ul>
                        <li>Array of ip addresses</li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            List of nodes alredy connected with sending node.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"NodesList",
                   "Data":[
                      "192.168.18.1",
                      "192.168.18.1"
                   ]
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            KeyInfo
        </td>
        <td>
            <ul>
                <li>Data
                    <ul>
                        <li>AesKey</li>
                        <li>AesIv</li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            Symetric encryption key. Encrypted with public key.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"KeyInfo",
                   "Data":{
                      "AesKey":"very long string",
                      "AesIv":"very long string"
                   }
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            NicknameUpdate
        </td>
        <td>
            <ul>
                <li>Data</li>
            </ul>
        </td>
        <td>
            New node nickname.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"NicknameUpdate",
                   "Data":"test"
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Goodbye
        </td>
        <td>
            null
        </td>
        <td>
            Sent after disconnect command. Blocking automatic reconnection in node.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"Goodbye",
                   "Data":null
                }
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            PrivateMessage
        </td>
        <td>
            Data
        </td>
        <td>
            Encrypted message sent to single node.
        </td>
        <td>
            <pre lang="json">
                {
                   "Type":"PrivateMessage",
                   "Data":"scRzgudmk4I30rU9h\u002BNFyQ=="
                }
            </pre>
        </td>
    </tr>
</table>