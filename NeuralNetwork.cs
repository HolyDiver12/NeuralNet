using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NeuralNet.Strategies;

namespace NeuralNet
{
    public class NeuralNetwork
    {
        public NNParameters net_params_ { get; private set; }
        private NeuralNetLayer[] net_layers_;
        public bool IsSaved { get; private set; }
        public bool IsInitialized { get; private set; }
        public string? FileName { get; private set; }

        //Using stratagies interfaces
        private IActivateFunc activateFunc;
        private ISynapseInitFunc InitSynapsFunc;

        public NeuralNetwork() => IsSaved = false;
        public NeuralNetwork(string file_name)
        {
            InitFromFile(file_name);
        }
        public NeuralNetwork(in NNParameters p)
        {
            IsSaved = false;
            InitWithParameters(p);
        }


        public void InitFromFile(string filename)
        {
            if (IsInitialized) throw new InvalidOperationException("Trying to initialize from file already initialized neural net!");
            NNSerialisationInfo? net_info = LoadNeuralNetInfo(filename);
            if (net_info == null) throw new FileLoadException("Coud not load neural net info from file " + filename);
            net_params_ = net_info!.parameters;

            InitNeuralNetwork(net_info.synaps_vectors);
            IsSaved = true;
            IsInitialized = true;
            FileName = filename;

        }

        public void InitWithParameters(NNParameters? p = null)
        {
            if (net_params_ == null && p == null)
                throw new InvalidOperationException("No parameters for network initialization");
            if (p != null) net_params_ = p;

            InitNeuralNetwork();
            for (int i = 0; i < net_params_!.LayersSize.Length; i++)
                net_layers_[i].InitSynapse(net_params_.InitSynaps);
            IsSaved = false;
            IsInitialized = true;
        }
        //------------------------------------
        // Инициализирует нейросеть параметрами p
        // Если p опущено, пытается инициализировать с параметрами,
        // данными при создании объекта
        //
        private void InitNeuralNetwork((float f_value_, float f_last_diff_)[][][]? synaps_vectors = null)
        {
            if (net_params_.LayersSize.Length < 2)
                throw new InvalidOperationException("Neural net shoul has at least 2 layers");

            ActFuncCollection act_collection = new();
            activateFunc = act_collection.GetActivateFuncClass(net_params_.Func);
            InitSynapsFuncCillection init_synaps_collection = new();
            InitSynapsFunc = init_synaps_collection.GetSynapseInitFunc(net_params_.InitSynaps);


            net_layers_ = new NeuralNetLayer[net_params_.LayersSize.Length];
            for (int i = 0; i < net_params_.LayersSize.Length; i++)
            {
                if (i == 0)
                    net_layers_[i] = new NeuralNetLayer(net_params_.LayersSize[i],
                                                        LayerPosition.Input,
                                                        activateFunc,
                                                        InitSynapsFunc,
                                                        net_params_.DoAddBias);


                else if (i == net_params_.LayersSize.Length - 1)
                    net_layers_[i] = new NeuralNetLayer(net_params_.LayersSize[i],
                                                        LayerPosition.Out,
                                                        activateFunc,
                                                        InitSynapsFunc,
                                                        false, //В последнем слое нейронов смещения не бывает
                                                        net_layers_[i - 1],
                                                        synaps_vectors == null ? null : synaps_vectors[i - 1]);

                else
                    net_layers_[i] = new NeuralNetLayer(net_params_.LayersSize[i],
                                                        LayerPosition.Hidden,
                                                        activateFunc,
                                                        InitSynapsFunc,
                                                        net_params_.DoAddBias,
                                                        net_layers_[i - 1],
                                                        synaps_vectors == null ? null : synaps_vectors[i - 1]);

                //net_layers_[i].InitSinaps(net_params_.InitSynaps);
            }


        }

        public (float, bool) DoForwardRun(NNIcon icon)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Error! Trying to calculate with neural network not initialized");
            }
            IsSaved = false;
            net_layers_[0].LoadIconData(icon);
            for (int i = 1; i < net_layers_.Length; i++)
                net_layers_[i].CalculateForward();
            return net_layers_[^1].CalcOutputError(icon);
        }

        public void DoErrBackPropagation()
        {
            for (int i = net_layers_.Length - 1; i > 0; i--)
                net_layers_[i].BackPropagate(net_params_.LearnRate, net_params_.Momentum);
        }

        public void Save(string path)
        {
            if (net_layers_ == null) throw new Exception("net_layers_ = NULL!");
            NNSerialisationInfo net_info = new()
            {
                synaps_vectors = new (float f_value_, float f_last_diff_)[net_layers_.Length - 1][][],
                parameters = net_params_
            };

            for (int i = 0; i < net_layers_.Length - 1; i++)
            {
                net_info.synaps_vectors[i] = new (float f_value_, float f_last_diff_)[net_layers_[i + 1].NeuronsCount][];
                for (int j = 0; j < net_layers_[i + 1].NeuronsCount; j++)
                {
                    net_info.synaps_vectors[i][j] = net_layers_[i + 1].GetNeuron(j).GetSynapseArray;
                }
            }

            XmlSerializer xml_net_params_ser = new(typeof(NNSerialisationInfo));

            using FileStream fs = File.Create(path);
            xml_net_params_ser.Serialize(fs, net_info);
            fs.Close();
            IsSaved = true;
        }

        public NNSerialisationInfo? LoadNeuralNetInfo(string path)
        {
            NNSerialisationInfo? net_info;
            XmlSerializer xml_net_params_ser = new(typeof(NNSerialisationInfo));
            using FileStream fs = File.OpenRead(path);
            net_info = (NNSerialisationInfo?)xml_net_params_ser.Deserialize(fs);
            fs.Close();
            return net_info;

        }

        public void ConPrintOut()
        {
            if (!IsInitialized)
            {
                Console.WriteLine("Neural Net hasn't been initialized yet!");
                return;
            }
            Console.WriteLine("Neural Network parameters are:");
            Console.WriteLine("\tUse bias neurons: " + (this.net_params_.DoAddBias ? "Yes" : "No"));
            Console.WriteLine("\tActivation function used: " + activateFunc.FuncName);
            Console.WriteLine("\tInitialization method for synaps: " + InitSynapsFunc.FuncName);
            Console.WriteLine($"\tLearn rate is: {net_params_.LearnRate} ; Momentum is {net_params_.Momentum}");
            Console.WriteLine($"\tNeural network has got {net_params_.LayersSize.Length} layers of which:");
            for (int i = 0; i < net_params_.LayersSize.Length; i++)
                Console.WriteLine($"\t\tLayer {i + 1} has {net_params_.LayersSize[i]} neurons");
        }

    }
}
