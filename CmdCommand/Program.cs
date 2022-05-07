using System;
using System.IO;
using System.Collections.Generic;

namespace CmdCommand
{
    class Program
    {
        
      static  void Main(string[] args)
        {
            // print the current directory
           
            string path = Directory.GetCurrentDirectory();

            // create variable to initiate the path of file
            string FilePath = @"D:\Virtual disk.ppt";
            // create object to pass the path to the method
            VirtualDisk vir = new VirtualDisk(FilePath);
            
            // method to write on the file
           
      
             //   vir.Initialize(FilePath);
               
            

            // create and print fat table to check that it's working
            //FatTable ft = new FatTable();
            //ft.write_fattable();
            //ft.print_fattable();


            Console.Write(path + ">");
            // read command from user
            string Command;
            Command = Console.ReadLine();
            Command = Command.ToLower();
            CommandValidation com = new CommandValidation(Command);
            com.PrintValidation();


            
            //get available block 
            //int index = ft.get_available_block();
            //byte[] data = new byte[4096];
            //vir.WriteBlock(data, index);
           
        }
    }
}
