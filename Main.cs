using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string filePath = "hello.txt";
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The file 'hello.txt' does not exist.");
            return;
        }

        var lines = File.ReadAllLines(filePath);
        
        foreach (var line in lines)
        {
            //Console.WriteLine($"calc> {line}");
            var lexer = new Lexer(line);
            var parser = new Parser(lexer);
            var interpreter = new Interpreter(parser);
            var result = interpreter.Interpret();
            Console.WriteLine(result);
        }
    }
}
