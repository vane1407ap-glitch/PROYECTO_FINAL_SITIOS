import Header from "./Header";
import Sidebar from "./Sidebar";
import { Outlet } from "react-router-dom";

function Layout() {
  return (
    <div className="app-container">
      <Sidebar />

      <div className="content-area">
        <Header />

        <main className="main-content">
          <Outlet />
        </main>

        <footer className="app-footer">
          AdminPersonal · Sistema de Gestión de Personal
        </footer>
      </div>
    </div>
  );
}

export default Layout;