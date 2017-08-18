using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceChatBot.Models
{
    [Serializable]
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Data> Incidents { get; set; }
    }
}