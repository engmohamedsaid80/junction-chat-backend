using System;

namespace DomainCore.Models
{
    public class GroupModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public string Owner { get; set; }
        public int MemberCount { get; set; }
    }
}
