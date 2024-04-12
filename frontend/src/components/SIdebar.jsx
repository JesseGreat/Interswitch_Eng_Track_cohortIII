import React from 'react'
import { FaClock } from 'react-icons/fa';
import { FaPeopleGroup, FaPerson } from 'react-icons/fa6';
import { NavLink } from "react-router-dom";


const Sidebar = ({ role, loggedInUser }) => {

  const linkClass = ({ isActive }) =>
    isActive
      ? "bg-black border-b my-2 text-white font-semibold flex hover:bg-gray-900 hover:text-white rounded-md px-3 py-2"
      : "text-black border-b my-2 font-semibold my-2 flex hover:bg-gray-900 hover:text-white rounded-md px-3 py-2";

  return (
    <div id="Main" className="bg-gray-300 border-gray-500 xl:rounded-lg flex flex-col justify-start items-start h-full w-[20%]">
      <div className="md:ml-0 flex flex-col w-full">
        <div className="flex flex-col space-x-2 items">
          <NavLink to="/" className="disabled">
          </NavLink>
          <NavLink to="/vendors" className={linkClass}>
            <FaPerson className=' text-red-500 mx-2' /> Vendors
          </NavLink>
          { localStorage.getItem("role") == "1" ? (
            <>
              <NavLink to="/users" className={linkClass}>
                <FaPeopleGroup className=' text-red-500 mx-2' /> Users
              </NavLink>
              <NavLink to="/create-user" className={linkClass}>
                <FaPeopleGroup className=' text-red-500 mx-2' /> Create User
              </NavLink>
              
            </>
          ) : (
            <>
              <NavLink to="/blacklist" className={linkClass}>
                <FaPeopleGroup className=' text-red-500 mx-2' /> Blacklist
              </NavLink>
              <NavLink to="/blacklist-items" className={linkClass}>
                <FaPeopleGroup className=' text-red-500 mx-2' /> Blacklist Items
              </NavLink>

            </>
          )
          }

          <NavLink to="/vendorprofile" className={linkClass}>
            <FaClock className=' text-red-500 mx-2' /> Vendor Profile
          </NavLink>
        </div>
      </div>


    </div>
  )
}

export default Sidebar