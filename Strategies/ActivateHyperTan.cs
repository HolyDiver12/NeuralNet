using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActivateHyperTan : ActivateClass, IActivateFunc
    {
        public ActivateHyperTan() : base(ActivationFunc.ACT_HYPER_TAN) => FuncName = "Hyper Tangen";

        public override float ActivateValue(float input_value)
        {
            return ((float)(Math.Pow(Math.E, (double)input_value * 2D) - 1D)) / ((float)(Math.Pow(Math.E, (double)input_value * 2D) - 1D));
        }

        public override float ActivateDerivateError(float input_value)
        {
            return input_value * (1.0F - input_value);
        }
    }
}
