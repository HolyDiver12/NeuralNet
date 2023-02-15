using System;
namespace NeuralNet.Strategies
{
	public interface ISynapseInitFunc
	{
		public string FuncName { get; }
		public float InitSynapse(int val = 0);
	}
}

