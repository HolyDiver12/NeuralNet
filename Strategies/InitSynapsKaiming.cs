using System;
using NeuralNet;
namespace NeuralNet.Strategies
{
	public class InitSynapsKaiming : ISynapseInitFunc
	{

        string ISynapseInitFunc.FuncName => "KAIMING";
        float ISynapseInitFunc.InitSynapse(int val)
        {
            if (val == 0) return 0F;

            return (float)Program.rand.NextDouble() * (float)Math.Sqrt(2.0F / val);
        }
    }
}

