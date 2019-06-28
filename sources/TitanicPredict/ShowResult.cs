using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;

namespace TitanicPredict
{
    internal static class ShowResult
    {
        internal static void ShowPredictedLabels(MLContext mlContext, IDataView dataView, IEnumerable<bool> labels)
        {
            Console.WriteLine("Pred\tPclass\tSex\tAge\tSibSp\tParch\tFare");
            Console.WriteLine("");

            var testData = mlContext.Data.CreateEnumerable<Passenger>(dataView, reuseRowObject: false).Take(20).ToArray();
            var labelsArray = labels.Take(20).ToArray();
            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine($"{labelsArray[i]}\t{testData[i].Pclass}\t{testData[i].Sex}\t{testData[i].Age:F1}\t{testData[i].SibSp}\t{testData[i].Parch}\t{testData[i].Fare}");
            }
        }
    }
}