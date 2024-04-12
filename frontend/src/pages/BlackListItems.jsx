import React, { useEffect, useState } from "react";
import { DataTable } from "@/components/blacklist/data-table";
import { columns, blacklistdata, blItem } from "@/components/blacklist/columns";
import Spinner from '@/components/ui/Spinner'


const BlackListItems = () => {

  const [loading, setLoading] = useState(true);
  const [blacklistItems, setBlacklistItems] = useState([]);
  // const [blacklistData, setBlacklistData] = useState<Array<blItem>>([]);
  const fullName = localStorage.getItem("fullName")

  useEffect(() => {
    const fetchData = async () => {
      const API_URL = 'https://blackliskappapi.azurewebsites.net/api/Item/get-blacklist-items'
      const token = localStorage.getItem("auth")
      console.log("token", token)

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
        const formatted = users.map((user) => ({
          name: user.name,
          reason: '',
          time: user.dateCreated
        }))
        setBlacklistItems(formatted)
        console.log(formatted)
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
        <h2 className="text-4xl text-white font-bold">{ fullName }</h2>
        <div className="ml-6 mt-8">
          <h3 className="text-3xl font-semibold text-white">Blacklist</h3>
          <div className="bg-white py-6 mt-4">
            {loading ?
              (<Spinner loading={loading} />) 
              :
              (<DataTable columns={columns} data={blacklistItems} />)
            }
          </div>
        </div>
      </div>
    </div>
  );
};

export default BlackListItems;
