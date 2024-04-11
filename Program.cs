using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string input = "";

try
{
    do
    {
        Console.Write("Enter your selection:");
        Console.Write("1) Display all blogs");
        Console.Write("2) Add Blog");
        Console.Write("3) Create Post");
        Console.Write("4) Display posts");
        Console.Write("Enter q to quit");

        input = Console.ReadLine();

        logger.Info($"Option \"{input}\" selected");

        switch (input)
        {
            case "1":



                break;

            case "2":



                break;

            case "3":
                break;
            case "4":
                break;
        }


    } while (input is "1" or "2" or "3" or "4");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");
