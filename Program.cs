namespace NeuralNet
{
    internal class Program
    {
        public const uint NN_ICON_SIZE = 8 * 8;
        public const string NN_TRAIN_FILE = "DATASET/optdigits.tra";
        public const string NN_TEST_FILE = "DATASET/optdigits.tes";
        public const string NN_FILE = "NeuralNet.xml";

        public static Random? rand;

        public static void Main()
        {
            rand = new Random();
            NeuralNetwork my_net = new();
            int unswer = GetUserUnswer($"Ready to initialize new neural network (1), or load it from file {NN_FILE} (2)? (1/2/Q):", "12Q");
            try
            {
                switch (unswer)
                {
                    case 0:
                        my_net.InitWithParameters(new NNParameters()); break; //Find actual parameters in NNParameters ctor.
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

            my_net.net_params_.ConPrintOut();

            Console.Write("Network is ready! ");

            bool quit = false;
            while (!quit) {
                unswer = GetUserUnswer("Train (1), Test (2), or Quit, or alg test (3)? (1/2/Q/3):", "12Q3");
                switch (unswer)
                {
                    case 0:
                        {    //Going to perform some workouts
                            NNData data = new();
                            try
                            {
                                data.ReadFromFile(NN_TRAIN_FILE);
                                data.Prepare(PrepareType.PREP_ZERO_WAV);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to load test dataset!");
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                throw;
                            }

                            RollingAverage<float> rollingAverage = new(300);
                            float performance = 0F;
                            (float error, bool guess) nn_result;
                            Console.WriteLine($"Train run of {data.icons_list_.Count} icons");
                            for (int i = 0; i < data.icons_list_.Count; i++)
                            {
                                nn_result = my_net.DoForwardRun(data.icons_list_[i]);
                                rollingAverage.AddValue(nn_result.error);
                                if (nn_result.guess) performance++;
                                if (i % 300 == 0 && i != 0)
                                {
                                    Console.WriteLine($"{i} sets processed with av error of {rollingAverage.Average} and performance {performance / 300f} %");
                                    performance = 0f;
                                }
                                my_net.DoErrBackPropagation();
                            }
                            Console.WriteLine("Training complete!");
                            break;
                        }
                    case 1:
                        {
                            NNData data = new();
                            try
                            {
                                data.ReadFromFile(NN_TEST_FILE);
                                data.Prepare(PrepareType.PREP_ZERO_WAV);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to load test dataset!");
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                throw;
                            }
                            RollingAverage<float> rollingAverage = new(300);
                            float performance = 0F;
                            (float error, bool guess) nn_result;
                            Console.WriteLine($"Test run of {data.icons_list_.Count} icons");
                            for (int i = 0; i < data.icons_list_.Count; i++)
                            {
                                nn_result = my_net.DoForwardRun(data.icons_list_[i]);
                                rollingAverage.AddValue(nn_result.error);
                                if (nn_result.guess) performance++;
                                if (i % 300 == 0 && i != 0)
                                {
                                    Console.WriteLine($"{i} sets processed with av error of {rollingAverage.Average}");

                                }
                                my_net.DoErrBackPropagation();
                            }
                            Console.WriteLine($"Test complete with overall performance of {performance / data.icons_list_.Count}%");
                            break;
                        }
                    case 2:
                        {
                            if (!my_net.IsSaved)
                            {
                                int uns = GetUserUnswer("Your neural net is not saved! Save? (Y/N):", "YN");
                                if (uns == 0) my_net.Save(NN_FILE);
                            }
                            quit = true;
                            break;
                        }
                    case 3:
                        {
                            NNData data = new();
                            try
                            {
                                data.ReadFromFile(NN_TEST_FILE);
                                data.Prepare(PrepareType.PREP_ZERO_WAV);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to load test dataset!");
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                throw;
                            }
                            RollingAverage<float> rollingAverage = new(300);
                            float performance = 0F;
                            (float error, bool guess) nn_result;
                            //Console.WriteLine($"Test run of {data.icons_list_.Count} icons");
                            for (int i = 0; i < data.icons_list_.Count; i++)
                            {
                                nn_result = my_net.DoForwardRun(data.icons_list_[1]);
                                rollingAverage.AddValue(nn_result.error);
                                if (nn_result.guess) performance++;
                                
                                
                                Console.WriteLine($"{i} sets processed with av error of {rollingAverage.Average}");

                                
                                my_net.DoErrBackPropagation();
                                Console.ReadKey();
                            }
                            Console.WriteLine($"Test complete with overall performance of {performance / data.icons_list_.Count}%");
                            break;
                        }
                }
            }
            Console.WriteLine("Have a nice day!");
            return;
        }

        public static int GetUserUnswer( string greeting, string variants, bool isCaseSensitive = false)
        {
            int result;
            string? input_string;
            if(!isCaseSensitive)  greeting = greeting.ToUpper(); 
            Console.Write( greeting );
            while (true)
            {
                input_string = Console.ReadLine();
                if (input_string==null) continue;
                if (input_string.Length > 1) continue;
                if( !isCaseSensitive ) input_string = input_string.ToUpper();
                result = variants.IndexOf(input_string);
                if(result == -1) continue;
                break;
            }
           
            return result;
        }
    }
}