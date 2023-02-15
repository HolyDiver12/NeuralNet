using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class PrepIconsFuncCollection
    {
        (PrepareType func_type, IPrepareIcons PrepareIconsFunc)[] PrepareIconsFuncs;
        public PrepIconsFuncCollection() 
        {
            PrepareIconsFuncs = new (PrepareType func_type, IPrepareIcons PrepareIconsFunc)[3];
            PrepareIconsFuncs[0] = (PrepareType.PREP_UNSIG, new PrepareIconsUnsigned());
            PrepareIconsFuncs[1] = (PrepareType.PREP_ZERO_AV, new PrepareIconsUnsigned());
            PrepareIconsFuncs[2] = (PrepareType.PREP_ZERO_WAV, new PrepareIconsWeightedAverageNull());
        }

        public IPrepareIcons GetPrepareIconsClass(PrepareType prepare_type)
        {
            foreach( var (func_type, PrepareIconsFunc) in PrepareIconsFuncs)
            {
                if (func_type == prepare_type) return PrepareIconsFunc;
            }
            throw new NotImplementedException();
        }
    }
}
