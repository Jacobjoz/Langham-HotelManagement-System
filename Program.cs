using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assessment2Task2
{
    // Custom Class - Room
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    // Custom Class - Customer
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    // Custom Class - RoomAllocation
    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    // Custom Main Class - Program
    class Program
    {
        // Variables declaration and initialization
        public static List<Room> listOfRooms = new List<Room>();
        public static List<RoomAllocation> listOfRoomAllocations = new List<RoomAllocation>();
        public static string filePath;

        // Main function
        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, "lhms_studentid.txt");

            char ans= Convert.ToChar("A");

            do
            {
                Console.Clear();
                Console.WriteLine("***********************************************************************************");
                Console.WriteLine("                 LANGHAM HOTEL MANAGEMENT SYSTEM                  ");
                Console.WriteLine("                            MENU                                 ");
                Console.WriteLine("***********************************************************************************");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save the Room Allocations To a File");
                Console.WriteLine("8. Show the Room Allocations From a File");
                Console.WriteLine("9. Exit");
                Console.WriteLine("0. Backup");
                Console.WriteLine("***********************************************************************************");
                Console.Write("Enter Your Choice Number Here:");
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter a number between 0 and 9.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddRooms();
                        break;
                    case 2:
                        DisplayRooms();
                        break;
                    case 3:
                        AllocateRoom();
                        break;
                    case 4:
                        DeAllocateRoom();
                        break;
                    case 5:
                        DisplayRoomAllocations();
                        break;
                    case 6:
                        Console.WriteLine("Billing Feature is Under Construction and will be added soon…!!!");
                        break;
                    case 7:
                        SaveRoomAllocationsToFile();
                        break;
                    case 8:
                        ShowRoomAllocationsFromFile();
                        break;
                    case 9:
                        return;
                    case 0:
                        BackupRoomAllocations();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 0 and 9.");
                        break;
                }

                Console.Write("\nWould You Like To Continue(Y/N):");
                ans = Convert.ToChar(Console.ReadLine());
            } while (ans == 'y' || ans == 'Y');
        }

        static void AddRooms()
        {
            Console.Write("Please Enter the Total Number of Rooms in the Hotel: ");
            int numberOfRooms = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < numberOfRooms; i++)
            {
                Console.Write("Please enter the Room Number: ");
                try
                {
                    int roomNo = Convert.ToInt32(Console.ReadLine());
                    listOfRooms.Add(new Room { RoomNo = roomNo, IsAllocated = false });
                }
                catch
                {
                    Console.WriteLine("Invalid Format");
                    i--;
                }
              
            }

            Console.WriteLine("Rooms added successfully.");
        }

        static void DisplayRooms()
        {
            Console.WriteLine("List of Rooms:");
            foreach (var room in listOfRooms)
            {
                Console.WriteLine($"Room No: {room.RoomNo}, Is Allocated: {room.IsAllocated}");
            }
        }

        static void AllocateRoom()
        {
            Console.Write("Please Enter Room Number: ");
            try
            {
                int roomNo = Convert.ToInt32(Console.ReadLine());
                var room = listOfRooms.FirstOrDefault(r => r.RoomNo == roomNo);

                if (room == null)
                {
                    Console.WriteLine("Room not found.");
                    return;
                }

                if (room.IsAllocated)
                {
                    Console.WriteLine("Room is already allocated.");
                    return;
                }

                Console.Write("Please Enter Customer Number: ");
                int customerNo = Convert.ToInt32(Console.ReadLine());
                Console.Write("Please Enter Customer Name: ");
                string customerName = Console.ReadLine();

                var customer = new Customer { CustomerNo = customerNo, CustomerName = customerName };
                listOfRoomAllocations.Add(new RoomAllocation { AllocatedRoomNo = roomNo, AllocatedCustomer = customer });
                room.IsAllocated = true;

                Console.WriteLine("Room allocated successfully.");
            }
            catch
            {
                Console.WriteLine("Invalid Format");
            }
          
        }

        static void DeAllocateRoom()
        {
            Console.Write("Please Enter Room Number: ");
            try
            {
                int roomNo = Convert.ToInt32(Console.ReadLine());
                var room = listOfRooms.FirstOrDefault(r => r.RoomNo == roomNo);

                if (room == null)
                {
                    Console.WriteLine("Room not found.");
                    return;
                }

                if (!room.IsAllocated)
                {
                    Console.WriteLine("Room is not allocated.");
                    return;
                }

                var allocation = listOfRoomAllocations.FirstOrDefault(a => a.AllocatedRoomNo == roomNo);
                if (allocation != null)
                {
                    listOfRoomAllocations.Remove(allocation);
                    room.IsAllocated = false;
                    Console.WriteLine("Room de-allocated successfully.");
                }
            }catch
            {
                Console.WriteLine("Invalid Format");
            }
          
        }

        static void DisplayRoomAllocations()
        {
            Console.WriteLine("Room Allocations:");
            foreach (var allocation in listOfRoomAllocations)
            {
                Console.WriteLine($"Room No: {allocation.AllocatedRoomNo}, Customer No: {allocation.AllocatedCustomer.CustomerNo}, Customer Name: {allocation.AllocatedCustomer.CustomerName}");
            }
        }

        static void SaveRoomAllocationsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Room Allocations as of {DateTime.Now}:");
                    foreach (var allocation in listOfRoomAllocations)
                    {
                        writer.WriteLine($"Room No: {allocation.AllocatedRoomNo}, Customer No: {allocation.AllocatedCustomer.CustomerNo}, Customer Name: {allocation.AllocatedCustomer.CustomerName}");
                    }
                }

                Console.WriteLine("Room allocations saved to file successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: Unauthorized access. You do not have permission to write to this file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving to the file: {ex.Message}");
            }
        }

        static void ShowRoomAllocationsFromFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }

        static void BackupRoomAllocations()
        {
            string backupFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "lhms_studentid_backup.txt");

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    using (StreamWriter writer = new StreamWriter(backupFilePath, true))
                    {
                        writer.WriteLine($"Backup as of {DateTime.Now}:");
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine("Backup created and original file content cleared successfully.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File not found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: Unauthorized access. You do not have permission to write to this file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during backup: {ex.Message}");
            }
        }
    }
}
