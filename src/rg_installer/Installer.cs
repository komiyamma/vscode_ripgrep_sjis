using System;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace rg_installer
{
    class Installer
    {
        static void Main(string[] args)
        {
            const string command = "cmd /k where node";

            try
            {

                Process process = new Process();

                process.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                process.StartInfo.Arguments = "/c where code.cmd";

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                //イベントハンドラの追加
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

                process.ErrorDataReceived += proc_ErrorDataReceived;
                process.OutputDataReceived += proc_OutputDataReceived;


                //起動する
                process.Start();
                process.BeginOutputReadLine();

                process.WaitForExit();

                try
                {
                    if (process != null)
                    {
                        process.Close();
                        process.Kill();
                    }
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private static void proc_OutputDataReceived(object sender, DataReceivedEventArgs ev)
        {
            string line = ev.Data;
           if (File.Exists(line))
            {
                string basePath = Path.GetDirectoryName(line);
                string relativePath = @"..\resources\app\node_modules.asar.unpacked\vscode-ripgrep\bin\rg.exe";
                System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(basePath, relativePath));
                string rgFullPath = fi.FullName;
                if (File.Exists(rgFullPath))
                {
                    string rgFullDir = Path.GetDirectoryName(rgFullPath);
                    string rgUTF8FullPath = rgFullDir + @"\rg_utf8e.exe";
                    try
                    {
                        File.Move(rgFullPath, rgUTF8FullPath);
                    } catch(Exception e)
                    {

                    }
                    string myProgramFullPath = Assembly.GetExecutingAssembly().Location;
                    try
                    {
                        File.Copy(myProgramFullPath, rgFullPath);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

        }

        private static void proc_ErrorDataReceived(object sender, DataReceivedEventArgs ev)
        {
            proc_OutputDataReceived(sender, ev);
        }
    }
}
