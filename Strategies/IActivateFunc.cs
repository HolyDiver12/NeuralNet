using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public interface IActivateFunc
    {
        float ActivateValue(float input_value);
        float ActivateDerivateError(float input_value);

    }
}
