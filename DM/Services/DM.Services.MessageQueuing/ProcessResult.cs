namespace DM.Services.MessageQueuing
{
    public enum ProcessResult
    {
        Success = 0,
        RetryNeeded = 1,
        Fail = 2
    }
}