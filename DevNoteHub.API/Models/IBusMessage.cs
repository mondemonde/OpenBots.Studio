namespace DevNoteHub.Models
{
    public interface IBusMessage
    {
        string Content { get; set; }
        string DomainName { get; set; }
        int GlobalMachineId { get; set; }
        bool IsResponse { get; set; }
        string Label { get; set; }
        string MessageId { get; set; }
        string Mode { get; set; }
        string ReferenceId { get; set; }
        string Topic { get; set; }
    }
}