using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NeuralNet.Strategies;

namespace NeuralNet
{
    [Serializable]
    public class NNParameters
    {
        public float LearnRate;
        public float Momentum;
        public ActivationFunc Func;
        public InitSynapsMethod InitSynaps;
        public PrepareType NetPrepare;
        public bool DoAddBias;
        public int[] LayersSize;

        public NNParameters() 
        {
            LearnRate = 0.7F;
            Momentum = 0.6F;
            Func = ActivationFunc.ACT_SIGMOID;
            InitSynaps = InitSynapsMethod.INIT_KAIMING;
            NetPrepare = PrepareType.PREP_ZERO_WAV;
            DoAddBias = true;
            LayersSize = new int[] { 64, 50, 50, 10 };
        }
        /*public void ConPrintOut()
        {
            Console.WriteLine("Neural Network parameters are:");
            Console.WriteLine("\tUse bias neurons: " + (DoAddBias? "Yes" : "No"));
            //Console.WriteLine("\tActivation function used: " + activateClass.FuncName);
            Console.Write("\tInitialization method for synaps: ");
            switch (InitSynaps)
            {
                case InitSynapsMethod.INIT_KAIMING:
                    Console.WriteLine("Kaiming");
                    break;
                case InitSynapsMethod.INIT_LSUV:
                    Console.WriteLine("LSUV");
                    break;
                default:
                    throw new Exception("No InitSynapsMethod set!");
                    break;
            }
            Console.WriteLine($"\tLearn rate is: {LearnRate} ; Momentum is {Momentum}" );
            Console.WriteLine($"\tNeural network has got {LayersSize.Length} layers of which:" );
            for( int i = 0; i < LayersSize.Length; i++)
                Console.WriteLine($"\t\tLayer {i+1} has {LayersSize[i]} neurons");
        }*/
    }
}
