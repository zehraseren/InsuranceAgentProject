using InsureYouAI.Enums;

namespace InsureYouAI.Dtos.MessageDtos;

public class ResultMessageDto
{
    public int MessageId { get; set; }
    public string NameSurname { get; set; }
    public string Subject { get; set; }
    public string Email { get; set; }
    public string MessageDetail { get; set; }
    public string? AICategory { get; set; }
    public MessagePriority Priority { get; set; }
    public DateTime SendDate { get; set; }
    public bool IsRead { get; set; }
}
