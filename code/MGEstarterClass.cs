using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MGEXEguiStarter
{
    class MGEstarterClass
    {
        private List<string> processNamesList;
        private List<string> exeFileNeededList;
        private List<string> dllFileNeededList;
        private string curentMGEprocess;

        public MGEstarterClass()
        {
            processNamesList = new List<string>();
            processNamesList.Add("MGEXEgui");
            processNamesList.Add("MGEgui");

            exeFileNeededList = new List<string>();
            foreach (string item in processNamesList)
            {
                exeFileNeededList.Add(item + ".exe");
            }

            dllFileNeededList = new List<string>();
            dllFileNeededList.Add("dinput8.dll");
            dllFileNeededList.Add("dinput8");
        }

        private void sleepUnlilProcessIsRuning(string processName)
        {
            while (Process.GetProcessesByName(processName).Any())
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        private bool isProcessIsRuning(string processName)
        {
            return Process.GetProcessesByName(processName).Any();
        }

        private bool isFileExist(string FileName)
        {
            return File.Exists(FileName);
        }

        private void startProcess(string processName)
        {
            Process.Start(processName);
        }

        private void renamingFiles(string fileNameInput, string fileNameOutput)
        {
            File.Move(fileNameInput, fileNameOutput);
        }

        private bool checkFiles(List<string> exeFiles, List<string> dllFiles)
        {
            bool exeResult = false;
            bool dllResult = false;
            //---
            for (int i = 0; i < exeFiles.Count; i++)
            {
                if (isFileExist(exeFiles[i]))
                {
                    exeResult = true;
                    curentMGEprocess = processNamesList[i];
                    Console.WriteLine(exeFiles[i]+" was found");
                    break;
                }
            }
            if (!exeResult) { Console.WriteLine("Can't find MGEXEgui.exe or MGEgui.exe file."); }
            //---
            for (int i = 0; i < dllFiles.Count; i++)
            {
                if (isFileExist(dllFiles[i]))
                {
                    dllResult = true;
                    Console.WriteLine(dllFiles[i]+" was found");
                    break;
                }
            }
            if (!dllResult) { Console.WriteLine("Can't find dinput8.dll or dinput8 file."); }
            //---

            if (!isFileExist("Morrowind Launcher.exe")) Console.WriteLine("WARNING! There is no Morrowind Launcher.exe in folder");
            if (!isFileExist("Morrowind.exe")) Console.WriteLine("WARNING! There is no Morrowind.exe in folder");


            return (!exeResult || !dllResult) ? false : true;
            
        }

        public void run()
        {
            if (!checkFiles(exeFileNeededList, dllFileNeededList))
            {
                Console.ReadKey();
                return;
            }

            if (!isProcessIsRuning(curentMGEprocess))
	        {
                if (isFileExist("dinput8"))
                {
                    renamingFiles("dinput8", "dinput8.dll");
                    Console.WriteLine("Renaming dinput8 to dinput8.dll");
                }
                startProcess(curentMGEprocess);
                Console.WriteLine("Starting " + curentMGEprocess);
                Console.WriteLine("Wating until it'll be over");
                sleepUnlilProcessIsRuning(curentMGEprocess);

                if (isFileExist("dinput8"))
                {
                    return;
                }

                if (!isFileExist("dinput8.dll")) 
                {
                    Console.WriteLine("While " + curentMGEprocess + "was runing, file dinput8.dll was chenged or deleted");
                    Console.ReadKey();
                    return;
                }
               
                renamingFiles("dinput8.dll", "dinput8");
                Console.WriteLine("Renaming dinput8.dll to dinput8");
            }
            else
            {
                Console.WriteLine(curentMGEprocess+" already running! \nPress any key to exit.");
                Console.ReadKey();
            }

        }
    }
}
