using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NFL_Blitz_2000_Roster_Manager.Models;
using System.Reflection;

namespace NFL_Blitz_2000_Roster_Manager.Helpers
{
    public static class GameFileManager
    {
        public static List<BlitzGame> LoadAllGameFiles()
        {
            string[] filePaths = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +"/GameFiles", "*.xml");
            List<BlitzGame> gameFiles = new List<BlitzGame>();
            foreach (string file in filePaths)
            {
                gameFiles.Add(XmlHelper.DeSerializeObject<BlitzGame>(file));
            }
            return gameFiles;
        }

    }
}
