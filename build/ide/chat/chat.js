const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://ironchat.azurewebsites.net/chathub")
    .configureLogging(signalR.LogLevel.Information)
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


var params = new URLSearchParams(document.location.search.substring(1))

async function Send() {
    try {
        await connection.invoke("SendMessage",
            params.get("displayname"),
            document.getElementById("bms_send_message").value);
        
    } catch (err) {
        console.error(err);
        return;
    }
    document.getElementById("bms_send_message").value = "";
    document.getElementById("bms_messages")
        .appendChild(
            MakeMassageElement(message,null).classList.add("bms_right")
        );
}

connection.on("ReceiveMessage", (user, message) => {
    document.getElementById("bms_messages")
        .appendChild(
            MakeMassageElement(message,user).classList.add("bms_left")
        );
});

function MakeMassageElement(content,name){
    var card = document.createElement("div");
    card.classList.add("bms_message");
    if(name){
        var title = document.createElement("div");
        title.classList.add("bms_message_name");
        title.innerText = name;
        card.appendChild(title);
    }
    
    var content = document.createElement("div");
    content.classList.add("bms_message_content");

    var text = document.createElement("div");
    text.classList.add("bms_message_text");

    content.appendChild(text);
    card.appendChild(content);

    return card;
}