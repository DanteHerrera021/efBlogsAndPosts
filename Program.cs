using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string input = "";
var db = new BloggingContext();

try
{
    do
    {
        Console.WriteLine("Enter your selection:");
        Console.WriteLine("1) Display all blogs");
        Console.WriteLine("2) Add Blog");
        Console.WriteLine("3) Create Post");
        Console.WriteLine("4) Display posts");
        Console.WriteLine("Enter q to quit");

        input = Console.ReadLine();

        logger.Info($"Option \"{input}\" selected");

        switch (input)
        {
            case "1":

                var query = db.Blogs.OrderBy(b => b.Name);

                Console.WriteLine($"{query.Count()} Blogs returned");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine();

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
