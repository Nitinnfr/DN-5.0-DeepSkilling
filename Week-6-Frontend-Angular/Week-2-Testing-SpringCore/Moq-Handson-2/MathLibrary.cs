using System;

namespace MathLibraryApp
{
    public class MathLibrary
    {
        private double result = 0;

        public double GetResult => result;

        public double Add(double a, double b)
        {
            result = a + b;
            return result;
        }

        public double Subtract(double a, double b)
        {
            result = a - b;
            return result;
        }

        public double Multiply(double a, double b)
        {
            result = a * b;
            return result;
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new ArgumentException("Division by zero");
            result = a / b;
            return result;
        }

        public void AllClear()
        {
            result = 0;
        }
    }
}