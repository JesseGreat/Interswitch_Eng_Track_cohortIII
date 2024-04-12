import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify';
import Spinner from '@/components/ui/Spinner'

const CreateUser = ({ addUserSubmit, validateUser, updateUser }) => {

    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [fullName, setFullName] = useState('')
    const [role, setRole] = useState('')
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const addUser = async (e) => {
        e.preventDefault();
        setLoading(true)

        const user = {
            "emailAddress": email,
            "userRole": role
        }

        try {
            const res = await addUserSubmit(user);
            if (res.status == 201) {
                console.log(res)
                
                setLoading(() => {!loading});

                const userRes = await validateUser({"emailAddress": email})
                if (userRes.status == 200) {
                    console.log(userRes)

                    const userId = userRes.content.userId
                    const userToUpdate = {
                        "id": userId,
                        "fullName": fullName,
                        "password": password,
                        "isChangePassword": false
                    }
                    
                    const updatedUserRes = await updateUser(userToUpdate);
                    if (updatedUserRes.status == 200) {
                        console.log("updatedRes: ", updatedUserRes)
                    }
                    toast.success("User created with credentials")
                }
                return navigate('/users')
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
        <div className='flex border w-full h-screen mx-auto items-center'>
            <div className="bg-customBlue w-full h-full p-6">
                <div className='flex justify-center items-center h-full'>
                    <form onSubmit={addUser} className='bg-customBlue flex flex-col'>
                        <input onChange={(e) => setEmail(e.target.value)} className='my-1 p-2 rounded-lg' placeholder='Email' />
                        <input onChange={(e) => setPassword(e.target.value)} className='my-1 p-2 rounded-lg' placeholder='password' type='password' />
                        <input onChange={(e) => setFullName(e.target.value)} className='my-1 p-2 rounded-lg' placeholder='Full Name' type='text' />
                        <input onChange={(e) => setRole(e.target.value)} className='my-1 p-2 rounded-lg' placeholder='0' type='number' />
                        {
                            loading ?  (<Spinner loading={loading} />) 
                            : (
                                <button type='submit' className={'bg-blue-500 text-center items-center text-white p-2 mt-4 rounded-lg'}>
                                    Add User
                                </button>
                            )
                        }
                    </form>
                </div>
            </div>
        </div>
    )
}

export default CreateUser