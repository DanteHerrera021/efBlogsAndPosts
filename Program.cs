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

                var blogsList = db.Blogs.OrderBy(b => b.Name).ToList();

                Console.WriteLine("Select the blog you would like to post to:");

                logger.Info($"There are {blogsList.Count()} blog(s) available");
                int index = 0;
                foreach (var item in blogsList)
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

                logger.Info($"User selected {blogsList[blogIndex - 1].Name}");

                Blog selectedBlog;
                try
                {
                    selectedBlog = blogsList.ToList().ElementAt(blogIndex - 1);
                }
                catch
                {
                    logger.Error("There are no Blogs saved with that ID");
                    break;
                }
                selectedBlog = blogsList[blogIndex - 1];

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

                var listOfBlogs = db.Blogs.OrderBy(b => b.Name).ToList();

                Console.WriteLine("Select the blog's posts to display:");
                Console.WriteLine("0) Posts from all blogs");

                int bIndex = 1;
                foreach (Blog b in listOfBlogs)
                {
                    Console.WriteLine($"{bIndex}) {b.Name}");
                    bIndex++;
                }

                string postsFromBlogs = Console.ReadLine();

                Blog selectedB;
                int selectedBlogIndex;
                try
                {
                    if (postsFromBlogs != "0")
                    {
                        selectedBlogIndex = int.Parse(postsFromBlogs);
                        selectedB = listOfBlogs[selectedBlogIndex - 1];
                    }
                }
                catch
                {
                    logger.Error("Invalid choice");
                    break;
                }

                selectedBlogIndex = int.Parse(postsFromBlogs);

                int displayedBlogs = 0;

                if (selectedBlogIndex == 0)
                {
                    foreach (Post p in db.Posts)
                    {
                        p.Display();
                        displayedBlogs++;
                    }
                }
                else
                {
                    selectedB = listOfBlogs[selectedBlogIndex - 1];

                    foreach (Post p in db.Posts)
                    {
                        if (p.BlogId == selectedBlogIndex - 1)
                        {
                            p.Display();
                            displayedBlogs++;
                        }
                    }
                }

                logger.Info($"{displayedBlogs} Blogs returned");

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
