namespace DM.Services.Mail.Sender;

/// <summary>
/// DTO model for sending email
/// </summary>
public class MailLetter
{
    /// <summary>
    /// Email address to send letter to
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Letter subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Letter body
    /// </summary>
    public string Body { get; set; }
}