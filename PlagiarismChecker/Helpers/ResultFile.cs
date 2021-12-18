using Copyleaks.SDK.V3.API.Models.Callbacks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlagiarismChecker.Helpers
{
    public class ResultFile
    {
        public static string GetResultDirectory() => $"{Path.GetTempPath()}\\CopyleaksSdkDemo";

        public static string GetResultsFilePath(string scanId) => $"{GetResultDirectory()}\\{scanId}.json";

        public static void CreateResultDirectory() => Directory.CreateDirectory(GetResultDirectory());

        private static bool HasResults(string scanId) => File.Exists(GetResultsFilePath(scanId));

        public static CompletedCallback GetResults(string scanId)
        {
            if (HasResults(scanId))
            {
                string path = GetResultsFilePath(scanId);
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<CompletedCallback>(json);
            }
            else
                return null;
        }

        //public static void DeleteFileIfExists(string scanId)
        //{
        //    string path = GetResultsFilePath(scanId);
        //    if (File.Exists(path))
        //        File.Delete(path);
        //}

        public static void SaveResults(CompletedCallback completedCallback, string scanId)
        {
            string json = JsonConvert.SerializeObject(completedCallback);
            string resultFilePath = GetResultsFilePath(scanId);

            File.WriteAllText(resultFilePath, json);
        }
    }
}
