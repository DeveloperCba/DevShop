
document.addEventListener("DOMContentLoaded", () => {

    setTimeout(() => {
        let version = document.querySelector(".swagger-ui .topbar .download-url-wrapper .select-label span");
        if (version) {
            version.innerHTML = "Selecione a versão:";
        }
    }, 1);

    setTimeout(() => {
        let authorize = document.querySelector(".swagger-ui .btn.authorize span");
        if (authorize) {
            authorize.innerText = "Autorizar";
        }
    }, 300);
  
});

