using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.ML;
using TitanicPredict;
using TitanicPredict.Data;

[assembly: WebJobsStartup(typeof(Startup))]
namespace TitanicPredict
{
    class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddPredictionEnginePool<Passenger, PassengerPredict>()
                .FromFile("Models/TrainedModel.zip");
                // .FromUri("https://<Blobストレージアカウント>/models/TrainedModel.zip");
        }
    }
}