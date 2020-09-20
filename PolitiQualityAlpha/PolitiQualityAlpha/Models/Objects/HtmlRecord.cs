using PolitiQualityAlpha.Logic.Enums;
using PolitiQualityAlpha.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQuality.Logic.Objects
{
    public class HtmlRecord
    {
        public PublisherEnum NewsPublisher { get; set; }

        public TypeOfCosine CosineType { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Header { get; set; }

        public string Article { get; set; }

        public string Author { get; set; }

        public string Date { get; set; }

        public DateTime DateRecord { get; set; }

        public Dictionary<string, int> ArticleWordsAndFrequency { get; set; }

        public List<string> ArticleUniqueWords { get; set; }

        public int ArticleId { get; set; }
    }
}
