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

                break;

            case "2":

                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                if (name == "" || name == null)
                {
                    logger.Error("Blog name cannot be null");
                    break;
                }

                var blog = new Blog { Name = name };

                db.AddBlog(blog);
                logger.Info($"Blog added - \"{name}\"");

                break;

            case "3":

                Console.WriteLine("Select the blog you would like to post to:");

                var availableBlogs = db.Blogs.OrderBy(b => b.Name);

                logger.Info($"There are {availableBlogs.Count()} blog(s) available");
                int index = 0;
                foreach (var item in availableBlogs)
                {
                    index++;
                    Console.WriteLine($"{index}) {item.Name}");
                }

                string blogToPost = Console.ReadLine();

                try
                {
                    int blogIndex = int.Parse(blogToPost);

                    Blog selectedBlog = availableBlogs.ElementAt(blogIndex - 1);
                }
                catch
                {
                    logger.Error("Invalid Blog ID");
                }



                break;
            case "4":
                break;
        }

        Console.WriteLine();

    } while (input is "1" or "2" or "3" or "4");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");
