import React, { useState } from 'react'
import { Route, createBrowserRouter, createRoutesFromElements, RouterProvider } from 'react-router-dom'
import BlackListPage from './pages/BlackListPage'
import MainLayout from './layouts/MainLayout'
import NotFound from './pages/NotFound'
import VendorProfile from './pages/VendorProfile'
import VendorList from './pages/VendorList'
import Login from './pages/Login'
import UsersPage from './pages/UsersPage'
import CreateUser from './pages/CreateUser'


  // Login func
  const loginFunc = async (user) => {
    const res = await fetch('https://blackliskappapi.azurewebsites.net/api/Authentication/authenticate-user', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(user)
    });
    return res.json();
  }

  // AddUSer func
  const addUserFunc = async (user) => {
    const token = localStorage.getItem("auth")
    const res = await fetch('https://blackliskappapi.azurewebsites.net/api/User/create-new-user', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(user)
    });
    return res.json();
  }


const App = () => {

  const [role, setRole] = useState(null);
  const [loggedInUser, setLoggedInUser] = useState(null);
  
  const router = createBrowserRouter(
    createRoutesFromElements(
      // <Route path='/' element={<MainLayout />} >
      //   <Route path='/login' element={<Login login={loginFunc} />} />
      //   <Route index element={<VendorList />} />
      //   <Route path='/blacklist' element={<BlackListPage />} />
      //   <Route path='/vendorprofile' element={<VendorProfile/>} />
      //   <Route path='/vendorlist' element={<VendorList/>} />
      //   <Route path='*' element={<NotFound />} />
      // </Route>
      <Route path="/">
        {/* MainLayout with nested routes */}
        <Route element={<MainLayout role={role} loggedInUser={setLoggedInUser} />}>
          <Route index element={<VendorList />} />
          <Route path="/blacklist" element={<BlackListPage />} />
          <Route path="/users" element={<UsersPage />} />
          <Route path="/create-user" element={<CreateUser addUserSubmit={addUserFunc} />} />
          <Route path="/vendorprofile" element={<VendorProfile />} />
          <Route path="/vendorlist" element={<VendorList />} />
          <Route path="*" element={<NotFound />} />
        </Route>

        {/* Login page without MainLayout */}
        <Route path="/login" element={<Login login={loginFunc} setRole={setRole} loggedInUser={setLoggedInUser} />} />
      </Route>
    )
  )

  return <RouterProvider router={router} />
}

export default App