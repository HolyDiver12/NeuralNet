using System;
namespace NeuralNet.Strategies
{
	public class InitSynapsRand : ISynapseInitFunc
	{
        string ISynapseInitFunc.FuncName => "RAND 0.0 - 1.0";

        float ISynapseInitFunc.InitSynapse(int val)
        {
            return (float)Program.rand.NextDouble();
        }
    }
}

