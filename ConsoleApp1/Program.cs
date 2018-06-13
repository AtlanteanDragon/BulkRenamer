using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyUpdater
{
    class Program
    {
        internal static string remoteUrl = "";
        internal static Process[] ToUpdate;
        internal static string processName;

        static void Main(string[] args)
        {
            //TODO: make application update once every so often, compare last time of modification to version.txts
            if (args.Count() > 0)
                processName = args[0];
            else
                processName = "HelloPrintServer.exe";
            UpdateProcess();
        }
        private static void UpdateProcess()
        {
            StopProcess();
            DownloadVersionFile();
            var files = ReadFiles();
            DownloadAndReplaceFile(files);
            StartProcess();
        }
        private static void StartProcess()
        {
            try {
                //ProcessStartInfo start = new ProcessStartInfo()
                //{
                //    //FileName = processName,
                //    //WindowStyle = ProcessWindowStyle.Normal,
                //    //CreateNoWindow = false
                //};
                //Process process = Process.Start(start);
                ToUpdate.FirstOrDefault().Start();
            }
            catch(Exception e)
            {
                FuckupLogger(e);
            }
        }
        private static void StopProcess()
        {
            try
            {
                ToUpdate = Process.GetProcessesByName(processName);
                ToUpdate.FirstOrDefault().CloseMainWindow();
                
            }
            catch (Exception e) { FuckupLogger(e); }
        }
        private static void DownloadAndReplaceFile(List<string> fileList)
        {

            WebClient client = new WebClient();
            foreach (string fileName in fileList)
            {
                string webPath = remoteUrl + fileName;
                client.DownloadFile(webPath, fileName);
            }

        }
        private static void DownloadVersionFile()
        {
            try { 
            WebClient client = new WebClient();
                string fileName = "version.txt";
            string webPath = remoteUrl + fileName;
            client.DownloadFile(webPath, fileName);
            }
            catch (Exception e) { FuckupLogger(e); }


        }
        private static List<string> ReadFiles()
        {
            List<string> fileList = new List<string>();
            using (StreamReader reader = new StreamReader("version.txt"))
            {
                while (!reader.EndOfStream)
                {
                    fileList.Add(reader.ReadLine());
                }
            }
            return fileList;
        }
        #region FuckupLogger( Exception or string)
        private static void FuckupLogger(dynamic input)
        {
            FuckupLogger(input, DateTime.Now);
        }
        private static void FuckupLogger(string input, DateTime dateTimeLogger)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"\EasyUpdaterLog.txt");

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(dateTimeLogger.Year.ToString() + "-" +
                    dateTimeLogger.Month.ToString() + "-" + dateTimeLogger.Day.ToString() + " " +
                    dateTimeLogger.Hour + ":" + dateTimeLogger.Minute + " => " + input
                    );
            }
        }
        private static void FuckupLogger(Exception input, DateTime dateTimeLogger)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"\EasyUpdaterLog.txt");

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(dateTimeLogger.Year.ToString() + "-" +
                    dateTimeLogger.Month.ToString() + "-" + dateTimeLogger.Day.ToString() + " " +
                    dateTimeLogger.Hour + ":" + dateTimeLogger.Minute + " => " + input.ToString()
                    );
            }
        }
        #endregion
    }
}
