// service worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.getRegistration()
    .then (registration => {
      // 登録中の SW がなければ、これが初回登録である
      const firstRegistration = (registration === undefined);
      // SW を登録する
      navigator.serviceWorker.register("service_worker.js")
      .then(registration => {
        // 初回登録でなければ更新が見つかったかチェックする
        if (!firstRegistration) {
          registration.addEventListener('updatefound', () => {
            const installingWorker = registration.installing;
            if (installingWorker != null) {
              installingWorker.onstatechange = e => {
                if (e.target.state == 'installed') {
                  registration.unregister();
                  if(document.getElementById("chenge")){
                    document.getElementById("chenge").classList.remove("hide");
                  }
                }
              };
            }
          });
        }
      });
    });
  }
  
// Login to project
function LoginPrj(){
    document.location = `${document.location.origin}/ide/cowork/cowork.html?projectId=${document.getElementById("prjId").value}`
}