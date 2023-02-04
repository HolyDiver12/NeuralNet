using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    public class NNIcon
    {
        public float[] data_; //= new float[Program.NN_ICON_SIZE];
        public uint val_;
        public NNIcon() => data_ = new float[Program.NN_ICON_SIZE];
    };
}
