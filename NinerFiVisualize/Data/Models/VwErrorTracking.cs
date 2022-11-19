﻿using System;
using System.Collections.Generic;

namespace NinerFiVisualize.Data.Models
{
    public partial class VwErrorTracking
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string? Hostname { get; set; }
        public string TypeName { get; set; } = null!;
        public int? LogEntries { get; set; }
    }
}
