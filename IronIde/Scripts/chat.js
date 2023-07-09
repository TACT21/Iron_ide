const connection = new signalR.HubConnectionBuilder()
    .withUrl("ironchat.azurewebsites.net/chathub")
    .configureLogging(signalR.LogLevel.Information).
    withAutomaticReconnect()
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

function Send() {
    try {
        await connection.invoke("SendMessage",
            document.getElementById("displayname").innerText,
            document.getElementById("content").value);
    } catch (err) {
        console.error(err);
    }
}

connection.on("ReceiveMessage", (user, message) => {
    var card = document.createElement("div");
    card.classList.add("card__textbox");

    var title = document.createElement("div");
    title.classList.add("card__titletext");
    title.innerText = user;
    card.appendChild(title);

    var content = document.createElement("div");
    content.classList.add("card__overviewtext");
    content.innerText = message;
    card.appendChild(content);

    document.getElementById("chat_room").appendChild(card);
});