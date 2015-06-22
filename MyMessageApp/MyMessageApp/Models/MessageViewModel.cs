using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyMessageApp.Models
{
    public class MessageViewModel
    {
        public int id { get; set; }
        public int article_id { get; set; }

        [Required]
        public string user_name { get; set; }
        [Required]
        public string messageText { get; set; }

        public DateTime? viewdate { get; set; }
        public int identLevel { get; set; }
        public int plusNumber { get; set; }
        public int minusNumber { get; set; }
    }
}