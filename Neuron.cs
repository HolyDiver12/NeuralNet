using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNet.Strategies;

namespace NeuralNet
{
    //----------------------------------------------------
    // Класс инкапсулирует данные нейрона и вектор синапсов, связывающих
    // нейрон со всеми нейронами предыдущего слоя (для первого слоя вектора синапсов пустые)
    // Умеет инициализировать свои синапсы
    // Рассчитывать свое значение по значениям неронов предыдущего слоя и значениям синапсов
    // Имеет функции активации и расчета производной
    //
    public class Neuron
    {
        public (float f_value_, float f_last_diff_)[]? synaps_array_;//Массив синапсов, соединяющих нейрон с нейронами предыдущего слоя

        private float f_value_; //Значение нейрона
        public float Val    // Не дает присвоить значение нейрону сдвига 
        {
            get => f_value_;
            set
            {
                f_value_ = !IsBias ? value : throw new InvalidOperationException("Trying to change Val of a Bias neuron");
            }
        }
        public float Error { get; set; } //Ошибка нейрона
        public readonly int Index;  //Индекс нейрона (присваивается при создании)
        public bool IsBias { get; private set; } //Признак нейрона сдвига для чтения снаружи класса
        public readonly ActivateClass ActivateStrat; //Стратегия функции активации и ее производной

        //-----------------------------
        //Инициализирует данные нейрона c индексом index в 0 и создает вектор синапсов размером sinaps_size
        //
        public Neuron(int synaps_size, int index, ActivateClass act_strat, (float f_value_, float f_last_diff_)[]? synaps = null)
        {
            Index = index;
            IsBias = false;
            f_value_ = 0F;
            ActivateStrat = act_strat;

            synaps_array_ =
                synaps == null ?
                    new (float f_value_, float f_last_diff_)[synaps_size]
                :
                    synaps;
        }

        //-----------------------------
        //Преобразует нейрон в нейрон сдвига
        //
        public void MakeBias()
        {
            Val = 1.0F;
            IsBias = true;
        }

        //------------------------------
        // Возвращает значение синапса, соединяющего
        // этот нейрон с нейроном номер neuron_index предыдущего слоя
        //
        public float GetSynapsValForNeuron(int neuron_index) => synaps_array_[neuron_index].f_value_;


        //------------------------------
        // Возвращает значение последнего изменения синапса, соединяющего
        // этот нейрон с нейроном номер neuron_index предыдущего слоя
        //
        public float GetSynapsDiffForNeuron(int neuron_index) => synaps_array_[neuron_index].f_last_diff_;

        //------------------------------
        // Корректирует значение синапса, соединяющего
        // этот нейрон с нейроном номер neuron_index предыдущего слоя
        // на значение correction и переписывает значение последнего
        // изменения этого нейрона
        //
        public void CorrectSynapsForNeuron(int neuron_index, float correction)
        {
            synaps_array_[neuron_index].f_value_ += correction;
            synaps_array_[neuron_index].f_last_diff_ = correction;
        }

        //-----------------------------------------
        // Инициализирует синапсы нейрона методом Кайминга
        //
        public void InitSynapsKaiming(int layer_size)
        {
            /*
            foreach( var synaps in synaps_array_)
            {
                synaps.f_value_ = (float)Program.rand.NextDouble() * (float)Math.Sqrt(2.0F / layer_size);
            }*/
            for (int i = 0; i < synaps_array_.Length; i++)
            {
                synaps_array_[i].f_value_ = (float)Program.rand.NextDouble() * (float)Math.Sqrt(2.0F / layer_size);
            }

        }

        //---------------------------------------------
        // Вычисляет значение нейрона суммируя произведения значений синапсов
        // и значений нейронов предыдущего слоя и активирует полученную сумм
        // выбранной функцией активации
        //
        public void CalcValue(in NeuralNetLayer previous_layer)
        {
            if (IsBias) return; // Вычислять bias не нужно
            /*if(synaps_array_.Length != previous_layer.neurons_array_.Length)
            {
                Console.WriteLine("synaps vectors of the "+ (previous_layer.index_+1)
                    + " are not of the same size as the layer No " + previous_layer.index_);
            }*/
            float value_to_activate = 0F;
            for (int i = 0; i < synaps_array_.Length; i++)
            {
                value_to_activate += synaps_array_[i].f_value_ *
                                    previous_layer.neurons_array_[i].Val;
            }
            Val = ActivateStrat.ActivateValue(value_to_activate);
        }

        //-------------------------------------------
        // Расчитывает ошибку нейрона выходного слоя по правильному значению f_true_value 
        //
        public void CalcOutputError(float f_true_value) => Error = (f_true_value - Val) * ActivateStrat.ActivateDerivateError(Val);


    }

}
