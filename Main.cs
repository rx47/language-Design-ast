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

        Console.WriteLine("Select option: \n1. Lex file and display results\n2. Parse file");
        var userInput = Console.ReadLine();

        var code = File.ReadAllText(filePath); // Reads all lines at once

        switch(userInput)
        {
            case "1":
                var lexer = new Lexer(code);
                var allTokens = lexer.GetAllTokens(); 

                Console.WriteLine("Lexed tokens: ");
                foreach (var token in allTokens)
                {
                    Console.WriteLine(token); // modify this line to display the token the way you want
                }
                break;
            case "2":
                var parser = new Parser(new Lexer(code).GetAllTokens());
                var interpreter = new Interpreter(parser);

                var result = interpreter.Interpret();
                if (result != null && result?.ToString() != "")
                {
                    Console.WriteLine(result);
                }
                break;
            default:
                Console.WriteLine("Invalid option");
                break;
        }
    }
}
