using System;
using System.Collections.Generic;

namespace NinerFiVisualize.API.Data.Models
{
    public partial class VwLogCount
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Hour { get; set; }
        public long? NumberOfLogs { get; set; }
    }
}
