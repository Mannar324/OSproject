using System;
using System.Collections.Generic;
using System.Text;

namespace CmdCommand
{
    public class Directoriy :DirectoryEntry
    {
       public List<DirectoryEntry> directorytable;
       public Directoriy parent;
      

      public  Directoriy(char []name, byte attr, int cluster,Directoriy parent) : base(name, attr,cluster)
        {
            directorytable = new List<DirectoryEntry>();
            this.parent = parent;
        }

        

        public void WriteDirectory( List <DirectoryEntry>directorytable)
        {  // method to write directory table in virtual disk
            if (directorytable.Count == 0) { 
            //declare array of bytes to store directory table
            byte[] Directory_table_bytes = new byte[32 * directorytable.Count];
            //declare array of bytes to store directory entry
            byte[] Directory_entry_bytes = new byte[32];
            for(int i = 0; i < directorytable.Count; i++)
            {
                //read all bytes from the specific dir entry
                Directory_entry_bytes = directorytable[i].get_bytes(directorytable[i].file_name,directorytable[i].file_empty,directorytable[i].file_attribute,directorytable[i].file_size,directorytable[i].first_cluster);
               //write bytes of data entry in dirctory table bytes 
               for(int j = i*32,c=0;c<32 ; j++,c++)
                {
                    Directory_table_bytes[j] = Directory_entry_bytes[c];
                }
                    
            }
            // get numbers of block to store directory table

            double number_of_required_block= (Directory_table_bytes.Length / 1024); 
            number_of_required_block = Math.Ceiling(number_of_required_block);
            //get number of full bolcks
          decimal number_of_full_size_block= Directory_table_bytes.Length /1024;
            number_of_full_size_block = Math.Floor(number_of_full_size_block);
            //get number of reminder blocks
            int number_of_reminder_block = Directory_table_bytes.Length % 1024;
            FatTable f1 = new FatTable();
               //create list of array of bytes to store the blocks
              List<byte[]> ls = new List<byte[]>();
                if (number_of_required_block <= f1.get_available_blocks())
                {


                    byte[] newarr = new byte[1024];
                    int j = 0, c;
                    for (int i = 0; i < Directory_table_bytes.Length; i++)
                    {
                        for (j = i * 1024, c = 0; c < 1024; j++, c++)
                        {
                            newarr[c] = Directory_table_bytes[j];
                        }
                        ls[i] = (newarr);
                    }

                    int fatindex, lastindex = -1;
                    //check if firstcluster is reserved
                    if (first_cluster != 0)
                    {
                        fatindex = first_cluster;
                    }
                    else
                    {   //get available block and store it in fatindex
                        fatindex = f1.get_available_block();
                        first_cluster = fatindex;
                    }
                    string FilePath = @"D:\Virtual disk.txt ";
                    VirtualDisk v1 = new VirtualDisk(FilePath);
                    int count = 0;
                    for (int i = 0; i < number_of_full_size_block; i++)
                    {
                        v1.WriteBlock(ls[i], fatindex);
                        //count number of index wrote it
                        count++;
                        f1.set_next(fatindex, -1);
                        if (lastindex != -1)
                        {
                            f1.set_next(lastindex, fatindex);
                            lastindex = fatindex;
                            fatindex = f1.get_available_block();
                            
                        }
                        

                    }
                    for (int i = count; i < number_of_reminder_block; i++)
                    {
                        v1.WriteBlock(ls[i], fatindex);
                        f1.set_next(fatindex, -1);
                        if (lastindex != -1)
                        {
                            f1.set_next(lastindex, fatindex);
                            lastindex = fatindex;
                            fatindex = f1.get_available_block();
                        }

                    }

                    f1.write_fattable();
                }
                }
        }

        

        public void ReadDirectory()
        {
            List<DirectoryEntry> directorytable2 = new List<DirectoryEntry>();
            List<byte> ls = new List<byte>();
            FatTable f1 = new FatTable();
            string FilePath = @"D:\Virtual disk.txt ";
            VirtualDisk v1 = new VirtualDisk(FilePath);
            int fatindex, next;
            if (first_cluster != 0)
            {
                fatindex = first_cluster;
               next=f1.get_next(fatindex);

                do
                {
                    //read and write the block in list
                   ls.AddRange( v1.ReadBlock(fatindex));
                    //update fatindex 
                    fatindex = next;
                    if(fatindex!=-1)
                      next = f1.get_next(fatindex);
                }while (fatindex!=-1);
                //add every 32 byte of array in directory table2
               // int co = 0;
                for(int i = 0; i < ls.Count;  i++)
                { 
                  byte[] data = new byte[32];
                  for(int j =0; j < 33; j++)
                    {
                        data[j] = ls[i*32];
                    }
                 //convert bytes to directory entry 
                   directorytable2.Add(get_directory_entry(data));
                }

            }


        }
      public int SearchDirectory(string filename )
        {
            ReadDirectory();
            for(int i = 0; i < directorytable.Count; i++)
            {
                if (filename.Equals(directorytable[i].file_name))
                {
                    return i;
                }

            }
            return -1;
        }
        //method to update the content of dirtable
        public void UpDatecontent(DirectoryEntry d)
        {
            ReadDirectory();
            //convert char[] to string
            string filename = new string(d.file_name);
            //search if file exist 
            int index = SearchDirectory(filename);
            if (index != -1)
            {
                //delete the old directory entry and write the new
                directorytable.RemoveAt(index);
                directorytable.Insert(index, d);
            }


        }
        public void DeleteDirectory()
        {
            if (first_cluster != 0)
            {
                int index = first_cluster;
                FatTable f = new FatTable();
                int next = f.get_next(index);
                do
                {
                    f.set_next(index, 0);
                    index = next;
                    if (index != -1)
                       next = f.get_next(index);
                }
                while (index != -1);
                if (parent != null)
                {
                    parent.ReadDirectory();

                    string filename = new string(file_name);
                    int i = parent.SearchDirectory(filename);
                    if (i != -1)
                    {
                        parent.directorytable.RemoveAt(i);
                        parent.WriteDirectory(directorytable);

                    }
                    f.write_fattable();
                }    
                
            }
        }
     
    }
}
