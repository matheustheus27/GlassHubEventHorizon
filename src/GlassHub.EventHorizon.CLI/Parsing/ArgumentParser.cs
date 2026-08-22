namespace GlassHub.EventHorizon.CLI.Parsing;

public static class ArgumentParser
{
    public static CommandOptions Parse(string[] args)
    {
        var options = new CommandOptions();

        if (args.Length == 0)
            return options;

        int startIndex = 0;
        string firstArg = args[0].ToLowerInvariant();

        // Check if first argument is a command name or command flag
        switch (firstArg)
        {
            case "compress":
            case "c":
            case "--compress":
            case "-c":
                options.Command = "compress";
                startIndex = 1;
                break;

            case "extract":
            case "x":
            case "--extract":
            case "-x":
                options.Command = "extract";
                startIndex = 1;
                break;

            case "list":
            case "--list":
                options.Command = "list";
                startIndex = 1;
                break;

            case "info":
            case "--info":
                options.Command = "info";
                startIndex = 1;
                break;

            case "verify":
            case "v":
            case "--verify":
            case "-v":
                options.Command = "verify";
                startIndex = 1;
                break;

            case "help":
            case "h":
            case "--help":
            case "-h":
            case "-?":
            case "/?":
                options.Command = "help";
                startIndex = 1;
                break;

            default:
                // Not a recognized command as first arg; start option parsing from index 0
                startIndex = 0;
                break;
        }

        for (int index = startIndex; index < args.Length; index++)
        {
            string flag = args[index].ToLowerInvariant();

            switch (flag)
            {
                case "--compress":
                case "-c":
                    options.Command = "compress";
                    break;

                case "--extract":
                case "-x":
                    options.Command = "extract";
                    break;

                case "--list":
                    options.Command = "list";
                    break;

                case "--info":
                    options.Command = "info";
                    break;

                case "--verify":
                case "-v":
                    options.Command = "verify";
                    break;

                case "--help":
                case "-h":
                    options.Command = "help";
                    break;

                case "-i":
                case "--input":
                    if (index + 1 < args.Length)
                        options.InputFiles.Add(args[++index]);
                    break;

                case "-o":
                case "--output":
                    if (index + 1 < args.Length)
                        options.OutputFile = args[++index];
                    break;

                case "-f":
                case "--file":
                    if (index + 1 < args.Length)
                        options.SourceFile = args[++index];
                    break;

                case "-d":
                case "--dest":
                    if (index + 1 < args.Length)
                        options.DestinationDirectory = args[++index];
                    break;

                case "-p":
                case "--password":
                    if (index + 1 < args.Length)
                        options.Password = args[++index];
                    break;

                case "--lang":
                    if (index + 1 < args.Length)
                        options.Language = args[++index];
                    break;
            }
        }

        return options;
    }
}