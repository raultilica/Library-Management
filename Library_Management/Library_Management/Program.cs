using Library_Management.Repository;
using Library_Management.Service;
using Library_Management.UI;
using Library_Management.Tests;

namespace Library_Management
{
    class Program
    {
        static void Main()
        {
            TestsApp.RunAllTests();
            var repo = new RepositoryFile("books.csv");
            var service = new BookService(repo);
            var ui = new UserInterface(service);
            ui.Run();
        }
    }
}