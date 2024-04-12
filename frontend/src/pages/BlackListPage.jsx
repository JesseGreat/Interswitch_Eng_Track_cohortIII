import React, { useEffect, useState } from "react";
import { DataTable } from "@/components/blacklist/data-table";
import { columns, blacklistdata, blItem } from "@/components/blacklist/columns";
import Spinner from '@/components/ui/Spinner'


const BlackListPage = () => {

  const [loading, setLoading] = useState(true);
  const [blacklistData, setBlacklistData] = useState([]);
  // const [blacklistData, setBlacklistData] = useState<Array<blItem>>([]);
  const fullName = localStorage.getItem("fullName")

  useEffect(() => {
    const fetchData = async () => {
      const API_URL = 'https://blackliskappapi.azurewebsites.net/api/Item/get-all-items'
      const token = localStorage.getItem("auth")
      console.log("token", token)
      console.log("fullName", fullName)

      try {
        const response = await fetch(API_URL, {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        });
        const data = await response.json();
        console.log(data)

        
        const users = data.content
        // Filter users where isBlackListed is true
        const blacklistedUsers = users.filter(user => user.isBlacklisted);
        const formatted = blacklistedUsers.map((user) => ({
          name: user.name,
          reason: '',
          time: "Blacklisted"
        }))
        setBlacklistData(formatted)
        console.log(formatted)
        console.log("BLACKLISTED: ", blacklistedUsers)
        // setBlacklistData(data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching data:", error);
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="bg-customBlue h-full">
      <div className="mx-8 py-6">
        <h2 className="text-4xl text-white font-bold">{ fullName } </h2>
        <div className="ml-6 mt-8">
          <h3 className="text-3xl font-semibold text-white">Blacklist</h3>
          <div className="bg-white py-6 mt-4">
            {loading ?
              (<Spinner loading={loading} />) 
              :
              (<DataTable columns={columns} data={blacklistData} />)
            }
          </div>
        </div>
      </div>
    </div>
  );
};

export default BlackListPage;
