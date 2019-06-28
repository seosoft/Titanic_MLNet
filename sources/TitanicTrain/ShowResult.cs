using System;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace TitanicTrain
{
    internal class ShowResult
    {
        internal static void ShowPassengers(MLContext mlContext, IDataView dataView)
        {
            Console.WriteLine("");
            Console.WriteLine("----------------");
            Console.WriteLine("");
            Console.WriteLine("Surv\tPclass\tSex\tAge\tSibSp\tParch\tFare");
            Console.WriteLine("");

            var preview = mlContext.Data.CreateEnumerable<Passenger>(dataView, reuseRowObject: false).Take(20);
            foreach (var p in preview)
            {
                Console.WriteLine($"{p.Survived}\t{p.Pclass}\t{p.Sex}\t{p.Age:F1}\t{p.SibSp}\t{p.Parch}\t{p.Fare}");
            }
        }

        internal static void ShowMetrics(CalibratedBinaryClassificationMetrics metrics)
        {
            Console.WriteLine("");
            Console.WriteLine("----------------");
            Console.WriteLine("");

            Console.WriteLine($"Accuracy: {metrics.Accuracy}");
            Console.WriteLine($"F1 Score: {metrics.F1Score}");
        }
    }
}