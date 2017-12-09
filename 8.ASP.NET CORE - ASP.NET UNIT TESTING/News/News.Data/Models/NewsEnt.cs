using System;
using System.Collections.Generic;
using System.Text;

namespace News.Data.Models
{
    public class NewsEnt
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
