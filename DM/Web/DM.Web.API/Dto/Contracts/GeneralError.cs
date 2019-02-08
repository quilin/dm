namespace DM.Web.API.Dto.Contracts
{
    public class GeneralError
    {
        public GeneralError(string message)
        {
            Message = message;
        }
        
        public string Message { get; }
    }
}