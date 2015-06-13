using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMessageApp.Models
{
    public class MessageViewModel
    {
        public int id { get; set; }
        public string messageText { get; set; }
        public int identLevel { get; set; }
    }
}