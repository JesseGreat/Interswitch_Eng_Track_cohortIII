import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify';
import Spinner from '@/components/ui/Spinner'

const CreateUser = ({ addUserSubmit }) => {

    const [email, setEmail] = useState('')
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
                toast.success("User created")
                setLoading(() => {!loading});
                // return navigate('/users')
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
                        <input onChange={(e) => setRole(e.target.value)} className='p-2 rounded-lg' placeholder='0' type='number' />
                        <button type='submit' className={'bg-blue-500 text-center items-center text-white p-2 mt-4 rounded-lg'}>
                            {loading ?
                                (<Spinner loading={loading} />) :
                                "Add User"
                            }
                        </button>
                    </form>

                </div>
            </div>
        </div>
    )
}

export default CreateUser