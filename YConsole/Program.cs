using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Feature;
namespace YConsole
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            else
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Verb = "runas";
                try
                {
                    System.Diagnostics.Process.Start(startInfo);
                    Environment.Exit(0);
                }
                catch
                {
                    return;
                }
            }
            MainAsync().GetAwaiter().GetResult();
        }
        static Dictionary<string,string> _vars=new Dictionary<string,string>();
        private static async Task MainAsync()
        {
            ConsoleOperation.SetTitle("YConsole Pro Edition v1.0");
            while (true)
            {
                ConsoleOperation.Write(ConsoleOperation.WelcomeMessage);
                string command = ConsoleOperation.ReadLine().ToLower();
                switch (command)
                {
                    case "output":
                        ConsoleOperation.WriteLine("Please type your string:");
                        ConsoleOperation.WriteLine(ConsoleOperation.ReadLine());
                        break;
                    case "guid":
                        ConsoleOperation.WriteLine(Guid.NewGuid().ToString());
                        break;
                    case "random":
                        ConsoleOperation.WriteLine("Please type your generate number:");
                        string num = ConsoleOperation.ReadLine();
                        if (int.TryParse(num, out int numInt))
                        {
                            Random random = new Random();
                            for (int i = 0; i < numInt; i++)
                            {
                                ConsoleOperation.Write(random.Next(100000000) + " ");
                            }
                            ConsoleOperation.WriteLine("");
                        }
                        else
                        {
                            ConsoleOperation.WriteLine("Invalid number.");
                        }
                        break;
                    case "drawer":
                        ConsoleOperation.WriteLine("Please type your number:");
                        int.TryParse(ConsoleOperation.ReadLine(), out var number);
                        Random ran = new Random();
                        for (int i = 0; i < 100; i++)
                        {
                            await Task.Delay(200);
                            ConsoleOperation.Write("\r                   ");
                            ConsoleOperation.Write("\r"+ran.Next(1,number));
                        }
                        ConsoleOperation.WriteLine("");
                        break;
                    case "kill":
                        ConsoleOperation.WriteLine("Please type tour process name:");
                        string process = ConsoleOperation.ReadLine();
                        ProcessOperation.Kill(process);
                        break;
                    case "dir":
                        ConsoleOperation.WriteLine("Please type your directory path:");
                        string dirPath = ConsoleOperation.ReadLine();
                        if (Directory.Exists(dirPath))
                        {
                            foreach (var item in FileAndDirectoryOperation.FileOperation.ReturnGetFiles(dirPath, true))
                            {
                                ConsoleOperation.WriteLine(item);
                            }

                            foreach (var item in FileAndDirectoryOperation.DirectoryOperation.ReturnGetDirectories(dirPath, true))
                            {
                                ConsoleOperation.WriteLine(item);
                            }
                        }            
                        else
                        {                
                            ConsoleOperation.WriteLine("Directory does not exist.");
                        }
                        break;
                    case "set":
                        ConsoleOperation.WriteLine("Please type your variable name:");
                        string varName = ConsoleOperation.ReadLine();
                        ConsoleOperation.WriteLine("Please type your variable value:");
                        string varValue = ConsoleOperation.ReadLine();
                        _vars.Add(varName,varValue);  
                        _vars[varName] = varValue;
                        break;
                    case "net-checker":
                        ConsoleOperation.WriteLine("Checking network status...");
                        await Task.Delay(1000);
                        if (NetOperation.NetChecker())
                        {
                            ConsoleOperation.WriteLine("Network is available.");
                        }
                        else
                        {
                            ConsoleOperation.WriteLine("Network is not available.");
                        }
                        break;
                    case "download":
                        ConsoleOperation.WriteLine("Please type your url:");
                        string url = ConsoleOperation.ReadLine();
                        ConsoleOperation.WriteLine("Please type your save path:");
                        string path = ConsoleOperation.ReadLine();
                        await NetOperation.Downloader(url, path);
                        break;
                    case "exit":
                        await Task.Delay(1000);
                        ConsoleOperation.WriteLine("Exiting...");
                        return;
                    default:
                        foreach (var item in _vars.Keys)
                        {
                            if (string.Equals(command, item))
                            {
                                ConsoleOperation.WriteLine(_vars[item]);
                            }
                            else
                            {
                                ConsoleOperation.WriteLine("Invalid command.");
                            }
                        }
                        break;
                }
            }
        }
    }
}