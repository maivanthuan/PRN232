using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
	public class NewsUpdateDto
	{
		[Required]
		public string NewsArticleId { get; set; } = string.Empty;

		[Required]
		[StringLength(200)]
		public string NewsTitle { get; set; } = string.Empty;

		[Required]
		public string NewsContent { get; set; } = string.Empty;

		[Required]
		public string Headline { get; set; } = string.Empty;

		public short CategoryId { get; set; }

		public bool NewsStatus { get; set; }

		public List<int> TagIds { get; set; } = new();
	}
}
