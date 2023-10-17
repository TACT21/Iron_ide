function PaletChenge(target){
    let element = document.getElementById('Palet');
    for (const child of element.children) {
        if(child.id == target){
            child.style.zIndex = 1;
        }else{
            child.style.zIndex = 0;
        }
    }
}

window.addEventListener("load",() => {
    console.log("load")
    PaletChenge("Filer");
})