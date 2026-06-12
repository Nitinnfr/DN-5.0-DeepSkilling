using System;

namespace FinancialForecasting
{
    public class ForecastingEngine
    {
        // --- STEP 3: IMPLEMENT RECURSIVE ALGORITHM ---
        /// <summary>
        /// Predicts the future value of an asset using a recursive approach.
        /// </summary>
        /// <param name="currentValue">The initial capital or present value.</param>
        /// <param name="growthRate">The constant annual growth rate (e.g., 0.05 for 5%).</param>
        /// <param name="years">The numbers of years to forecast into the future.</param>
        /// <returns>The forecasted future asset value.</returns>
        public static double PredictFutureValue(double currentValue, double growthRate, int years)
        {
            // 1. THE BASE CASE
            // If the remaining forecast timeline hits 0 years, return the value as-is.
            if (years <= 0)
            {
                return currentValue;
            }

            // 2. THE RECURSIVE STEP
            // Calculate the value at the end of the current year
            double nextYearValue = currentValue * (1 + growthRate);

            // Call itself recursively, reducing the remaining years by 1
            return PredictFutureValue(nextYearValue, growthRate, years - 1);
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Initializing Predictive Financial Core ---\n");

            // Setup parameters: $10,000 baseline investment growing at 7% annually
            double principal = 10000.00;
            double annualGrowthRate = 0.07; 
            int forecastHorizon = 10; // Forecast duration in years

            Console.WriteLine($"Initial Investment: ${principal:N2}");
            Console.WriteLine($"Assumed Growth Rate: {annualGrowthRate * 100}% per annum");
            Console.WriteLine($"Forecast Horizon:    {forecastHorizon} Years\n");

            Console.WriteLine("Calculating recursive future valuation projections...");
            double finalValue = ForecastingEngine.PredictFutureValue(principal, annualGrowthRate, forecastHorizon);

            Console.WriteLine("\n================ PROJECTION REPORT ================");
            Console.WriteLine($" Estimated Future Asset Worth: ${finalValue:N2}");
            Console.WriteLine($" Total Compounded Interest:   ${(finalValue - principal):N2}");
            Console.WriteLine("===================================================\n");
        }
    }
}