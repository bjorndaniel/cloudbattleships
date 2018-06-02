let isInitialized = false;
let connectionInfo = {};
let connection = {};

Blazor.registerFunction("Battleships.BlazorUI.JsInterop.InitSignalR", () => {
    if (isInitialized) {
        return;
    }
    connectionInfo = JSON.parse(localStorage.getItem("Connection"));
    isInitialized = true;
    console.log("Setting up SignalR connection");
    getConnectionInfo().then(info => {
        const options = {
            accessTokenFactory: () => info.accessKey
        };
        connection = new signalR
            .HubConnectionBuilder()
            .withUrl(info.endpoint, options)
            .configureLogging(signalR.LogLevel.Information)
            .build();
        connection.on("ReceiveMessage", messageReceived);
        connection.on("GameUpdated", gameUpdated);
        connection.onclose(() => console.log("disconnected"));
        connection
            .start()
            .then(() => {
                console.log("Established SignalR connection")
                addEventHandlers();
            })
            .catch(console.error);
    });
});

function getConnectionInfo() {
    return axios
        .post(`${connectionInfo.baseUrl}${connectionInfo.negotiate}`)
        .then(resp => resp.data);
}

function addEventHandlers() {
    document.getElementById("sendButton").addEventListener("click", event => {
        const user = document.getElementById("userName").value;
        const message = document.getElementById("messageInput").value;
        document.getElementById("messageInput").value = "";
        axios
            .post(`${connectionInfo.baseUrl}${connectionInfo.messages}`, { message: message, user: user })
            .then((result) => console.log(result));
        event.preventDefault();
    });

    document.getElementById("btnPlay").addEventListener("click", event => {
        event.preventDefault();
        const user = document.getElementById("userName").value;
        const clientId = document.getElementById("hfClientId").value;
        axios
            .post(`${connectionInfo.baseUrl}${connectionInfo.initGame}`, { clientId: clientId, user: user })
            .then((result) => console.log(result));
    });

    document.getElementById("btnEnd").addEventListener("click", event => {
        const gameId = document.getElementById("gameId").value;
        axios
            .post(`${connectionInfo.baseUrl}${connectionInfo.playToEnd}`, { gameId: gameId })
            .then((result) => console.log(result));
        event.preventDefault();
    });

    document.getElementById("btnFire").addEventListener("click", event => {
        const playerId = document.getElementById("playerId").value;
        const gameId = document.getElementById("gameId").value;
        const row = document.getElementById("hfSelectedRow").value;
        const column = document.getElementById("hfSelectedColumn").value;
        axios
            .post(`${connectionInfo.baseUrl}${connectionInfo.fire}`, { gameId: gameId, playerId: playerId, row: row, column: column })
            .then((result) => console.log(result));
        event.preventDefault();
    });
}

function messageReceived(message) {
    console.log(message);
    const encodedMsg = `${message.User}: ${message.Message}`;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    var list = document.getElementById("messagesList");
    list.insertBefore(li, list.childNodes[0]);
}

function gameUpdated(game) {
    localStorage.setItem("Game", JSON.stringify(game.game));
    var button = document.getElementById("btnUpdateGame");
    button.click();
}

Blazor.registerFunction("Battleships.BlazorUI.JsInterop.Alert", function (message) {
    alert(message);
});