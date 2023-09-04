# Language Design AST 🚀

## 📖 Project Description

The "Language Design AST" project is a simple turing complete interpreter written in C#.

The job of interpreter or compiler is its ability to understand and transform source code. This project delves deep into this process, shedding light on two vital phases:

1. **Lexical Analysis**: The raw, textual source code is broken down into meaningful chunks called tokens. These tokens could be keywords, operators, or other language-specific constructs. The project provides a unique opportunity to visually witness this transformation.
2. **Parsing**: The tokenized code undergoes a structural transformation to form an Abstract Syntax Tree (AST). The AST is a tree representation capturing the syntactic hierarchy of the source code, paving the way for subsequent processing phases in typical compilers.

What's unique about "Language Design AST" is its interactive nature:

- Users can visually observe the tokenization of source code.
- Delve deep into the AST, appreciating the intricate relationships and structures that the source code hides within.

## 🛠️ Build the Project

To build the project, follow these steps:

1. Open the project in Visual Studio Code.
2. Click on the Terminal tab in VS Code and open a new Terminal.
3. Once the Terminal has loaded, run the following command:

```bash
dotnet build
```

4. 🎉 The project will be built into an executable. Once the build process is finished, the file path to the executable will be displayed.

##  🏃 Open and Run the Project

To open and run the project, follow these steps:

1. Open the project in Visual Studio Code.
2. Click on the Terminal tab in VS Code and open a new Terminal.
3. Once the Terminal has loaded, run the following command:

```bash
dotnet run
```

4. After running the command, you will be presented with two exciting options:

```
✨ Select an option:
1. Lex file and display results 
2. Parse file and explore the magic 
Enter your choice:
```

- If you choose option 1, the tokenizer will create tokens from the provided code, giving you a visual representation of the results.
- If you choose option 2, you will dive into the abstract syntax tree (AST) of the code, exploring the intricate beauty of language design. 🌳

## 📝 Changing Test Files

To change the test file used in the program, follow these steps:

1. Open the `Main.cs` file in the project.
2. Locate the fifth line in the file.
3. Within the quotation marks (`""`), you will find the name of the current file (e.g., `"test1.txt"`).
4. Replace the current file name with the desired test file name.

Always remember to save the changes after modifying the test file.
