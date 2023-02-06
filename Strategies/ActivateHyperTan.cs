using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActivateHyperTan : ActivateClass, IActivateFunc
    {
        public ActivateHyperTan() : base(ActivationFunc.ACT_HYPER_TAN) => FuncName = "Hyper Tangent";

        public override float ActivateValue(float input_value)
        {
            //float La =  ((float)(Math.Exp((double)input_value * 2D) - 1D)) / ((float)(Math.Exp( (double)input_value * 2D) + 1D));
            float La = (float)(Math.Exp((double)input_value) - Math.Exp(-(double)input_value)) /
                       (float)(Math.Exp((double)input_value) + Math.Exp(-(double)input_value));
            return La;
        }

        public override float ActivateDerivateError(float input_value)
        {
            float La = (float)(1 - Math.Pow(ActivateValue(input_value) , 2));
            return La;
        }
    }
}
