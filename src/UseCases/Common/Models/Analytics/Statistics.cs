using System.Collections.Generic;

namespace MyHome.Application.Common.Models.Analytics
{
    public class Statistics
    {
        public float Balance { get; set; }

        public decimal Count { get; set; }

        public IEnumerable<TimespanGroup> Buckets { get; set; } = new List<TimespanGroup>();
    }
}
