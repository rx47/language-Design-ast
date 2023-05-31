public class Program
{
    public static void Main(string[] args)
    {
        string filePath = "hello1.txt";
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The file does not exist.");
            return;
        }

        var lines = File.ReadAllLines(filePath);
        
        // Create interpreter instance outside of the loop
        Interpreter? interpreter = null;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            var lexer = new Lexer(line);
            var parser = new Parser(lexer);
            // Reuse the same interpreter for each line
            if (interpreter == null)
            {
                interpreter = new Interpreter(parser);
            }
            else
            {
                interpreter.Parser = parser;
            }
            var result = interpreter.Interpret();
            if (result == null)
            {
                continue;
            }
            Console.WriteLine(result);
        }
    }
}
