using Microsoft.ML.Data;

namespace TitanicTrain
{
    public class Passenger
    {
        [LoadColumn(1)]
        [ColumnName("Label")]
        public bool Survived { get; set; }

        [LoadColumn(2)]
        public float Pclass { get; set; }

        [LoadColumn(4)]
        public float Sex { get; set; }

        [LoadColumn(5)]
        public float Age { get; set; } = float.NaN;

        [LoadColumn(6)]
        public float SibSp { get; set; }

        [LoadColumn(7)]
        public float Parch { get; set; }

        [LoadColumn(9)]
        public float Fare { get; set; }
    }
}
