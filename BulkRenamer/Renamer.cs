using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace BulkRenamer
{
    public  class Renamer
    {
        private int counter = 0;
        private string NewFileName(string file, string pattern, string folder)
        {
            string returnString = "";
          
            foreach (Match item in Regex.Matches(pattern, @"\[(d)\]|\[(n)\]|\{([a-zA-Z0-9.; _-]+)\}"))
            {
                dynamic argument = null;
                if (item.Groups[1].Value.ToLower().Contains("d")) { argument = GetValue(Path.Combine(folder, file), SystemProperties.System.DateModified); }
                else if (item.Groups[2].Value.ToLower().Contains("n")) { argument = counter; }
                else { argument = item.Groups[3].Value; }
                returnString += argument.ToString();
            }
            counter++;
            return returnString + Regex.Match(file, @".+(\.[A-Za-z]+)").Groups[1].Value;
        }
        private string GetValue(string filePath, PropertyKey property)
        {
            IShellProperty value = ShellFile.FromFilePath(filePath).Properties.GetProperty(property);
            return (value == null || value.ValueAsObject == null) ? String.Empty : value.ValueAsObject.ToString();
        }
        public bool RenameFiles(string pattern, string folder)
        {
            bool successCheck = false;
            try
            {
                foreach (FileInfo item in new DirectoryInfo(folder).GetFiles())
                { File.Move(item.FullName, Path.Combine(item.DirectoryName, Regex.Replace(NewFileName(item.Name, pattern, folder), "[\\/\\\\|\\:\\\"\\?\\*\\<\\>]", "-"))); }
                successCheck = true;
            } catch(Exception) {  }
            return successCheck;
        }
    }
}
