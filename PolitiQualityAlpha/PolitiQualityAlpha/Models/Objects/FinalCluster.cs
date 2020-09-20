using PolitiQuality.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Objects
{
    public class FinalCluster
    {

        public List<VectorEntry> VectorEntries { get; set; }

        public double AggregatedCosineScoreAverage { get; set; }

        public HtmlRecord Article { get; set; }

        public int ClusterNumber { get; set; }

 
    }
}
