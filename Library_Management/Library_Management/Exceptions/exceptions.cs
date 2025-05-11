using System;

namespace Library_Management.Exceptions
{
    // Generic exception for domain errors.
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
    
    // Generic exception for service errors.
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
    }
    
    // Generic exception for repository errors.
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
    }
}