import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";

import Login from "./pages/Login";
import Bienvenida from "./pages/Bienvenida";

import Layout from "./components/Layout";

function App() {

    return (

        <BrowserRouter>

            <Routes>

                {/* Página de Login */}
                <Route
                    path="/"
                    element={<Login />}
                />

                {/* Páginas con Layout */}
                <Route element={<Layout />}>

                    <Route
                        path="/bienvenida"
                        element={<Bienvenida />}
                    />

                </Route>

            </Routes>

        </BrowserRouter>

    );

}

export default App;