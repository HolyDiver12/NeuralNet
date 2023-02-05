using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public abstract class ActivateClass : IActivateFunc
    {
        public ActivateClass(ActivationFunc func)
        {
            ActivationFunc = func;
            FuncName = "No func selected";
        }
        public string FuncName { get; protected set; }
        public readonly ActivationFunc ActivationFunc;
        public abstract float ActivateValue(float input_value);
        public abstract float ActivateDerivateError(float input_value);
    }
}
