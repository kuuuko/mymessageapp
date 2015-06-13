using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyMessageApp.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int lft { get; set; }
        public int rgt { get; set; }
        public int plus { get; set; }
        public int minus { get; set; }
        public string text { get; set; }
    }

    public class MessageDBContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}