using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace YConsole
{
    internal class Program
    {
        static readonly string e_d_key = "ekey";
        static readonly string w_message = ">";
        static bool logoned = false;
        static void Main()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                Console.Title = "YConsole";
            Start:
                Console.Write(w_message);
                string reader = Console.ReadLine().ToLower();
                switch (reader)
                {
                    case "output":
                        Console.WriteLine("Please input your string:");
                        string output = Console.ReadLine();
                        Console.WriteLine(output);
                        goto Start;
                    case "mkdir":
                        Console.WriteLine("Please enter your path:");
                        string reader_2 = Console.ReadLine();
                        mkdir(reader_2);
                        break;
                    case "cc":
                        cc();
                        break;
                    case "rmdir":
                        Console.WriteLine("Please enter your path:");
                        string reader_3 = Console.ReadLine();
                        rmdir(reader_3);
                        break;
                    case "download":
                        Console.WriteLine("Please input your URI:");
                        string URI = Console.ReadLine();
                        download(URI);
                        break;
                    case "bgc":
                        Console.WriteLine("Please choose your color ID:");
                        Console.WriteLine("Green|White|Red|Yellow or Reset");
                        string ID = Console.ReadLine();
                        bgc(ID);
                        break;
                    case "%random%":
                        Console.WriteLine("Please enter your count:");
                        string reader_5 = Console.ReadLine();
                        int count = Convert.ToInt32(reader_5);
                        random(count);
                        break;
                    case "zip":
                        if (!Directory.Exists(@".\zippath"))
                        {
                            Directory.CreateDirectory(@".\zippath");
                        }
                        Console.WriteLine(@"Please put files to .\zippath\,and input your output path.");
                        string outputpath = Console.ReadLine();
                        zip(@".\zippath\", outputpath);
                        break;
                    case "view":
                        Console.WriteLine("Please input your path:");
                        string reader_8 = Console.ReadLine();
                        view(reader_8);
                        break;
                    case "runas":
                        Console.WriteLine("Please enter your .exe file path:");
                        string reader_4 = Console.ReadLine();
                        if (File.Exists(@reader_4) & Path.GetExtension(reader_4) == ".exe")
                        {
                            RunAsAdministrator(reader_4);
                        }
                        break;
                    case "del":
                        Console.WriteLine("Please enter your file path:");
                        string reader_6 = Console.ReadLine();
                        del(reader_6);
                        break;
                    case "create":
                        Console.WriteLine("Please enter your file path:");
                        string reader_7 = Console.ReadLine();
                        create(reader_7);
                        break;
                    case "logon":
                        if (logoned == false)
                        {
                            logon();
                        }
                        else
                        {
                            Console.WriteLine("You logoned!");
                            goto Start;
                        }
                        break;
                    case "bs":
                    choose:
                        Console.WriteLine("Do you want to do this?(Y/N)");
                        string choose = Console.ReadLine();
                        if (choose == "Y" || choose == "y")
                        {
                            Console.WriteLine("Really?(Y/N)");
                            string choose_2 = Console.ReadLine();
                            if (choose_2 == "Y" || choose_2 == "y")
                            {
                                bs();
                            }
                            else if (choose_2 == "N" || choose_2 == "n")
                            {
                                Main();
                            }
                            else
                            {
                                Console.WriteLine("Error!");
                                goto choose;
                            }
                        }
                        else if (choose == "N" || choose == "n")
                        {
                            Main();
                        }
                        else
                        {
                            Console.WriteLine("Error!");
                            goto choose;
                        }
                        break;
                    case "start":
                        Console.WriteLine("Please input your file path:");
                        string fp= Console.ReadLine();
                        Console.WriteLine("Please input your args:");
                        string args = Console.ReadLine();
                        choose2:
                        Console.WriteLine("Do you want to create window?");
                        string choose2 = Console.ReadLine();
                        if (choose2 == "Y" || choose2 == "y")
                        {
                            bool cnw = false;
                            start(fp, args, cnw);
                        }
                        else if (choose2 == "N" || choose2 == "n")
                        {
                            bool cnw = true;
                            start(fp, args, cnw);
                        }
                        else
                        {
                            Console.WriteLine("Error!");
                            goto choose2;
                        }
                        break;
                    case "cheaknet":
                        bool scss = cheaknet();
                        if(scss == false)
                        {
                            Console.WriteLine("You're not connected to the internet!");
                            goto Start;
                        }
                        else
                        {
                            Console.WriteLine("You're connected to the internet");
                            goto Start;
                        }
                    case "guid":
                        Console.WriteLine(guid());
                        goto Start;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error!");
                        goto Start;
                }
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
                }
                catch
                {
                    return;
                }
            }         
        }
        static void mkdir(string path)
        {
            try
            {
                if (!Directory.Exists(@path))
                {
                    Directory.CreateDirectory(@path);
                    Main();
                }
                else
                {
                    choose:
                    Console.WriteLine("The directory exists, do you want to override it?(Y/N)");
                    string choose = Console.ReadLine();
                    if (choose == "Y" || choose == "y")
                    {
                        Directory.Delete(@path, true);
                        Directory.CreateDirectory(@path);
                        Main();
                    }
                    else if (choose == "N" || choose == "n")
                    {
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Error!");
                        goto choose;
                    }
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void cc()
        {
            Console.Clear();
            Main();
        }
        static void rmdir(string path)
        {
            try
            {
                if (Directory.Exists(@path))
                {
                    Directory.Delete(@path, true);
                    Main();
                }
                else
                {
                    Console.WriteLine("The directory not exists!");
                    Main();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void download(string URI)
        {
            try
            {
                if (!Directory.Exists(@".\downloads"))
                {
                    Directory.CreateDirectory(@".\downloads");
                }
                string path = @".\downloads\";
                var URI2 = new Uri(URI);
                string filename = Path.GetFileName(URI);
                if (File.Exists(filename)) { 
                    filename = "(2)"+filename;
                }
                WebClient client = new WebClient();
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileAsync(URI2,path+filename);
                while (true)
                {
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("\nDownload completed!");
            Main();
        }

        static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\r" + e.ProgressPercentage + "%" + "\r");
        }
        static void zip(string source, string destination)
        {
            try
            {
                if (!Directory.Exists(source))
                {
                    Directory.CreateDirectory(source);
                }
                ZipFile.CreateFromDirectory(source,destination,CompressionLevel.Optimal,false);
                Console.WriteLine("File is in "+ destination + ".");
                Main();
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message) ;
                Main();
            }
        }
        static void RunAsAdministrator(string filepath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = filepath,
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(startInfo);
                Main();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }

        }
        static void bgc(string ID)
        {
            switch (ID.ToLower())
            {
                case "red":Console.BackgroundColor = ConsoleColor.Red; Console.Clear();Main(); break;
                case "green": Console.BackgroundColor = ConsoleColor.Green; Console.Clear(); Main(); break;
                case "yellow": Console.BackgroundColor = ConsoleColor.Yellow; Console.Clear(); Main(); break;
                case "white": Console.BackgroundColor = ConsoleColor.White; Console.Clear(); Main(); break;
                case "reset": Console.ResetColor(); Console.Clear();Main();break;
                default: Console.WriteLine("Error ID!");Main();break;
            }
        }
        static void random(int count)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                if(count > 100000)
                {
                    Console.WriteLine("Error!");
                    Main(); 
                }
                Console.Write(random.Next(1000000) + " ");
            }
            Console.Write("\n");
            Main();
        }
        static void del(string filepath)
        {
            try
            {
                if (File.Exists(@filepath))
                {
                    File.Delete(@filepath);
                    Main();
                }
                else
                {
                    Console.WriteLine("The file not exists,or we don't have file permissions.");
                    Main();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void create(string filepath)
        {
            try
            {
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                    Main();
                }
                else
                {
                    choose:
                    Console.WriteLine("The file exists, do you want to override it?(Y/N)");
                    string choose = Console.ReadLine();
                    if (choose == "Y" || choose == "y")
                    {
                        File.Delete(@filepath);
                        File.Create(@filepath).Dispose();
                        Main();
                    }
                    else if (choose == "N" || choose == "n")
                    {
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Error!");
                        goto choose;
                    }
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void view(string path)
        {
            try
            {
                if (Directory.Exists(@path))
                {
                    choose:
                    Console.WriteLine("Do you want to find subfolders?(Y/N)");
                    string choose = Console.ReadLine();
                    if (choose == "Y" || choose == "y")
                    {
                        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                        string[] dirs = Directory.GetDirectories(path,"*",SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            Console.WriteLine(file);
                        }
                        Console.WriteLine("Dirs:");
                        foreach (string dir in dirs)
                        {
                            Console.WriteLine(dir);
                        }
                        Main();
                    }
                    else if (choose == "N" || choose == "n")
                    {
                        string[] files = Directory.GetFiles(path, "*.*");
                        string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            Console.WriteLine(file);
                        }
                        Console.WriteLine("Dirs:");
                        foreach (string dir in dirs)
                        {
                            Console.WriteLine(dir);
                        }
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Error!");
                        goto choose;
                    }
                }
                else
                {
                    Console.WriteLine("Path not found!");
                    Main();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static string Encrypt(string str)
        {

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            byte[] key = Encoding.Unicode.GetBytes(e_d_key);  
            byte[] data = Encoding.Unicode.GetBytes(str);
            MemoryStream MStream = new MemoryStream();     
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);    
            CStream.FlushFinalBlock();              
            return Convert.ToBase64String(MStream.ToArray());

        }
        static string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   
            byte[] key = Encoding.Unicode.GetBytes(e_d_key); 
            byte[] data = Convert.FromBase64String(str);
            MemoryStream MStream = new MemoryStream();      
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length); 
            CStream.FlushFinalBlock();    
            return Encoding.Unicode.GetString(MStream.ToArray());
        }
        static void logon()
        {
            int err_of_logon = 0;
            try
            {
                if (!File.Exists("pwd"))
                {
                    File.Create("pwd").Dispose();
                }
                if (logoned == false)
                {
                    string read = File.ReadAllText("pwd");
                    if (read == "") {
                        Console.WriteLine("Please input your password:");
                        string ps2 = Encrypt(Console.ReadLine());
                        File.WriteAllText("pwd",ps2);
                        Console.Clear();
                    }
                    input:
                    Console.WriteLine("Password:");
                    string ps = Console.ReadLine();
                    string ps3 = Decrypt(File.ReadAllText("pwd"));
                    if (ps == ps3)
                    {
                        Console.Clear();
                        Console.WriteLine("Password is right.");
                        logoned = true;
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Password isn't right!");
                        if(err_of_logon == 3)
                        {
                            Environment.Exit(0);
                        }
                        err_of_logon = ++err_of_logon;
                        goto input;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void bs()
        {
            try
            {
                Process[] process = Process.GetProcessesByName("svchost");
                foreach (Process p in process)
                {
                    p.Kill();
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static void start(string filepath,string args,bool cnw)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = filepath;
                startInfo.Arguments = args;
                startInfo.CreateNoWindow = cnw;
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
                Main();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }
        }
        static bool cheaknet()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingStatus =
                ping.Send(IPAddress.Parse("20.70.246.20"), 3000);
                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Net.NetworkInformation.PingException) { 
                return false;
            }
        }
        static string guid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
