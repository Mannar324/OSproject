using System;
using System.Collections.Generic;
using System.Text;

namespace CmdCommand
{
   public class DirectoryEntry
    {
      public  char[] file_name = new char[11];
      public  byte[] file_empty = new byte[12]; // group of zeros
       public byte file_attribute; // 0x0 for files, or 0x10 for folders

       

        public int file_size;
        public int first_cluster;

        public DirectoryEntry(char[] name, byte attr, int cluster)
        {
            // initialize file name, attribute, first cluster
           this. file_name = name;
            this.file_attribute = attr;
            this.first_cluster = cluster;
        }
    
        public byte[] get_bytes(char[] name, byte[] file_empty, byte attr, int size, int first_cluster)
        {
            // convert attributes to byte array of size 32
            // byte array to store data
            byte[] data = new byte[32];

            // convert file name to bytes and store it
            

            // convert file size to bytes and store it
            byte[] byte_file_size = new byte[4];
            byte_file_size = BitConverter.GetBytes(size);

            // convert first cluster to bytes and store it
            byte[] byte_first_cluster = new byte[4];
            byte_first_cluster = BitConverter.GetBytes(first_cluster);

            // return data in bytes
            return data;
        }

        public DirectoryEntry get_directory_entry(byte[] data)
        {
            // convert array of bytes to directory entry

            // convert file name from bytes to char
            char[] name = new char[11];

            // set file empty and attribute
            byte attr=0;
            byte[] f_empty = new byte[12];

            // convert file size from bytes to int
            int size=0;

            // converst first cluster from bytes to int
            int cluster=0;

            // object to store data and return directory entry
            DirectoryEntry d1 = new DirectoryEntry(name, attr, cluster);
            d1.file_size = size;
            d1.file_empty = file_empty;
            return d1;
        }
    }
}
