using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

const int maxLevenshteinDistance = 1;

string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"dictionary");
if (!File.Exists(path))
{
    Console.WriteLine("Can't found file");
    return;
}
string[] names = File.ReadAllLines(path);
Console.WriteLine("Enter name:");
string input = Console.ReadLine();
if (!input.All(c => Char.IsLetter(c) || c == ' ' || c == '-'))
{
    Console.Error.WriteLine("Something went wrong. Check your input and retry.");
}


if (input.Length < 1)
{
    NameNotFound();
    return;
}
int minLevenshteinDistance = maxLevenshteinDistance + 1;
int minLevenshteinDistanceIndex = 0;
for (int index = 0; index < names.Length; ++index)
{
    var levenshteinDistance = LevenshteinDistance(input, names[index]);
    if (levenshteinDistance == 0)
    {
        SayHello(input);
        return;
    }

    if (levenshteinDistance < minLevenshteinDistance)
    {
        minLevenshteinDistance = levenshteinDistance;
        minLevenshteinDistanceIndex = index;
    }
}

if (minLevenshteinDistance > maxLevenshteinDistance)
{
    NameNotFound();
    return;
}

if (IsName(names[minLevenshteinDistanceIndex]))
{
    SayHello(input);
    return;
}
for (int index = minLevenshteinDistanceIndex + 1; index < names.Length; ++index)
{
    var levenshteinDistance = LevenshteinDistance(input, names[index]);
    if (levenshteinDistance == minLevenshteinDistance && IsName(names[index]))
    {
        SayHello(input);
        return;
    }
}
NameNotFound();
return;

bool IsName(string name)
{
    Console.WriteLine($"Did you mean “{name}”? Y/N");
    string readLine;
    do
    {
        readLine = Console.ReadLine();
    } while (readLine != "y" && readLine != "Y" && readLine != "n" && readLine != "N");

    if (readLine == "y" || readLine == "Y")
    {
        return true;
    }
    
    return false;
}

void NameNotFound()
{
    Console.WriteLine($"Your name was not found.");
}

void SayHello(string name)
{
    Console.WriteLine($"Hello, {name}!");
}

int LevenshteinDistance(string str1, string str2)
{
    int [,] levenshteinDistances = new int [str2.Length + 1, str1.Length + 1];
    for (int index = 0; index < levenshteinDistances.GetLength(0); ++index)
    {
        levenshteinDistances[index, 0] = index;
    }
    for (int index = 0; index < levenshteinDistances.GetLength(1); ++index)
    {
        levenshteinDistances[0, index] = index;
    }
    for (int i = 1; i < levenshteinDistances.GetLength(0); ++i)
    {
        for (int j = 1; j < levenshteinDistances.GetLength(1); ++j)
        {
            if (str2[i - 1] == str1[j - 1])
            {
                levenshteinDistances[i, j] = levenshteinDistances[i - 1, j - 1];
            }
            else
            {
                levenshteinDistances[i, j] = MinThree(
                    levenshteinDistances[i - 1, j - 1],
                    levenshteinDistances[i - 1, j],
                    levenshteinDistances[i, j - 1]) + 1;
            }
        }
    }
    return levenshteinDistances[levenshteinDistances.GetLength(0) - 1, levenshteinDistances.GetLength(1) - 1];

    int MinThree(int first, int second, int third)
    {
        int min = first;
        if (min > second)
        {
            min = second;
        }
        if (min > third)
        {
            min = third;
        }
        return min;
    }
}