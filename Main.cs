public class Program
{
    public static void Main(string[] args)
    {
        string filePath = "hello2.txt";
            
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The file does not exist.");
            return;
        }

        Console.WriteLine("Select option: \n1. Lex file and display results\n2. Parse file");
        var userInput = Console.ReadLine();
        var code = File.ReadAllText(filePath); 

        switch(userInput)
        {
            case "1":
                var lexer = new Lexer(code);
                var allTokens = lexer.GetAllTokens(); 

                Console.WriteLine("Lexed tokens: ");
                foreach (var token in allTokens)
                {
                    Console.WriteLine(token); 
                }
                break;
            case "2":
                var parser = new Parser(new Lexer(code).GetAllTokens());
                var interpreter = new Interpreter(parser);

                var result = interpreter.Interpret();
                // If the result is not null and not an empty string, print it
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
