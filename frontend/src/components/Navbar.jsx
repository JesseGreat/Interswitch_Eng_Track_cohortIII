import React from "react";
import logo from "../assets/images/logo.svg";
import { NavLink } from "react-router-dom";
import {
  FaPersonBooth,
  FaHeadset,
  FaSatelliteDish,
  FaHamburger,
  FaScrewdriver,
} from "react-icons/fa";

const Navbar = () => {
  const linkClass = ({ isActive }) =>
    isActive
      ? "bg-black text-white  hover:bg-gray-900 hover:text-white rounded-md px-3 py-2"
      : "text-white hover:bg-gray-900 hover:text-white rounded-md px-3 py-2";

  return (
    <>
      <nav className="bg-gray-300 border-gray-500 w-full">
        <div className=" mx-auto w-full px-2 sm:px-6 lg:px-8">
          <div className="flex h-20 items-center justify-between">
            <div className="flex flex-1 items-center justify-center md:items-stretch md:justify-start">
              {/* <!-- Logo --> */}
              <NavLink
                className="flex flex-shrink-0 items-center mr-4 ml-[2.5%]"
                to="/"
              >
                <img className="h-10 w-auto" src={logo} alt="React Jobs" />
                <span className="hidden md:block text-blue-900 text-2xl font-bold ml-32">
                  ISW_COHORT_3_GRP4
                </span>
              </NavLink>
              <div className="md:ml-auto">
                <div className="flex space-x-2">
                  <NavLink to="/" className={linkClass}>
                    <FaPersonBooth />
                  </NavLink>
                  <NavLink to="/job" className={linkClass}>
                    <FaHeadset />
                  </NavLink>
                  <NavLink to="add-job" className={linkClass}>
                    <FaScrewdriver />
                  </NavLink>
                  <NavLink to="dummy" className={linkClass}>
                    <FaHamburger />
                  </NavLink>
                </div>
              </div>
            </div>
          </div>
        </div>
      </nav>
    </>
  );
};

export default Navbar;
