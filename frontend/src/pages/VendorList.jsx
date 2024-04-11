import React, { useEffect, useState } from "react";
import { Checkbox } from "@/components/ui/checkbox";

const VendorList = () => {

  const [vendors, setVendors] = useState([])
  const [loading, setLoading] = useState(true)

  // useEffect(() => {
  //   const fetchVendors = async () => {
  //     const BASE_URL = 'https://blackliskappapi.azurewebsites.net/api/';


  //   }
  
  //   return () => {
  //     second
  //   }
  // }, [third])
  

  return (
    <div className="h-full">
      <div className="mx-8 py-6">
        <div>
          <div className="py-4 px-8 flex justify-between border-b border-b-gray-300">
            <div className="">
              <h2 className="text-2xl font-bold">Vendor List</h2>
            </div>
            <div className="">
              <button className="w-32 px-3 h-9 bg-customBlue border cursor-pointer py-1 font-medium text-sm">
                Add Vendor
              </button>
            </div>
          </div>
          <div className="flex py-8 gap-x-8">
            <div className="w-[30%] space-y-4 border-r">
              <div className="flex cursor-pointer justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Uche and Sons</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Olayemi and Associates</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Itunu and Sons</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Godfrey Joseph Limited</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Heirs Holdings </p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Vendor LearnLoom</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Vendor Collab Sphere</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Vendor ID:8-183</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Lorem Ipsum</p>
              </div>
              <div className="flex justify-between hover:bg-[#cde1f2] py-2 px-3 rounded">
                <Checkbox
                  aria-label="Select"
                  className="rounded border-customBlue p-3"
                />
                <p>Lorem Ipsum</p>
              </div>
            </div>
            <div className="w-[70%] space-y-4">
              <h2 className="font-bold text-3xl">
                Transact{" "}
                <span className="text-sm font-medium">Version 3.1.1</span>
              </h2>
              <div className="flex gap-x-12">
                <button className="w-32 px-3 h-9 bg-customBlue border cursor-pointer py-1 font-medium text-sm">
                  View
                </button>
                <button className="w-32 px-3 h-9 bg-white border cursor-pointer py-1 font-medium text-sm">
                  Transactions
                </button>
                <button className="w-32 px-3 h-9 bg-white border cursor-pointer py-1 font-medium text-sm">
                  Details
                </button>
                <button className="w-32 px-3 h-9 bg-red-700 border cursor-pointer py-1 font-medium text-sm">
                  Blacklist
                </button>
              </div>
              <div className="flex flex-col gap-y-8">
                <div className="space-y-3">
                  <div className="flex gap-x-12">
                    <div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor Type</p>
                        <p className="font-medium text-sm">Vendor Software</p>
                      </div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor Item</p>
                        <p className="font-medium text-sm">Vendor Number</p>
                      </div>
                    </div>
                    <div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor</p>
                        <p className="font-medium text-sm">
                          Vendor Finicial Managment
                        </p>
                      </div>
                    </div>
                  </div>
                  <div className="flex gap-x-12">
                    <div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor</p>
                        <p className="font-medium text-sm">Vendor User</p>
                      </div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor Tax:</p>
                        <p className="font-medium text-sm">Vendor Stock:80%</p>
                      </div>
                    </div>
                    <div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor</p>
                        <p className="font-medium text-sm">Vendor 1</p>
                      </div>
                      <div className="flex gap-x-3 items-center">
                        <p className="font-semibold">Vendor Vista</p>
                        <p className="font-medium text-sm">
                          Vendor Pricing Regular
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
                <div>
                  <div className="flex gap-x-3 items-center">
                    <p className="font-semibold">Vendor</p>
                    <p className="font-medium text-sm">Vendor</p>
                  </div>
                  <div className="flex gap-x-3 items-center">
                    <p className="font-semibold">Vendor</p>
                    <p className="font-medium text-sm">Vendor SKU:-</p>
                  </div>
                </div>
                <div className="flex justify-center gap-x-8 mt-8">
                  <button className="w-32 px-3 h-9 bg-customBlue border cursor-pointer py-1 font-medium text-sm">
                    Edit Vendor
                  </button>
                  <button className="w-32 px-3 h-9 bg-white border cursor-pointer py-1 font-medium text-sm">
                    Delete Vendor
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default VendorList;
