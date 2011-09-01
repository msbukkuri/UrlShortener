using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace UrlShortener.Models
{
    public class UrlShortenerModel
    {
        [Key]
        [DataType(DataType.Url)]
        public string FullUrl { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string ShortUrl { get; set; }

        [Required]
        public int ShortenCount { get; set; }
    }

    public class UrlShortenerDbContext : DbContext
    {
        public DbSet<UrlShortenerModel> Urls { get; set; }
    }
}