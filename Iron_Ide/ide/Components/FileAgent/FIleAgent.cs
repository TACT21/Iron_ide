using Microsoft.Graph;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace ide.Components.FileAgent
{
    public static class FileAgent
    {
        private static List<FileCapsule> capsules { private set; get; } = new List<FileCapsule>() ;
        public static async Task AppendFile(FileCapsule file)
        {
            capsules.Add(file);
        }

        public async static Task<List<string>> GetPaths()
        {
            List<string> paths = new List<string>();
            foreach (var capsule in capsules)
            {
                paths.Add(capsule.Path);
            }
            return paths;
        }

        public async static Task<Dictionary<string,string>> GetPathDic()
        {
            Dictionary<string, string> paths = new ();
            foreach (var capsule in capsules)
            {
                paths.Add(capsule.Name,capsule.Path);
            }
            return paths;
        }
    }

    static class FileAgentSettings { 
        
    }

    public class FileCapsuleSerializeAgent
    {
        public string Name { get; private set; }
        public string MetaType { get; private set; }
        public string Content { get; private set; }
        public int Encoding { get; private set; }
        public string Path { get; private set; }
        public void ValueSet(FileCapsule file)
        {
            Name = file.Name;
            MetaType = file.MetaType;
            Path = file.Path;
            Encoding = file.Encoding.CodePage;
            Content = file.File
        }
    }

    public class FileCapsule
    {
        public string Name { get; set; }
        public string MetaType { get; set; }
        public FileStream File { get; set; }
        public Encoding Encoding { get; set; }
        public string Path { get; set; }

        public FileCapsule(Stream stream,string path, bool isDispose = true,string? name = default,Encoding? encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var content = new byte[stream.Length];
            stream.Read(content, 0, content.Length);
            this.File = new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            File.Write(content);
            this.Encoding = encoding;
            this.Path = path;
            Name = name ?? string.Empty;            
        }
    }
}
