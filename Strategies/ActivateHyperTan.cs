using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActivateHyperTan : IActivateFunc
    {
        public string FuncName { get; }
        public ActivateHyperTan() => FuncName = "Hyper Tangent";

        public  float ActivateValue(float input_value)
        {
            //float La =  ((float)(Math.Exp((double)input_value * 2D) - 1D)) / ((float)(Math.Exp( (double)input_value * 2D) + 1D));
            return  (float)(Math.Exp((double)input_value) - Math.Exp(-(double)input_value)) /
                       (float)(Math.Exp((double)input_value) + Math.Exp(-(double)input_value));
            
        }

        public  float ActivateDerivateError(float input_value)
        {
            return (float)(1 - Math.Pow(ActivateValue(input_value) , 2));
            
        }
    }
}
