import API_GATEWAY from "./gateway";

const LOGIN_URL =
    `${API_GATEWAY}/gateway/login`;

export async function login(
    usuario,
    contrasena
) {

    console.log(LOGIN_URL);

    console.log(usuario);

    console.log(contrasena);

}