// src/App.jsx

import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Bienvenida from "./pages/Bienvenida";
import PuestosActivos from "./pages/PuestosActivos";
import Oferentes from "./pages/Oferentes";
import DetalleOferente from "./pages/DetalleOferente";
import Layout from "./components/Layout";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />} />
                <Route element={<Layout />}>
                    <Route path="/bienvenida" element={<Bienvenida />} />
                    <Route path="/puestos" element={<PuestosActivos />} />
                    <Route path="/oferentes" element={<Oferentes />} />
                    <Route path="/detalle-oferente" element={<DetalleOferente />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;