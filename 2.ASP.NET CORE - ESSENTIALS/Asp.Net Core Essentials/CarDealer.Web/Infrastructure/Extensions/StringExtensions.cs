using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private const string NumberFormat = "F2";

        public static string ToPrice(this decimal priceText)
        {
            return $"${priceText.ToString(NumberFormat)}";
        }

        public static string ToPercentage(this double priceText)
        {
            return $"{priceText.ToString(NumberFormat)}%";
        }
    }
}
