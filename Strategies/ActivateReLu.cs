using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActivateReLu : IActivateFunc
    {
        public string FuncName { get; }
        public ActivateReLu() => FuncName = "Re Lu";

        public  float ActivateValue(float input_value)
        {
            return input_value > 0.0F ? input_value : 0.0F;
        }

        public  float ActivateDerivateError(float input_value)
        {
            return input_value < 0 ? (float)Program.rand.NextDouble() / 20.0F : input_value;
        }
    }
}
