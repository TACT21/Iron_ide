using Microsoft.Graph;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace ide.Components.FileAgent
{
    /*Dispose class*/
    public static class FileAgent
    {
        private static List<FileCapsule> capsules { set; get; } = new List<FileCapsule>() ;
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
            var contentArray = new byte[file.File.Length];
            file.File.Read(contentArray, 0, contentArray.Length);
            Content = file.Encoding.GetString(contentArray);
        }

        public async Task ValueSetAsync(FileCapsule file, bool dispose = true)
        {
            var contentArray = new byte[file.File.Length];
            var task = file.File.ReadAsync(contentArray, 0, contentArray.Length);
            Name = file.Name;
            MetaType = file.MetaType;
            Path = file.Path;
            Encoding = file.Encoding.CodePage;
            await task;
            Content = file.Encoding.GetString(contentArray);
            if (dispose)
            {
                file.File.Dispose();
            }
        }

        public async Task ValueSetAsync(byte[] content, string path, string? name = default, Encoding? encoding = null)
        {
            encoding = encoding != null? encoding : System.Text.Encoding.UTF8;
            Name = name;
            MetaType = "";
            Path = path;
            Encoding = encoding.CodePage;
            Content = encoding.GetString(content);
        }
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public async Task<string> SerializeAsync()
        {
            string result = string.Empty;
            using(MemoryStream ms = new())
            {
                await JsonSerializer.SerializeAsync(ms, this);
                var contentArray = new byte[ms.Length];
                await ms.ReadAsync(contentArray, 0, contentArray.Length);
                result  = UTF8Encoding.UTF8.GetString(contentArray);
                System.Console.WriteLine(result + "@FileAgent.cs SerializeAsync");
            }
            return result;
        }

        public void Deserialize(string target)
        {
            JsonSerializer.Deserialize<FileCapsuleSerializeAgent>(target);
        }

        public async Task DeserializeAsync(string target)
        {
            target = target != null ? target : String.Empty;
            if(target != String.Empty)
            {
                try
                {
                    System.Console.WriteLine(target + "@DeserializeAsync");
                    using (MemoryStream ms = new())
                    {
                        await ms.WriteAsync(UTF8Encoding.UTF8.GetBytes(target));
                        await JsonSerializer.DeserializeAsync<FileCapsuleSerializeAgent>(ms);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    this.ValueSet(new FileCapsule());
                }
            }
            else
            {
                this.ValueSet(new FileCapsule());
            }
        }
    }

    public class FileCapsule
    {
        public string Name { get; set; }
        public string MetaType { get; set; }
        public FileStream File { get; set; }
        public Encoding Encoding { get; set; }
        public string Path { get; set; }
        public FileCapsule()
        {
            Name = String.Empty;
            MetaType = String.Empty;
            Encoding = Encoding.UTF8;
            Path = String.Empty;
            File = new FileStream(Guid.NewGuid().ToString(),FileMode.CreateNew);
        }
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
