
namespace EMS_Backend_Project.EMS.Common.CustomExceptions
{
    [Serializable]
    public class DataNotFoundException<T> : Exception
    {
        public DataNotFoundException() { }
        public DataNotFoundException(string message) : base(message){ }
        public DataNotFoundException(T entityName) : base($"Data not found for {entityName}.") { }

        public override string ToString()
        {
            return Message;
        }
    }
}