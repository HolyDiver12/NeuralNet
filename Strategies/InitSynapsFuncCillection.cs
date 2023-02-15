using System;
namespace NeuralNet.Strategies
{
	public class InitSynapsFuncCillection
	{
        (InitSynapsMethod func_type, ISynapseInitFunc InitSynapsFunc)[] InitSynapsFuncs;
        public InitSynapsFuncCillection()
        {
            InitSynapsFuncs = new (InitSynapsMethod func_type, ISynapseInitFunc InitSynapsFunc)[2];
            InitSynapsFuncs[0] = (InitSynapsMethod.INIT_KAIMING, new InitSynapsKaiming());
            InitSynapsFuncs[1] = (InitSynapsMethod.INIT_RAND, new InitSynapsRand());
            //InitSynapsFuncs[2] = (PrepareType.PREP_ZERO_WAV, new PrepareIconsWeightedAverageNull());
        }

        public ISynapseInitFunc GetSynapseInitFunc(InitSynapsMethod init_method)
        {
            foreach (var (func_type, InitSynapsFunc) in InitSynapsFuncs)
            {
                if (func_type == init_method) return InitSynapsFunc;
            }
            throw new NotImplementedException();
        }
    }
}

