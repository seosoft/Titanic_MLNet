using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.ML;
using TitanicPredict.Data;

namespace TitanicPredict
{
    public class PredictSurvived
    {
        [FunctionName("PredictSurvived")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Passenger>(requestBody);

            var prediction = _predictionEnginePool.Predict(data);

            var sentiment = Convert.ToBoolean(prediction.Prediction) ? "Survived" : "Not Survived";
            return new OkObjectResult(sentiment);
        }

        private readonly PredictionEnginePool<Passenger, PassengerPredict> _predictionEnginePool;

        public PredictSurvived(PredictionEnginePool<Passenger, PassengerPredict> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
    }
}