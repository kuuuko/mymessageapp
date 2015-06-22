using System;
using System.Collections.Generic;
using System.Web;

namespace MyMessageApp.Models
{
    public class NewsArticle
    {   
        public virtual int article_Id { get; set; }
        public virtual string article_text { get; set; }
        public virtual int root_message_Id { get; set; }
    }
 
}