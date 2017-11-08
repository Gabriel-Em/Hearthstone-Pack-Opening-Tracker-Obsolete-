using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PackOpeningTracker.Model
{
    public class SettingsData
    {
        private string HSDirectoryPath, configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Blizzard\Hearthstone\log.config";
        
        public SettingsData()
        {
            if (File.Exists(@".\Data\Path.dat"))
            {
                try
                {
                    HSDirectoryPath = File.ReadAllText(@".\Data\Path.dat");
                }
                catch
                {
                    HSDirectoryPath = string.Empty;
                }
            }
            else
                HSDirectoryPath = string.Empty;

            checkIfPatched();
        }

        public string HSPath
        {
            get
            {
                return HSDirectoryPath;
            }

            set
            {
                HSDirectoryPath = value;
                setHSPath(value);
            }
        }

        private void setHSPath(string path)
        {
            StreamWriter file = new StreamWriter(@".\Data\Path.dat");
            file.Write(path);
            file.Close();
        }

        public bool checkIfPatched()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    string file = File.ReadAllText(configPath);
                    string[] lines = file.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Contains("[Achievements]"))
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(ex.ToString());
                file.Close();
                return false;
            }
        }
    }
}
