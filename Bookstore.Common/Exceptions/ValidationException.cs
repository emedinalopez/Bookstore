namespace Bookstore.Common.Exceptions
{
    public class ValidationException : Exception
    {        
        public IDictionary<string, string[]> Errors { get; }
     
        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }
        
        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }
        
        public ValidationException(string propertyName, string errorMessage)
            : base("Validation failed.")
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }
    }
}
