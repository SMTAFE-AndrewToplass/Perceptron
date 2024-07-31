namespace Perceptron
{

    static class Program
    {
        // Strings used for outputting the inputs & results.
        private static readonly Dictionary<int, string> weatherKeys = new() {
            {-1, "Rainy"},
            { 0, "Cloudy"},
            { 1, "Sunny"},
        };
        private static readonly Dictionary<int, string> moodKeys = new() {
            {-1, "Sad"},
            { 0, "Neutral"},
            { 1, "Happy"},
        };
        private static readonly Dictionary<int, string> resultKeys = new() {
            {0, "Watch movie"},
            {1, "Go to the beach"},
        };

        public static void Main(string[] args)
        {
            // Inputs: Weather & Mood.
            int[,] inputs = {
                { 1, 1}, // Sunny & happy -> Beach
                {-1, 1}, // Rainy & happy -> Movie
                { 1,  0}, // Sunny & Neutral -> Beach
                { 1, -1}, // Sunny & Sad -> Movie
            };
            // Whether to watch a movie (0) or go to the beach (1).
            int[] outputs = {
                1, 0, 1, 0,
            };

            // Assign random weights and bias before training.
            Random r = new();
            double[] weights = {
                r.NextDouble(), // weather input
                r.NextDouble(), // mood input
                r.NextDouble(), // bias
            };

            // Perform training on the perceptron to adjust the weights.
            Train(weights, inputs, outputs);

            // Data to test perceptron with (includes additional data not
            // present in training).
            int[,] testData = {
                { 1,  1}, // Sunny & happy
                { 1,  0}, // Sunny & neutral
                { 1, -1}, // Sunny & sad
                { 0,  1}, // Cloudy & happy
                { 0,  0}, // Cloudy & neutral
                { 0, -1}, // Cloudy & sad
                {-1,  1}, // Rainy & happy
                {-1,  0}, // Rainy & Neutral
                {-1, -1}, // Rainy & Sad
            };

            Console.WriteLine("Results:");
            for (int i = 0; i < testData.GetLength(0); i++)
            {
                // Current weather (-1: Rainy, 0: Cloudy, 1: Sunny).
                int weather = testData[i, 0];
                // Current mood (-1: Sad, 0: Neutral, 1: Happy).
                int mood = testData[i, 1];

                // Use the perceptron to determine whether to go to the beach or
                // watch a movie.
                int result = CalculateOutput(weather, mood, weights);

                // Write results to the console.
                Console.WriteLine($"{weatherKeys[weather],-6} + {moodKeys[mood],-7} = {resultKeys[result],-15}");
            }
            Console.WriteLine($"Weights: {string.Join(", ", weights)}");
        }

        public static void Train(double[] weights, int[,] inputs, int[] outputs)
        {
            double learningRate = 1;
            double totalError = 1;
            while (totalError > 0.2)
            {
                totalError = 0;
                for (int i = 0; i < inputs.GetLength(0); i++)
                {
                    int output = CalculateOutput(inputs[i, 0], inputs[i, 1], weights);

                    // Get the difference between the actual result and expected
                    // result from the training data.
                    int error = outputs[i] - output;

                    // Adjusting the two input weights.
                    weights[0] += learningRate * error * inputs[i, 0];
                    weights[1] += learningRate * error * inputs[i, 1];

                    // Adjusting bias weight.
                    weights[2] += learningRate * error * 1;

                    totalError += Math.Abs(error);
                }
            }
        }

        public static int CalculateOutput(double input1, double input2, double[] weights)
        {
            // Get sum of all weights multiplied by the inputs.
            double sum = input1 * weights[0] + input2 * weights[1] + 1 * weights[2];
            // Threshold for returning result.
            return sum >= 0 ? 1 : 0;
        }
    }
}
