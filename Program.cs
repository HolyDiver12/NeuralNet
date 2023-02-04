namespace NeuralNet
{
    internal class Program
    {
        public const uint NN_ICON_SIZE = 8 * 8;
        public const string NN_TRAIN_FILE = "DATASET/optdigits.tra";
        public const string NN_TEST_FILE = "DATASET/optdigits.tes";

        public static Random rand;

        public static void Main()
        {
            rand = new Random();
            Console.WriteLine("Reading icons from file");
            NNData icons = new();
            int read_count;
            try
            {
                read_count = icons.ReadFromFile(NN_TRAIN_FILE);
                icons.Prepare(PrepareType.PREP_ZERO_WAV);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read icons from file and initialize");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }
            Console.WriteLine($"{read_count} icons read and prepared");

            NeuralNetwork my_net;
            Console.WriteLine("Creating neural network and initializing with parameters");
            try
            {
                my_net = new NeuralNetwork(new NNParameters());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create and initialize neural net");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }

            (float f_result, bool was_gess) result;

            do
            {
                result = my_net.DoForwardRun(icons.icons_list_[0]);
                my_net.DoErrBackPropagation();

                Console.WriteLine($"Variation = {result.f_result} \t Guess? {result.was_gess}");
                //Console.ReadKey();


            } while (result.f_result > 0.001);



            Console.WriteLine("Everything seem to be OK!");
        }
    }
}