using System;
using System.Collections.Generic;

namespace NinerFiVisualize.API.Data.Models
{
    public partial class VwTrafficStats
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string Hostname { get; set; } = null!;
        public long? LogEntries { get; set; }
    }
}
