import React from "react";
import Navbar from "../components/Navbar";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Sidebar from "../components/SIdebar";

const MainLayout = ({ role, loggedInUser }) => {
  return (
    <>
      <div className="h-screen">
        <Navbar />
        <div className="flex w-full h-screen">
          <Sidebar role={role} loggedInUser={loggedInUser} />
          <div className="w-full">
            <Outlet />
          </div>
        </div>
      </div>
      <ToastContainer />
    </>
  );
};

export default MainLayout;
