using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.GameFiles
{
    class FingerPrint
    {
        public FingerPrint(string filePath)
        {
            files = new List<GameFile>();
            string fpstring = null;

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    fpstring = sr.ReadToEnd();
                }
                LoadFromJson(fpstring);
                Console.WriteLine("ObjectManager: fingerprint loaded");
            }
            else
            {
                Console.WriteLine("LoadFingerPrint: error! tried to load FingerPrint without file, run gen_patch first");
            }
        }

        public List<GameFile> files { get; set; }
        public string sha { get; set; }
        public string version { get; set; }

        public void LoadFromJson(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);

            JArray jsonFilesArray = (JArray)jsonObject["files"];
            foreach (JObject jsonFile in jsonFilesArray)
            {
                GameFile gf = new GameFile();
                gf.Load(jsonFile);
                files.Add(gf);
            }
            sha = jsonObject["sha"].ToObject<string>();
            version = jsonObject["version"].ToObject<string>();       
        }

        public string SaveToJson()
        {
            JObject jsonData = new JObject();

            JArray jsonFilesArray = new JArray();
            foreach (var file in files)
            {
                JObject jsonObject = new JObject();
                file.SaveToJson(jsonObject);
                jsonFilesArray.Add(jsonObject);
            }
            jsonData.Add("files", jsonFilesArray);
            jsonData.Add("sha", sha);
            jsonData.Add("version", version);

            return JsonConvert.SerializeObject(jsonData).Replace("/", @"\/");
        }
    }

    class GameFile
    {
        public GameFile() { }
        public String sha { get; set; }
        public String file { get; set; }

        public void Load(JObject jsonObject)
        {
            sha = jsonObject["sha"].ToObject<string>();
            file = jsonObject["file"].ToObject<string>();
        }

        public string SaveToJson(JObject fingerPrint)
        {
            fingerPrint.Add("sha", sha);
            fingerPrint.Add("file", file);

            return JsonConvert.SerializeObject(fingerPrint);
        }
    }
}
