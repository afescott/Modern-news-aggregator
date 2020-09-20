using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Objects
{
   public class CosineArticleSet
    {

        public VectorEntry ArticleX { get; set; }

        public VectorEntry ArticleY { get; set; }

        public VectorEntry ArticleZ { get; set; }

        public HtmlRecord HeaderMostSimilarToArticles { get; set; }

        public double CosineArticleScoreOtherArticles { get; set; }

        public double CosineDescriptionScoreAggregated { get; set; }

        public double CosineTitleScoreAggregated { get; set; }
    }
}
