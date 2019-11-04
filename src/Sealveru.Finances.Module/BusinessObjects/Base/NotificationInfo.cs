using DevExpress.ExpressApp;

namespace Sealveru.Finances.Module
{
    public class NotificationInfo
    {
        public NotificationInfo() { }
        public NotificationInfo(string message, string caption, InformationType type)
        {
            Message = message;
            Caption = caption;
            Type = type;
        }

        public string Message { get; set; }
        public string Caption { get; set; }
        public InformationType Type { get; set; }
    }
}