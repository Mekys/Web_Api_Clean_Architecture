using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public Position Code { get; set; }
        public string? Description { get; set; }

    }
}
