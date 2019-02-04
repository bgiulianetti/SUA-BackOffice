using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class SplineChartDataContract
    {
        public string name { get; set; }
        public string type { get; set; }
        public string yValueFormatString { get; set; }
        public bool showInLegend { get; set; }
        public List<DataPointsSplineContract> dataPoints { get; set; }
        public bool visible { get; set; }
    }

    public class DataPointsSplineContract
    {
        public int y { get; set; }
        public double x { get; set; }
    }
}