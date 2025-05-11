using System;
using System.Collections.Generic;
using Library_Management.Domain;
using Library_Management.Exceptions;
using Library_Management.Repository;
using Library_Management.Service;
using Library_Management.Validator;
using Xunit;

namespace Library_Management.Tests
{
    public class TestsApp
    {
        private RepositoryFile repo;
        private BookService service;

        public TestsApp()
        {
            repo = new RepositoryFile("tests.csv");
            service = new BookService(repo);
        }

        // Book tests
        [Fact] public void Ctor()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 5);
            Assert.Equal(5, b.Quantity);
            Assert.Equal(5, b.InitialQuantity);
        }
        [Fact] public void Inc()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            b.DecreaseStock(2);
            Assert.Equal(1, b.Quantity);
            b.IncreaseStock(1);
            Assert.Equal(2, b.Quantity);
        }
        [Fact] public void IncInvalid()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            try { b.IncreaseStock(0); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid amount for stock increase.", ex.Message); }
        }
        [Fact] public void IncExceed()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            try { b.IncreaseStock(3); Assert.True(false); }
            catch (DomainException ex) { Assert.Contains("exceed the initial stock", ex.Message); }
        }
        [Fact] public void Dec()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            b.DecreaseStock(2);
            Assert.Equal(1, b.Quantity);
        }
        [Fact] public void DecInvalid()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            try { b.DecreaseStock(0); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid amount for stock decrease.", ex.Message); }
        }
        [Fact] public void DecInsuf()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 3);
            try { b.DecreaseStock(5); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Not enough stock available for borrowing.", ex.Message); }
        }

        // Validator tests
        [Fact] public void ValOk()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", 1);
            BookValidator.ValidateBook(b);
        }
        [Fact] public void ValTitle()
        {
            var b = new Book(Guid.NewGuid(), " ", "A", 1);
            try { BookValidator.ValidateBook(b); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid title.", ex.Message); }
        }
        [Fact] public void ValAuthor()
        {
            var b = new Book(Guid.NewGuid(), "T", null, 1);
            try { BookValidator.ValidateBook(b); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid author.", ex.Message); }
        }
        [Fact] public void ValQty()
        {
            var b = new Book(Guid.NewGuid(), "T", "A", -1);
            try { BookValidator.ValidateBook(b); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid cantity.", ex.Message); }
        }

        // Repo tests
        [Fact] public void RepoAdd()
        {
            var b = new Book(Guid.NewGuid(), "X", "Y", 2);
            var a = repo.Add(b);
            Assert.Equal(b.Id, a.Id);
        }
        [Fact] public void RepoAddDup()
        {
            var b = new Book(Guid.NewGuid(), "X", "Y", 2);
            repo.Add(b);
            try { repo.Add(b); Assert.True(false); }
            catch (RepositoryException ex) { Assert.Equal("Book already exists.", ex.Message); }
        }
        [Fact] public void RepoGet()
        {
            var b = new Book(Guid.NewGuid(), "X", "Y", 2);
            repo.Add(b);
            Assert.Equal(b.Title, repo.Get(b.Id).Title);
        }
        [Fact] public void RepoGetNF()
        {
            try { repo.Get(Guid.NewGuid()); Assert.True(false); }
            catch (RepositoryException ex) { Assert.Equal("Book not found.", ex.Message); }
        }
        [Fact] public void RepoGetAll()
        {
            var l = repo.GetAll();
            Assert.IsType<List<Book>>(l);
        }
        [Fact] public void RepoFind()
        {
            var b1 = new Book(Guid.NewGuid(), "A1", "B1", 1);
            var b2 = new Book(Guid.NewGuid(), "A2", "B2", 1);
            repo.Add(b1); repo.Add(b2);
            Assert.Single(repo.Find("A1", null));
        }
        [Fact] public void RepoDel()
        {
            var b = new Book(Guid.NewGuid(), "X", "Y", 2);
            repo.Add(b);
            repo.Delete(b.Id);
            try { repo.Get(b.Id); Assert.True(false); }
            catch (RepositoryException) { }
        }

        // Service tests
        [Fact] public void SvcAdd()
        {
            var id = Guid.NewGuid();
            service.AddBook(id, "T", "A", 3);
            Assert.Equal(id, service.GetBook(id).Id);
        }
        [Fact] public void SvcGetAll()
        {
            Assert.NotNull(service.GetAllBooks());
        }
        [Fact] public void SvcGetNF()
        {
            try { service.GetBook(Guid.NewGuid()); Assert.True(false); }
            catch (RepositoryException ex) { Assert.Equal("Book not found.", ex.Message); }
        }
        [Fact] public void SvcBorrow()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "T", "A", 4);
            service.BorrowBook(id, 2);
            Assert.Equal(2, service.GetBook(id).Quantity);
        }
        [Fact] public void SvcBorrowInv()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "T", "A", 4);
            try { service.BorrowBook(id, 0); Assert.True(false); }
            catch (DomainException ex) { Assert.Equal("Invalid amount for stock decrease.", ex.Message); }
        }
        [Fact] public void SvcReturn()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "T", "A", 4);
            service.BorrowBook(id, 2); service.ReturnBook(id, 1);
            Assert.Equal(3, service.GetBook(id).Quantity);
        }
        [Fact] public void SvcReturnInv()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "T", "A", 4);
            try { service.ReturnBook(id, 10); Assert.True(false); }
            catch (DomainException ex) { Assert.Contains("exceed the initial stock", ex.Message); }
        }
        [Fact] public void SvcDel()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "T", "A", 1);
            service.DeleteBook(id);
            try { service.GetBook(id); Assert.True(false); }
            catch (RepositoryException) { }
        }
        [Fact] public void SvcSearch()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "SearchMe", "A", 1);
            Assert.Single(service.SearchBooks("SearchMe", null));
        }
        [Fact] public void SvcUpdate()
        {
            var id = Guid.NewGuid(); service.AddBook(id, "Old", "A", 1);
            service.UpdateBook(id, "New", "B", 1);
            Assert.Equal("New", service.GetBook(id).Title);
        }
        [Fact] public void SvcTop()
        {
            var id1 = Guid.NewGuid(); var id2 = Guid.NewGuid();
            service.AddBook(id1, "A", "A", 5);
            service.AddBook(id2, "B", "B", 5);
            service.BorrowBook(id2, 3);
            var t = service.GetMostBorrowedBooks(1);
            Assert.Single(t);
            Assert.Equal(id2, t[0].Id);
        }
        
        public static void RunAllTests()
        {
            var t = new TestsApp();
            t.Ctor(); t.Inc(); t.IncInvalid(); t.IncExceed();
            t.Dec(); t.DecInvalid(); t.DecInsuf();
            t.ValOk(); t.ValTitle(); t.ValAuthor(); t.ValQty();
            t.RepoAdd(); t.RepoAddDup(); t.RepoGet(); t.RepoGetNF(); t.RepoGetAll(); t.RepoFind(); t.RepoDel();
            t.SvcAdd(); t.SvcGetAll(); t.SvcGetNF(); t.SvcBorrow(); t.SvcBorrowInv();
            t.SvcReturn(); t.SvcReturnInv(); t.SvcDel(); t.SvcSearch(); t.SvcUpdate(); t.SvcTop();
            Console.WriteLine("All tests executed.");
            using (var fs = System.IO.File.Create(Path.Combine(Directory.GetCurrentDirectory(), "tests.csv"))) { }
        }
    }
}
