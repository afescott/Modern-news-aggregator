using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Objects
{
   public class HighthresholdResult
    {

        public List<VectorEntry> VectorEntries { get; set; }

        public HtmlRecord Record { get; set; }

        public HtmlRecord HighestValueNeighbour { get; set; }
               
        public double TotalCosineScore { get; set; }

        public double AggregatedCosineScoreAverage { get; set; }

        public bool IsFinalCluster { get; set; }


    }
}
