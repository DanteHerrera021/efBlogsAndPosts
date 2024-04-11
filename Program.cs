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
                string name = Console.ReadLine();

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

                var availableBlogs = db.Blogs.OrderBy(b => b.BlogId);

                logger.Info($"There are {availableBlogs.Count()} blog(s) available");
                int index = 0;
                foreach (var item in availableBlogs)
                {
                    index++;
                    Console.WriteLine($"{index}) {item.Name}");
                }

                string blogToPost = Console.ReadLine();

                int blogIndex;
                try
                {
                    blogIndex = int.Parse(blogToPost);
                }
                catch
                {
                    logger.Error("Invalid Blog ID");
                    break;
                }
                blogIndex = int.Parse(blogToPost);

                Blog selectedBlog;
                try
                {
                    selectedBlog = availableBlogs.ElementAt(blogIndex - 1);
                }
                catch
                {
                    logger.Error("There are no Blogs saved with that ID");
                    break;
                }
                selectedBlog = availableBlogs.ElementAt(blogIndex - 1);

                // START CREATING POST

                Console.WriteLine("Enter the post's title");
                string title = Console.ReadLine();

                if (title == "" || title == null)
                {
                    logger.Error("Post title cannot be null");
                    break;
                }

                Console.WriteLine("Enter the post's content");

                string content = Console.ReadLine();

                Post post = new Post
                {
                    Title = title,
                    Content = content,
                    BlogId = blogIndex - 1,
                    Blog = selectedBlog
                };

                db.AddPost(post);

                logger.Info($"Post added = \"{title}\"");

                break;
            case "4":

                Console.WriteLine("Select the blog's posts to display:");
                Console.WriteLine("0) Posts from all blogs");

                int bIndex = 1;
                foreach (Blog b in db.Blogs)
                {
                    Console.WriteLine($"{bIndex}) {b.Name}");
                    bIndex++;
                }

                string postsFromBlogs = Console.ReadLine();

                if (int.Parse(postsFromBlogs) == 0)
                {
                    foreach (Post p in db.Posts)
                    {
                        p.Display();
                    }
                }


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
