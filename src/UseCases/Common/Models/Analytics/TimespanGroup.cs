using System;

namespace MyHome.Application.Common.Models.Analytics
{
    public class TimespanGroup
    {
        public DateTime Key { get; set; }

        public long Count { get; set; }

        public float Total { get; set; }

        public float Income { get; set; }

        public float Expenses { get; set; }

        public float Max { get; set; }

        public float Min { get; set; }

        public TimespanGroup(DateTime key, long count, float total, float income, float expense, float max, float min)
        {
            Key = key;
            Count = count;
            Total = total;
            Income = income;
            Expenses = expense;
            Max = max;
            Min = min;
        }
    }
}
