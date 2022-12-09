export var DisconnectReason;
(function (DisconnectReason) {
    DisconnectReason[DisconnectReason["NONE"] = 0] = "NONE";
    DisconnectReason[DisconnectReason["ON_REFRESH"] = 1] = "ON_REFRESH";
    DisconnectReason[DisconnectReason["ON_BROWSE"] = 2] = "ON_BROWSE";
    DisconnectReason[DisconnectReason["ON_RECONNECT"] = 3] = "ON_RECONNECT";
    DisconnectReason[DisconnectReason["ON_ERROR"] = 4] = "ON_ERROR";
    DisconnectReason[DisconnectReason["ON_OTHER"] = 10] = "ON_OTHER";
})(DisconnectReason || (DisconnectReason = {}));
export var ConnectReason;
(function (ConnectReason) {
    ConnectReason[ConnectReason["NONE"] = 0] = "NONE";
    ConnectReason[ConnectReason["AFTER_REFRESH"] = 1] = "AFTER_REFRESH";
    ConnectReason[ConnectReason["AFTER_BROWSE"] = 2] = "AFTER_BROWSE";
    ConnectReason[ConnectReason["AFTER_RECONNECT"] = 3] = "AFTER_RECONNECT";
    ConnectReason[ConnectReason["AFTER_ERROR"] = 4] = "AFTER_ERROR";
    ConnectReason[ConnectReason["AFTER_OTHER"] = 10] = "AFTER_OTHER";
})(ConnectReason || (ConnectReason = {}));
