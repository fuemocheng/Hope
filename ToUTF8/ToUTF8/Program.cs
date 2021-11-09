using System;
using System.IO;
using System.Text;

namespace ToUTF8
{
    class Program
    {
        static void Main(string[] args)
        {
            //string dir = Directory.GetCurrentDirectory();
            //string dir = "D:\\GitHub\\Test\\ToUTF8\\ToUTF8\\bin\\Debug\\net5.0";
            string dir = "D:\\GitHub\\Test";

            foreach (var f in new DirectoryInfo(dir).GetFiles("*.cs", SearchOption.AllDirectories))
            {
                var s = File.ReadAllText(f.FullName, Encoding.Default);
                try
                {
                    File.WriteAllText(f.FullName, s, Encoding.UTF8);
                    Console.WriteLine(f.Name);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            Console.ReadLine();
        }
    }
}
