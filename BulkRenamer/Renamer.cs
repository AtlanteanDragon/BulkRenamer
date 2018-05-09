using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BulkRenamer
{
    public  class Renamer
    {
        private int counter = 0;
        private  string NewFileName(string file, string pattern)
        {
            string returnString = "";
            string fileExt = Regex.Match(file, @".+(\.[A-Za-z]+)").Groups[1].Value;

            MatchCollection regextextmatches = Regex.Matches(pattern, @"\[(n)\]|\{([a-zA-Z0-9.; _-]+)\}");
            foreach (Match item in regextextmatches)
            {
                if(item.Groups.Count != 1 && item.Groups[1].Value.ToLower() != "n")
                {
                    returnString += item.Groups[2];
                }
                else
                {
                    returnString += counter;
                    counter++;
                }
                
            }
            
            return returnString+fileExt;
        }
        public  bool RenameFiles(string pattern, string folder) {
           
            DirectoryInfo directory = new System.IO.DirectoryInfo(folder);
            FileInfo[] files = directory.GetFiles();
            bool successCheck = false;
        //    try
        //    {
                foreach (FileInfo item in files)
                {
                    File.Move(item.FullName, Path.Combine(item.DirectoryName, NewFileName(item.Name, pattern)));
                }
                successCheck = true;
           // } catch {
          //      successCheck = false;
         //   }
            return successCheck;
        }
    }
}
