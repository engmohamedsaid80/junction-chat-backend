using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatGroupsAPI.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string Game { get; set; }

        public IEnumerable<GroupNameViewModel> UserGroups { get; set; }

        public IEnumerable<GroupNameViewModel> OtherGroups { get; set; }
    }

    public class GroupNameViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
