namespace NeuralNet
{
    internal class Program
    {
        public const uint NN_ICON_SIZE = 8 * 8;
        public const string NN_TRAIN_FILE = "DATASET/optdigits.tra";
        public const string NN_TEST_FILE = "DATASET/optdigits.tes";
        public const string NN_FILE = "NeuralNet.xml";

        public static Random rand;

        public static void Main()
        {
            NeuralNetwork my_net = new();
            int i = GetUserUnswer($"Ready to initialize new neural network (1), or load it from file {NN_FILE} (2)? (1/2/Q):", "12Q");
            try
            {
                switch (i)
                {
                    case 0:
                        my_net.InitWithParameters(new NNParameters()); break;
                    case 1:
                        my_net.InitFromFile(NN_FILE); break;
                    default:
                        Console.WriteLine("Finishing program");
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initiate Neural Network!");   
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }

            i = GetUserUnswer("Network is ready! Train (1), or Test (2)? (1/2):", "12");

            if( i == 0)
            {
                NNData data = new();
                try
                {
                    data.ReadFromFile(NN_TRAIN_FILE);
                    data.Prepare(PrepareType.PREP_ZERO_WAV);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Failed to load test dataset!");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }


            }



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

        public static int GetUserUnswer( string greeting, string variants, bool isCaseSensitive = false)
        {
            int result = 0;
            string? input_string;
            if(!isCaseSensitive)  greeting.ToUpper(); 
            Console.Write( greeting );
            while (true)
            {
                input_string = Console.ReadLine();
                if (input_string==null) continue;
                if (input_string.Length > 1) continue;
                if( !isCaseSensitive ) input_string.ToUpper();
                result = variants.IndexOf(input_string);
                if(result == -1) continue;
                break;
            }
           
            return result;
        }
    }
}