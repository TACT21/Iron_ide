const sleep = (sec) => {
    return new Promise((resolve, reject) => {
        setTimeout(resolve, sec * 1000);
        //setTimeout(() => {reject(new Error("エラー！"))}, sec*1000);
    });
};

async function sleep_alt() {
    console.log("Wait");
    try {
        await sleep(1); // ここで10秒間止まります

        // ここに目的の処理を書きます。

    } catch (err) {
        console.error(err);
    }
}