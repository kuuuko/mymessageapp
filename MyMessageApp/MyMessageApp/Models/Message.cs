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
        public int parentID { get; set; }
        public int order { get; set; }
        public int level { get; set; }
        public string text { get; set; }
    }

    public class MessageDBContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}