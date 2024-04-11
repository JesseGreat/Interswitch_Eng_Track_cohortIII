import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { ColumnDef } from "@tanstack/react-table";

export const blacklistdata: BlacklistItem[] = [
  {
    name: "Vendor A",
    reason: "success",
    time: "ken99@yahoo.com",
  },
  {
    name: "Vendor B",
    reason: "success",
    time: "Abe45@gmail.com",
  },
  {
    name: "Vendor C",
    reason: "processing",
    time: "Monserrat44@gmail.com",
  },
  {
    name: "Vendor D",
    reason: "success",
    time: "Silas22@gmail.com",
  },
  {
    name: "Vendor E",
    reason: "failed",
    time: "carmella@hotmail.com",
  },
];

export type BlacklistItem = {
  name: string;
  reason: string;
  time: string;
};

export const blItem: BlacklistItem = {
  name: '',
  reason: '',
  time: ''
}

export const columns: ColumnDef<BlacklistItem>[] = [
  {
    id: "select",
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
      />
    ),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "name",
    header: "name",
    cell: ({ row }) => (
      <div className="">{row.getValue("name")}</div>
    ),
  },
  {
    accessorKey: "reason",
    header: () => <div className="text-right">reason</div>,
    cell: ({ row }) => {
      return <div className="text-right font-medium">{row.getValue("reason")}</div>;
    },
  },
  {
    accessorKey: "time",
    cell: ({ row }) => <div className="">{row.getValue("time")}</div>,
  }
];
