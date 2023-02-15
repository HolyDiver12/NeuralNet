using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class ActFuncCollection
    {
        private (ActivationFunc func_name, IActivateFunc ActivateFuncClass)[] ActivationFuncs;
        public ActFuncCollection()
        {
            ActivationFuncs = new (ActivationFunc func_name, IActivateFunc ActivateFuncClass)[3];
            ActivationFuncs[0] = (ActivationFunc.ACT_SIGMOID, new ActivateSigmoid());
            ActivationFuncs[1] = (ActivationFunc.ACT_RELU, new ActivateReLu());
            ActivationFuncs[2] = (ActivationFunc.ACT_HYPER_TAN, new ActivateHyperTan());
        }
        public IActivateFunc GetActivateFuncClass( ActivationFunc func_type)
        {
            foreach(var (func_name, ActivateFuncClass) in ActivationFuncs) 
            {
                if (func_type == func_name) return ActivateFuncClass;
            }
            throw new NotImplementedException();
        }
    }
}
