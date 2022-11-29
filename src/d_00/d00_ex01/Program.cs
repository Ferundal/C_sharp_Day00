using System;
using System.IO;
using System.Reflection;
using System.Resources;

string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"dictionary");
if (!File.Exists(path))
{
    Console.WriteLine("Can't found file");
    return (-1);
}
string[] names = File.ReadAllLines(path);
foreach (var name in names)
{
    Console.WriteLine(name);
}