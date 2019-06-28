using Microsoft.ML.Data;

namespace TitanicPredict.Data
{
 public class Passenger
    {
        [LoadColumn(1)]
        public float Pclass { get; set; }

        [LoadColumn(3)]
        public float Sex { get; set; }

        [LoadColumn(4)]
        public float Age { get; set; } = float.NaN;

        [LoadColumn(5)]
        public float SibSp { get; set; }

        [LoadColumn(6)]
        public float Parch { get; set; }

        [LoadColumn(8)]
        public float Fare { get; set; }
    }
}