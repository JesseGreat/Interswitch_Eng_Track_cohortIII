import React, { useEffect, useState } from "react";
import { DataTable } from "@/components/blacklist/data-table";
import { columns, blacklistdata, blItem } from "@/components/blacklist/columns";

const BlackListPage = () => {

  const [loading, setLoading] = useState(true);
  const [blacklistData, setBlacklistData] = useState([]);
  // const [blacklistData, setBlacklistData] = useState<Array<blItem>>([]);

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
        const dummy = [{
          name: "Name",
          reason: "dummy",
          time: "My time"
        }]
        setBlacklistData(dummy)
        console.log(blacklistData)
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
        <h2 className="text-4xl text-white font-bold">Itunu Shitta (RBAD)</h2>
        <div className="ml-6 mt-8">
          <h3 className="text-3xl font-semibold text-white">Blacklist</h3>
          <div className="bg-white py-6 mt-4">
            <DataTable columns={columns} data={blacklistdata} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default BlackListPage;
