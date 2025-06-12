using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
 
        public class NewsCreateDto
        {
            public string? NewsArticleId { get; set; }  // Bỏ qua khi tạo, dùng khi cập nhật

            [Required]
            [StringLength(200)]
            public string NewsTitle { get; set; } = string.Empty;

            [Required]
            [StringLength(300)]
            public string Headline { get; set; } = string.Empty;

            [Required]
            public string NewsContent { get; set; } = string.Empty;

            [Required]
            public short CategoryId { get; set; }

            public bool NewsStatus { get; set; } = true;

            public List<int> TagIds { get; set; } = new();

            public DateTime CreatedDate { get; set; } = DateTime.Now;

            public string AuthorName { get; set; } = string.Empty;  // Có thể hiển thị ở view nếu cần
        }

    }

