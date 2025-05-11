using System;
using Library_Management.Domain;
using Library_Management.Exceptions;

namespace Library_Management.Validator
{
    public class BookValidator
    {
        public static void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new DomainException("Invalid title.");
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new DomainException("Invalid author.");
            if (book.Quantity < 0)
                throw new DomainException("Invalid cantity.");
        }
    }
}