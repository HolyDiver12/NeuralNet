using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            DoAddBias = false;
            LayersSize = new int[] { 64, 50, 50, 10 };
        }
    }
}
