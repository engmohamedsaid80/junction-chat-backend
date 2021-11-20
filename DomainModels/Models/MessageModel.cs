using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Models
{
    public class MessageModel
    {
        public string id { get; set; }
        public string SenderName { get; set; }
        public string GroupName { get; set; }
        public DateTime MessageDT { get; set; }
        public string Content { get; set; }
    }
}
