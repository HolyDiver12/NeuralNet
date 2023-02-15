using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    public enum PrepareType
    {
        PREP_UNSIG = 1,
        PREP_ZERO_AV = 2,
        PREP_ZERO_WAV = 3
    }

    public enum ActivationFunc
    {
        ACT_SIGMOID = 1,
        ACT_RELU = 2,
        ACT_HYPER_TAN = 4
    }

    public enum InitSynapsMethod
    {
        INIT_KAIMING = 1,
        INIT_RAND = 2
    }

    public enum LayerPosition
    {
        Input, Hidden, Out
    }
}
