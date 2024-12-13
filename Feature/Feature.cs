using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Feature
{
        public class FileAndDirectoryOperation
        {
            public class FileOperation
            {
                public static void CreateFile(string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            File.Create(file).Dispose();
                        }
                        else
                        {
                            File.Create(file).Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static void DeleteFile(string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static void Encrtypt(string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Encrypt(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static void Decrypt(string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.Decrypt(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static void Write(string str, string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            File.WriteAllText(file, str);
                        }
                        else
                        {
                            File.Create(file).Dispose();
                            File.WriteAllText(file, str);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static string Read(string file)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            return File.ReadAllText(file);
                        }
                        else
                        {
                            return "File not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "";
                    }
                }

                private static readonly List<string> Files = new List<string>();

                private static void GetFiles(string rootPath, bool getFromSubDirectories)
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(rootPath);
                        FileSystemInfo[] files = info.GetFileSystemInfos();
                        foreach (FileSystemInfo item in files)
                        {
                            if (item is FileInfo subFile)
                            {
                                Files.Add(subFile.FullName);
                            }

                            if (item is DirectoryInfo subDir)
                            {
                                if (getFromSubDirectories)
                                {
                                    GetFiles(subDir.FullName, true);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                public static List<string> ReturnGetFiles(string rootPath, bool getFromSubDirectories)
                {
                    GetFiles(rootPath, getFromSubDirectories);
                    return Files;
                }

                public static void Copy(string source, string destination)
                {
                    try
                    {
                        File.Copy(source, destination, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static void Move(string source, string destination)
                {
                    try
                    {
                        if (File.Exists(source))
                        {
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }

                            File.Move(source, destination);
                        }
                        else
                        {
                            Console.WriteLine("File not found!");
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            public class DirectoryOperation
            {
                public static void Create(string dirPath)
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                public static void Delete(string dirPath)
                {
                    try
                    {
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                private static readonly List<string> Dirs = new List<string>();
                private static void GetDirectories(string rootPath, bool getFromSubDirectories)
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(rootPath);
                        FileSystemInfo[] fileAndDirs = info.GetFileSystemInfos();
                        foreach (FileSystemInfo item in fileAndDirs)
                        {
                            if (item is DirectoryInfo subDir)
                            {
                                if (getFromSubDirectories)
                                {
                                    GetDirectories(subDir.FullName, true);
                                }

                                Dirs.Add(subDir.FullName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                public static List<string> ReturnGetDirectories(string rootPath, bool getFromSubDirectories)
                {
                    GetDirectories(rootPath, getFromSubDirectories);
                    return Dirs;
                }
            }
        }
        public class StringOperation
        {
            private static readonly string Keys = "Encrypt";
            public static string Encrypt(string str)
            {
                
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.Unicode.GetBytes(Keys);  
                byte[] data = Encoding.Unicode.GetBytes(str);
                MemoryStream mStream = new MemoryStream();     
                CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(key, key), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);    
                cStream.FlushFinalBlock();              
                return Convert.ToBase64String(mStream.ToArray());

            }
            public static string Decrypt(string str)
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();   
                byte[] key = Encoding.Unicode.GetBytes(Keys); 
                byte[] data = Convert.FromBase64String(str);
                MemoryStream mStream = new MemoryStream();      
                CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(key, key), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length); 
                cStream.FlushFinalBlock();    
                return Encoding.Unicode.GetString(mStream.ToArray());
            }
        }

        public class NetOperation
        {
            public static bool NetChecker()
            {
                string host = "example.com";
                try
                {
                    using (var ping = new Ping())
                    {
                        var reply = ping.Send(host,2000);
                        if (reply != null)
                        {
                            if (reply.Status == IPStatus.Success)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            } 
            public static async Task Downloader(string url, string path, int numberOfThreads = 8)
            {
                string fileName = Path.GetFileName(url);
                string fullPath = path + fileName;
                string directoryPath = Path.GetDirectoryName(fullPath);
                if (directoryPath != null)
                {
                    // 预先创建临时文件名
                    List<string> tempFilePaths = Enumerable.Range(0, numberOfThreads)
                        .Select(i => Path.Combine(directoryPath, $"_{fileName}_{i}.tmp"))
                        .ToList();

                    try
                    {
                        // 获取文件总大小
                        long totalBytes;
                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response =
                               await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                        {
                            totalBytes = response.Content.Headers.ContentLength ?? -1;
                        }

                        // 计算每个线程下载的字节范围
                        long chunkSize = totalBytes / numberOfThreads;
                        List<Task> downloadTasks = new List<Task>();
                        Console.Clear();
                        for (int i = 0; i < numberOfThreads; i++)
                        {
                            long startByte = i * chunkSize;
                            long endByte = (i == numberOfThreads - 1) ? totalBytes - 1 : (i + 1) * chunkSize - 1;
                            // 每个任务负责下载一部分到临时文件
                            downloadTasks.Add(DownloadChunkToTempFileAsync(url, tempFilePaths[i], startByte, endByte,
                                i + 1));
                                
                        }

                        // 等待所有下载任务完成
                        await Task.WhenAll(downloadTasks);
                        Console.SetCursorPosition(0,9);
                        await MergeTempFilesIntoFinalFile(tempFilePaths, fullPath);
                        Console.WriteLine($"Download completed.File is in {fullPath}");
                    }
                    finally
                    {
                        // 清理临时文件
                        foreach (var tempFilePath in tempFilePaths)
                        {
                            if (File.Exists(tempFilePath))
                            {
                                File.Delete(tempFilePath);
                            }
                        }
                    }
                }
            }
            private static readonly SemaphoreSlim ConsoleAccessSemaphore = new SemaphoreSlim(1, 1);

            private static async Task DownloadChunkToTempFileAsync(string url, string tempFilePath, long startByte, long endByte, int threadId)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Range = new RangeHeaderValue(startByte, endByte);

                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    using (Stream stream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        long chunkTotalBytesRead = 0;
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            chunkTotalBytesRead += bytesRead;

                            // 使用SemaphoreSlim来同步控制台输出
                            await ConsoleAccessSemaphore.WaitAsync();
                            try
                            {
                                Console.SetCursorPosition(0, threadId); // 将光标设置到对应线程的行
                                double progressPercentage = (double)chunkTotalBytesRead / (endByte-startByte + 1) * 100;
                                Console.Write($"\rThread {threadId} Download Progress: {progressPercentage:F2}%");
                            }
                            finally
                            {
                                ConsoleAccessSemaphore.Release();
                            }
                        }
                    }
                }
            }
            private static async Task MergeTempFilesIntoFinalFile(List<string> tempFilePaths, string finalFilePath)
            {
                Console.WriteLine("Merging temporary files into final file...");
                using (FileStream finalFileStream = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    foreach (var tempFilePath in tempFilePaths.OrderBy(p => p))
                    {
                        using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true))
                        {
                            await tempFileStream.CopyToAsync(finalFileStream);
                        }
                    }
                }
            }
        }

        public class ConsoleOperation
        {
            public static readonly string WelcomeMessage = ">";
            public static void Clear()
            {
                Console.Clear();            
            }
            public static void WriteLine(string str)
            {
                Console.WriteLine(str);
            }
            public static void Write(string str)
            {
                Console.Write(str);
            }
            public static string ReadLine(){
                return Console.ReadLine();
            }
            public static string Title;
            public static void SetTitle(string title)
            {
                Title = title;
                Console.Title = title;
            }

            public static void SetBackgroundColor(ConsoleColor color)
            {
                Console.BackgroundColor = color;
            }
            public static void SetForegroundColor(ConsoleColor color)
            {                
                Console.ForegroundColor = color;
            }
        }
        public static class ProcessOperation
        {
            public static void Start(string file)
            {
                try          
                {
                    Process.Start(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public static void Kill(string processName)
            {
                try    
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    foreach (Process process in processes)
                    {
                        process.Kill();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public static void AdvancedStart(string file, string arguments, bool createNoWindow, string verb, bool waitForExit,bool useShellExecute,string workingDirectory)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(file);
                startInfo.Arguments = arguments;
                startInfo.CreateNoWindow = createNoWindow;
                startInfo.Verb = verb;
                startInfo.UseShellExecute = useShellExecute;
                startInfo.WorkingDirectory= workingDirectory;
                Process pos = new Process();
                pos.StartInfo = startInfo;
                pos.Start();
                if (waitForExit) pos.WaitForExit();
                pos.Close();
            }

            public static string GetProcessById(int processId)
            {
                try
                {
                    Process process = Process.GetProcessById(processId);
                    return process.ProcessName;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
}
