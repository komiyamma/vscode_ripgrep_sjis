/*
 * Copyright (C) 2021 Akitsugu Komiyama
 * under the MIT License
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;





internal static class StringEncodeExtension
{
    public static string EncodeCommandLineValue(this string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        var containsSpace = value.IndexOfAny(new[] { ' ', '\t' }) != -1;

        // 「\…\"」をエスケープ
        // やってることは、「"」直前の「\」の数を 2倍+1
        value = _commandLineEscapePattern.Replace(value, @"$1\$&");

        // スペース／タブが含まれる場合はデリミタで囲み、末尾が「\」だった場合、エスケープ
        if (containsSpace)
        {
            value = "\"" + _lastBackSlashPattern.Replace(value, "$1$1") + "\"";
        }
        return value;
    }
    private static readonly Regex _commandLineEscapePattern = new Regex("(\\\\*)\"");
    private static readonly Regex _lastBackSlashPattern = new Regex(@"(\\+)$");

    /// 
    /// コマンドライン引数複数個をエンコードして、スペースで結合
    /// 
    public static string EncodeCommandLineValues(this IEnumerable<string> values)
    {
        if (values == null) throw new ArgumentNullException("values");
        return string.Join(" ", values.Select(v => EncodeCommandLineValue(v)));
    }
}


internal class RipGrepCommandLine
{
    static Dictionary<String, bool> hit_string_dictionary = new Dictionary<String, bool>();

    Process process = new Process();

    List<String> arg_list = null;
    List<String> arg_list_head_for_sjis = new List<String> { "-E", "sjis" };

    const String rg_utf8_name = "/rg_utf8.exe";

    public RipGrepCommandLine(String[] args)
    {
        if (arg_list == null)
        {
            arg_list = new List<string>(args);
        }

        //起動するファイルを指定する
        var self = Assembly.GetExecutingAssembly().Location;
        process.StartInfo.FileName = Path.GetDirectoryName(self) + rg_utf8_name;

    }

    string MakeArgsString(Encoding enc)
    {
        if (enc == Encoding.GetEncoding(932))
        {
            // sjisオプション側を先頭にして
            arg_list_head_for_sjis.AddRange(arg_list);

            // 元のリストとする。
            arg_list = arg_list_head_for_sjis;
        }

        string arg_line = arg_list.EncodeCommandLineValues();
        System.Diagnostics.Trace.WriteLine(arg_line);
        return arg_line;
    }

    public void Grep(Encoding enc)
    {
        try
        {
            string arg_line = MakeArgsString(enc);

            process.StartInfo.Arguments = arg_line;

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

    private void proc_OutputDataReceived(object sender, DataReceivedEventArgs ev)
    {
        String data = ev.Data;
        try
        {
            if (data != null)
            {
                lock (hit_string_dictionary)
                {
                    // まだ登録されていない時だけ、出力
                    if (!hit_string_dictionary.ContainsKey(data))
                    {
                        hit_string_dictionary.Add(data, true);
                        Console.WriteLine(data);
                        dynamic document = System.Text.Json.JsonSerializer.Deserialize<System.Dynamic.ExpandoObject>(data);
                        try
                        {
                            string path = document.data.path.text;
                            int line_number = document.lines.line_number;
                            System.Diagnostics.Trace.WriteLine(path);
                            System.Diagnostics.Trace.WriteLine(line_number);
                        }
                        catch (Exception e) {

                        }
                        System.Diagnostics.Trace.WriteLine(data);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }

    }

    private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs ev)
    {
        proc_OutputDataReceived(sender, ev);
    }
}



public class RG
{
    public static void Main(String[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        RipGrepCommandLine rgcl1 = new RipGrepCommandLine(args);
        rgcl1.Grep(Encoding.UTF8);

        RipGrepCommandLine rgcl2 = new RipGrepCommandLine(args);
        rgcl2.Grep(Encoding.GetEncoding(932));
    }
}