using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
    public class NewsDto
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public string NewsContent { get; set; } = string.Empty;
        public string? NewsSource { get; set; }
        public string? CategoryName { get; set; }
        public bool? NewsStatus { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
