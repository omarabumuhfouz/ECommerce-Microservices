namespace FeedbackService.Domain.Exceptions
{
    public class EntityFoundException : Exception
    {
        public EntityFoundException(int id) : base($"Feed back Not Found with Id : {id}")
        {

        }
    }
}
