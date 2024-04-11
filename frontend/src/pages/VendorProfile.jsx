import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
const VendorProfile = () => {
  return (
    <div className="h-full space-y-6">
      <div className="bg-[#f0f0f0] flex justify-between  mt-4 mx-8 py-6 px-8">
        <Input
          placeholder="Search Vendors"
          className="rounded w-[300px] border-black"
        />
        <div className="flex gap-x-4">
          <button className="px-3 h-9 bg-white border cursor-pointer py-1 border-black font-medium text-sm">
            Add Vendor
          </button>
          <button className="w-12 px-3 h-9 ext-center bg-white border cursor-pointer py-1 border-black font-medium text-sm">
            View
          </button>
        </div>
      </div>
      <div className="bg-[#f0f0f0] mx-8">
        <div className="py-4 px-8 flex justify-between border-b border-b-gray-300">
          <div className="">
            <h2 className="text-2xl font-bold">Vendor Profile</h2>
          </div>
          <div className="">
            <DropdownMenu>
              <DropdownMenuTrigger className="bg-white px-4 py-1 rounded border-transparent">
                Dropdown &#xf078;
              </DropdownMenuTrigger>
              <DropdownMenuContent>
                <DropdownMenuLabel>My Account</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem>Profile</DropdownMenuItem>
                <DropdownMenuItem>Billing</DropdownMenuItem>
                <DropdownMenuItem>Team</DropdownMenuItem>
                <DropdownMenuItem>Subscription</DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>
        <div className="flex gap-x-6 px-8 py-8  border-b border-b-gray-300">
          <div>
            <h2 className="text-lg font-bold">Vendor Photo</h2>
          </div>
          <div>
            <img
              src={`https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRIZ5qd6H2ZmIVlNGi9ITEk22zp1pzECvD61rQX-__fYA&s`}
              width={200}
              height={200}
            />
          </div>
          <div className="mt-5">
            <button className="px-3 h-9 bg-white border cursor-pointer py-1 border-black font-medium text-sm">
              Change Photo
            </button>
            <p className="font-medium">Supported Format: jpg, png, jif</p>
          </div>
        </div>
        <div className="flex gap-x-6 px-8 py-8  border-b border-b-gray-300">
          <div>
            <h2 className="text-lg font-bold">Contact Information</h2>
          </div>
          <div className="space-y-3">
            <p className="text-sm font-medium">Vendors Full Name*</p>
            <Input
              placeholder="Enter Vendors Name"
              className="rounded w-[300px] border-black"
            />
          </div>
        </div>
        <div className="flex justify-between gap-x-6 px-8 py-8  border-b border-b-gray-300">
          <div>
            <h2 className="text-lg font-bold">Email Address</h2>
          </div>
          <div className="">
            <p className="text-sm font-medium underline">vendor@email.com</p>
          </div>
          <div className="">
            <button className="px-3 h-9 bg-white border cursor-pointer py-1 border-black font-medium text-sm">
              Edit Email
            </button>
          </div>
        </div>
        <div className=" px-8 py-24  border-b border-b-gray-300">
          <div className="flex justify-end">
            <DropdownMenu>
              <DropdownMenuTrigger className="bg-white px-4 py-1 rounded border-transparent">
                Dropdown &#xf078;
              </DropdownMenuTrigger>
              <DropdownMenuContent>
                <DropdownMenuLabel>My Account</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem>Profile</DropdownMenuItem>
                <DropdownMenuItem>Billing</DropdownMenuItem>
                <DropdownMenuItem>Team</DropdownMenuItem>
                <DropdownMenuItem>Subscription</DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>

          <div className="flex gap-x-6 justify-center">
            <button className="w-32 px-3 h-9 bg-red-700 border cursor-pointer py-1 border-black font-medium text-sm">
              Discard
            </button>
            <button className="w-32 px-3 h-9 bg-white border cursor-pointer py-1 border-black font-medium text-sm">
              Update
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default VendorProfile;
