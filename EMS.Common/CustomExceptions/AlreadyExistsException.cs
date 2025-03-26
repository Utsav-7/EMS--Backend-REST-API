namespace EMS_Backend_Project.EMS.Common.CustomExceptions
{
    public class AlreadyExistsException<T> : Exception
    {
        public AlreadyExistsException() { }
        public AlreadyExistsException(string? message) : base(message) { }
        public AlreadyExistsException(T entityName) : base($"Data Already exists for {entityName}.") { }

    }
}