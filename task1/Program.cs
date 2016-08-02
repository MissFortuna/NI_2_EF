using System;
using ConsoleApp.SQLite;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Create and fill some data:\n");
            using (var db = new BloggingContext())
            {
                //Add Blogs
                db.Blogs.Add(new Blog { Url = "http://stackoverflow.com/", Name = "StackOverflow" });
                db.Blogs.Add(new Blog { Url = "http://google.com/", Name = "Google" });
                db.Blogs.Add(new Blog { Url = "http://facebook.com/", Name = "Facebook" });
                db.Blogs.Add(new Blog { Url = "http://twitter.com/", Name = "Twitter" });
                db.Blogs.Add(new Blog { Url = "batman", Name = "Delete" });

                //Add posts to different posts
                db.Posts.Add(new Post { BlogId = 2, Title = "Find NaUKMA staff", Content = "https://www.google.com.ua/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=naukma" });
                db.Posts.Add(new Post { BlogId = 3, Title = "Elizabeth Lemeshko", Content = "https://www.facebook.com/elizabeth.lemeshko" });
                db.Posts.Add(new Post { BlogId = 3, Title = "Olesya Lebedenko", Content = "https://www.facebook.com/olesya.lemeshko" });

                //Save data
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                //Count data
                var blogsAmount = db.Blogs.Count();
                Console.WriteLine("\nAmount of blogs - {0}", blogsAmount);

                //Print data staff
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs)
                {
                    Console.WriteLine(" -> {0} - {1} - {2}", blog.BlogId, blog.Name, blog.Url);
                }

                
                //Remove wrong
                var blogDel = db.Blogs.Single(b => b.Url == "batman");
                db.Remove(blogDel);

                //Search using 'Contains'
                var blogs = db.Blogs
                            .Where(b => b.Url.Contains("facebook"))
                            .AsNoTracking()
                            .ToList();
                Console.WriteLine("\nAmount of blogs that contains 'facebook' - {0}", blogs.Count());

                //Get all blogs with posts
                var blogswp = db.Blogs
                            .Include(blog => blog.Posts)
                            .Where(blog => blog.Posts.Count() > 0)
                            .AsNoTracking()
                            .ToList();

                Console.WriteLine("Amount of blogs with posts - {0}", blogswp.Count());

                foreach (var item in blogswp)
                {
                    Console.WriteLine("\nBlog -> {0} - {1}\nIt's posts:", item.BlogId, item.Url);
                    foreach (var post in item.Posts)
                    {
                        Console.WriteLine("Post {0} -> {1} - {2}", post.PostId, post.Title, post.Content);
                    }
                }
            }

        }
    }
}