using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    [Serializable]
    public class NNSerialisationInfo
    {
        public (float f_value_, float f_last_diff_)[][][]? synaps_vectors; //[layer][neuron][synaps]
        public NNParameters? parameters { get; set; }
        public NNSerialisationInfo() { }
    }
}
