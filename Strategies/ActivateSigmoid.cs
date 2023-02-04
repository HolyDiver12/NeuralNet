using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActivateSigmoid : ActivateClass, IActivateFunc
    {
        public ActivateSigmoid() : base(ActivationFunc.ACT_SIGMOID) { }

        public override float ActivateValue(float input_value)
        {
            return 1.0F / (1.0F + (float)Math.Pow(Math.E, input_value * -1.0F));
        }

        public override float ActivateDerivateError(float input_value)
        {
            return input_value * (1.0F - input_value);
        }
    }
}
