using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CmdCommand
{
    class CommandValidation
    {
        string Path = Directory.GetCurrentDirectory();
        string Command, argument = " ";
        public CommandValidation(string command)
        {
            Command = command;
        }
        public void PrintValidation() {
            bool found = false;

            //create array of commands
            string[] Commands = { "cd", "cls", "dir", "quit", "copy", "del", "help",
                                  "md", "rd", "rename", "type", "import", "export" };


            // split the string into commands and args
            string[] arguments = Command.Split(' ');
            Command = arguments[0];

            // check if the command has arguments
            if (arguments.Length > 1)
                argument = arguments[1];

            // program start
            while (true)
            {
                // if command = quit close the program
                if (Command == "quit")
                    Environment.Exit(0);

                // check what is the command
                for (int i = 0; i < Commands.Length; i++)
                {
                    if (Command == Commands[i])
                    {
                        Console.WriteLine("Valid Command");
                        if (Command == "help")
                        {
                            //create object of type help
                            Help chelp = new Help();

                            //check if command with args or not
                            if (argument != " ")
                                chelp.help(argument);
                            else
                                chelp.help();
                        }
                        else if (Command == "cls")
                            Console.Clear();
                       

                        // if the command is valid
                          found = true;
                        break;
                    }
                  
                }
               

                // if the command is invalid
                if (!found)
                    Console.WriteLine(Command + " " + "is not recognized as an internal or external command");

                // get more commands
                Console.Write("\n" + Path + ">");
                Command = Console.ReadLine();
                Command = Command.ToLower();

                //split the string into commands and args
                arguments = Command.Split(' ');
                Command = arguments[0];

                // check if the command has arguments
                if (arguments.Length > 1)
                    argument = arguments[1];

            }
        }
    
    }
}
