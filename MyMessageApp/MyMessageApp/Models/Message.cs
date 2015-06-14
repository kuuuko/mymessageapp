using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyMessageApp.Models
{
    public class Message
    {
        public virtual int message_Id { get; set; }
        public virtual int lft { get; set; }
        public virtual int rgt { get; set; }
        public virtual int plus { get; set; }
        public virtual int minus { get; set; }
        public virtual string text { get; set; }
        public virtual bool abuseFlag { get; set; }
        public virtual DateTime? date { get; set; }
        public virtual int user_Id { get; set; }
    }

    public class MessageDBContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}