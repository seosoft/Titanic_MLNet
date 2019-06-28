using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace TitanicPredict
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            var rawDataView = mlContext.Data.LoadFromTextFile<Passenger>("Data/test.txt", hasHeader: true);
            var replaceEstimator = mlContext.Transforms.ReplaceMissingValues("Age", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            var replaceTransformer = replaceEstimator.Fit(rawDataView);
            var testDataView = replaceTransformer.Transform(rawDataView);

            var trainedModel = mlContext.Model.Load("Data/TrainedModel.zip", out _);
            var predictions = trainedModel.Transform(testDataView);
            var labels = predictions.GetColumn<bool>("PredictedLabel");
            ShowResult.ShowPredictedLabels(mlContext, testDataView, labels);
        }
    }
}
