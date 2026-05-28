document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById("loginForm");
    const btnSobre = document.getElementById("btnSobre");

    if (loginForm) {
        loginForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const usuario = document.getElementById("usuario").value.trim();
            const senha = document.getElementById("senha").value;

            // LOGIN DO ADMINISTRADOR
            if (usuario === "debuggers@cefsa.com" && senha === "Debuggers123") {
                sessionStorage.setItem("nidus_perfil", "admin"); // Salva que é Admin
                window.location.href = "/Home/Fazendas";
            }
            // LOGIN DO CLIENTE
            else if (usuario === "Rosalem@fazenda.com" && senha === "Rosalem123") {
                sessionStorage.setItem("nidus_perfil", "cliente"); // Salva que é Cliente
                window.location.href = "/Home/Fazendas";
            }
            else {
                alert("Usuário ou senha incorretos!");
            }
        });
    }

    if (btnSobre) {
        btnSobre.addEventListener("click", function () {
            window.location.href = "/Home/Privacy";
        });
    }
});