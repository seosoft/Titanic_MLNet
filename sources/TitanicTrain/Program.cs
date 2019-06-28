using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace TitanicTrain
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            var rawDataView = mlContext.Data.LoadFromTextFile<Passenger>("Data/train.txt", hasHeader: true);
            ShowResult.ShowPassengers(mlContext, rawDataView);

            var replaceEstimator = mlContext.Transforms.ReplaceMissingValues("Age", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            var replaceTransformer = replaceEstimator.Fit(rawDataView);
            var trainingDataView = replaceTransformer.Transform(rawDataView);
            ShowResult.ShowPassengers(mlContext, trainingDataView);

            var splitData = mlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            var prepEstimator = mlContext.Transforms.Concatenate("Features", "Pclass", "Sex", "SibSp", "Parch")
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());
            var trainedModel = prepEstimator.Fit(splitData.TrainSet);

            var predictions = trainedModel.Transform(splitData.TestSet);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions);
            ShowResult.ShowMetrics(metrics);

            mlContext.Model.Save(trainedModel, rawDataView.Schema, "Data/TrainedModel.zip");
        }
    }
}
