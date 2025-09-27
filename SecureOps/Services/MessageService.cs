using MudBlazor;

namespace SecureOps.Services
{
    public class MessageService
    {
        public string? PendingMessage { get; set; }
        public Severity PendingSeverity { get; set; }
    }
}
