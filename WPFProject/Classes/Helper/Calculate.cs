using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFProject.Classes.Data;

namespace WPFProject.Classes.Helper
{
    public static class Calculate
    {
        public static double Rating(Row currRow)
        {
            double C7 = Double.Parse(currRow.cashflow_category.Value);
            double C8 = Double.Parse(currRow.originality.Value);
            double C9 = Double.Parse(currRow.social_profit.Value);

            double C10 = Double.Parse(currRow.taxes);
            double C12 = Double.Parse(currRow.paid_salary);
            double C13 = Double.Parse(currRow.realize_period);

            double rating = (C10 + C12) * C13 / 12 * (C7 + C8 + C9) / 16;

            return Math.Round(rating, 4);
        }
    }
}
