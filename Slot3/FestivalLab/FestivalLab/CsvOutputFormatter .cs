using FestivalLab.model;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace FestivalLab
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
        }
        protected override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<Festival>).IsAssignableFrom(type) || typeof(Festival).IsAssignableFrom(type);
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var festivals = context.Object as IEnumerable<Festival> ?? new List<Festival> { context.Object as Festival };

            var writer = new StreamWriter(response.Body, selectedEncoding);

            foreach (var f in festivals)
            {
                await writer.WriteLineAsync($"{f.Id},{f.Name},{f.Province},{f.Date:yyyy-MM-dd}");
            }

            await writer.FlushAsync();

        }

    }
}