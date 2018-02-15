using HtmlAgilityPack;
using OWOrganizerDLL.Objects;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OWOrganizerDLL.Helpers
{
    public static class Basics
    {
        public static async Task<HtmlDocument> GetPage(string url)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var document = new HtmlDocument();
                var result = await response.Content.ReadAsStringAsync();
                document.LoadHtml(result);
                return document;
            }
            else
            {
                return null;
            }
        }

        public static int? GetSRFromDocument(HtmlDocument page)
        {
            var crElement = page.DocumentNode.Descendants("div")
                             .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("competitive-rank")).FirstOrDefault();

            if (crElement == null) return null;

            if (crElement.ChildNodes.Count() == 2)
            {
                int.TryParse(crElement.ChildNodes[1].InnerText, out int SR);
                return SR;
            }

            return null;
        }

        public static Account UpdateSRAccount(Account acc, int? sr)
        {
            if (sr.HasValue)
            {
                acc.SeasonRating = sr.Value;
                acc.Updated = true;
            }
            else
                acc.Updated = false;

            return acc;
        }
    }
}
