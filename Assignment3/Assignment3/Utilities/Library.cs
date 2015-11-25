using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    public class Library<T>
    {
        public Dictionary<String, T> Contents = new Dictionary<String, T>();

        public T this[string key]
        {
            get{ return Contents[key]; }
        }

        public void InitModelLibrary(ContentManager _content, string _filePath)
        {
        }

        public T Get(String key)
        {
            return Contents[key];
        }

        public void LoadContent(ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name.Remove(file.Name.Length - 4));

                Contents.Add(key,contentManager.Load<T>(contentFolder + "//" + key));
            }
        }
        
    }
}
