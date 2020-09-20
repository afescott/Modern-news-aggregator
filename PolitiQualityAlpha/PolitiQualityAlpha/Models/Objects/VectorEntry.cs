using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Objects
{
   public  class VectorEntry
    {
        public PublisherEnum ArticleType { get; set; }

        public  Dictionary<string,int> []  VectorDictionaries { get; set; }

        public double CosineScore { get; set; }

        public double CosineTotal { get; set; }

        public bool IsHighThreshold { get; set; }

        public HtmlRecord Record { get; set; }

        public HtmlRecord RecordComparedTo { get; set; }

       






    }
}
