const sleep = (sec) => {
    return new Promise((resolve, reject) => {
        setTimeout(resolve, sec);
        //setTimeout(() => {reject(new Error("エラー！"))}, sec*1000);
    });
};

const countUp = () => {
    console.log("Waiting..");
}

async function sleep_alt(e) {
    console.log("Wait");
    a = ""
    while (true) {
        setTimeout(countUp, sec);
        if (e.value != "") {
            a = e
        }
    }
    return a
}