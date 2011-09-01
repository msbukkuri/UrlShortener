using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class UrlShortenerController : Controller
    {
        private UrlShortenerDbContext urls = new UrlShortenerDbContext();
        //
        // GET: /UrlShortener/

        //public ActionResult GenerateShortUrl()
        //{
        //    return View();
        //}

        public ActionResult GenerateShortUrl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateShortUrl(UrlShortenerModel model)
        {
            if (!urls.Urls.Any(item => item.FullUrl.Equals(model.FullUrl)))
            {
                var url = new UrlShortenerModel { FullUrl = model.FullUrl, ShortUrl = ShortenUrl(model.FullUrl), ShortenCount = 1 };
                urls.Urls.Add(url);
                urls.SaveChanges();
            }
            else
            {
                UrlShortenerModel urlFound = urls.Urls.Where(url => url.FullUrl.Equals(model.FullUrl)).First();
                urlFound.ShortenCount++;
                urls.SaveChanges();
            }
            return View();
        }

        private string ShortenUrl(string fullUrl)
        {
            string rebuiltString = string.Empty;
            for (int i = 0; i < new Random().Next(3, fullUrl.Length - 1); i++)
            {
                rebuiltString += fullUrl[new Random().Next(0, fullUrl.Length)];
            }

            string shortUrl = string.Empty;
            foreach (var charString in rebuiltString)
            {
                shortUrl += Base62ToString(Convert.ToInt64(charString));
            }
            return shortUrl;
        }


        public ActionResult UrlAnalytics()
        {
            return View(urls.Urls.ToList());
        }

        public string Base62ToString(long fromValue)
        {
            int baseNum = 62;
            const string baseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string toValue = fromValue == 0 ? "0" : "";
            int mod = 0;

            while (fromValue != 0)
            {
                mod = (int)(fromValue % baseNum); //should be safe
                toValue = baseDigits.Substring(mod, 1) + toValue;
                fromValue = fromValue / baseNum;
            }

            return toValue;
        }
    }
}
