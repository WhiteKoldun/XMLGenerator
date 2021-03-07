using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLGenerator
{
    public class FileRepository
    {
        public FileRepository()
        {
            FileRepo = new FileRepository();
        }

        public FileRepository FileRepo { get; set; }
        public async Task<byte[]> GetFile(string path, string filename)
        {
            var pathAndFile = Path.Combine(path, filename);
            var bytes = File.ReadAllBytes(pathAndFile);
            return bytes;
        }

        public async Task<string[]> GetListOfXMLFiles(string path)
        {
            var listOfFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(s => (Path.GetExtension(s).ToLower() == ".xml")).ToArray();
            return listOfFiles;
        }
    }
}
