/*
 * Copyright (C) 2021 Akitsugu Komiyama
 * under the MIT License
 */


using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;


namespace Installer
{
    class Program
    {
        public static void Install()
        {
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
                FileInfo fiRg = new FileInfo(System.IO.Path.Combine(basePath, relativePath));
                string rgFullPath = fiRg.FullName;

                // rg.exeがvscodeの所定の場所に存在するのか。
                if (File.Exists(rgFullPath))
                {
                    string rgFullDir = Path.GetDirectoryName(rgFullPath);
                    string rgUTF8FullPath = rgFullDir + @"\rg_utf8.exe";
                    string myProgramFullPath = Assembly.GetExecutingAssembly().Location;
                    FileInfo fiSjis = new FileInfo(myProgramFullPath);
                    if (fiRg.Length != fiSjis.Length)
                    {

                        try
                        {
                            File.Move(rgFullPath, rgUTF8FullPath);
                        }
                        catch (Exception e)
                        {

                        }
                        try
                        {
                            File.Copy(myProgramFullPath, rgFullPath, true); // 上書き保存
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("同じファイルであるため、コピー処理を停止。");
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
