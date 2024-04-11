import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast, ToastContainer } from 'react-toastify';
import Logo from '../assets/images/logo.svg'
import { CgSpinner } from 'react-icons/cg';

const Login = ({ login, setRole, loggedInUser }) => {

    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const loginUser = async (e) => {
        e.preventDefault();
        setLoading(true)

        const user = {
            "email": email,
            "password": password
        }

        try {
            const res = await login(user);
            if (res.status == 200) {
                const token = res.content.token;
                const userData = res.content.userDetails;
                const role = res.content.userDetails.roleId;
                localStorage.setItem("auth", token)
                localStorage.setItem("role", `${role}`)
                loggedInUser = userData
                setRole(userData.roleId)
                console.log("user: ", user);
                console.log("token: ", token)
                console.log("user details: ", userData);
                setLoading(() => {!loading});
                console.log(loading)
                return navigate('/')
            }
            else {
                console.log(res);
                setLoading(() => {!loading});
                toast.error("Invalid credentials")
            }
        } catch (error) {
            console.log(error)
            setLoading(() => {!loading});
            toast.error("An error occured")
        }
    }

    return (
        <>
            <div className='flex border w-full h-screen mx-auto items-center'>
                {/* Left half with custom blue background and login form */}
                <div className="bg-customBlue w-1/2 h-full p-6">
                    <div className='flex justify-center items-center h-full'>
                        <form onSubmit={loginUser} className='bg-customBlue flex flex-col'>
                            <input onChange={(e) => setEmail(e.target.value)} className='my-1 p-2 rounded-lg' placeholder='Email' />
                            <input onChange={(e) => setPassword(e.target.value)} className='p-2 rounded-lg' placeholder='Password' type='password' />
                            <button type='submit' className={'bg-blue-500 text-center items-center text-white p-2 mt-4 rounded-lg'}>
                                { loading ?
                                    <CgSpinner className='items-center' /> :
                                    "Login"
                                }
                            </button>
                        </form>

                    </div>
                </div>
                {/* Right half with logo */}
                <div className="bg-white w-1/2 h-full">
                    <div className="flex justify-center items-center h-full">
                        <img src={Logo} alt="Logo" className="w-32" />
                    </div>
                </div>
            </div>
            <ToastContainer />
        </>
    )
}

export default Login