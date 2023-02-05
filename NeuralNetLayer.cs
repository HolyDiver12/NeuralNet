using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNet.Strategies;

namespace NeuralNet
{
    public class NeuralNetLayer
    {

        public readonly LayerPosition Position;
        //public readonly int Index = 0;
        public readonly int LayerSize = 0;
        public readonly Neuron[] neurons_array_;
        private readonly NeuralNetLayer? previous_layer = null;


        //-------------------------
        // Создает слой размером size и передает референс на предыдущий слой 
        // Если do_add_bias = true - добавляет к слою нейрон смещения сверх размера size
        public NeuralNetLayer(int size, LayerPosition pos, ActivateClass act_func,
                                bool do_add_bias = false, NeuralNetLayer? previous = null,
                                (float f_value_, float f_last_diff_)[][]? synapsVectors = null)
        {
            Position = pos;
            if (size == 0) throw new InvalidOperationException("Trying to create a layer of zero size");
            LayerSize = size;
            previous_layer = previous;

            neurons_array_ = new Neuron[do_add_bias ? (size + 1) : size];
            for (int i = 0; i < size; i++) // для всех кроме bias
            {
                neurons_array_[i] = new Neuron(previous == null ? 0 : previous.neurons_array_.Length, i, act_func, synapsVectors?[i]);
            }
            if (do_add_bias) //если нужно добавляем bias со значением 1
            {
                neurons_array_[size] = new Neuron(0, size, act_func, null);
                neurons_array_[size].MakeBias();
            }

        }
        //---------------------------------------
        // Инициализирует значения синапсов слоя выбранным методом
        // method - битовое поле с одним из вариантов инициализации
        //
        public void InitSinaps(InitSynapsMethod method = InitSynapsMethod.INIT_KAIMING)
        {
            if (Position == LayerPosition.Input) return; //синапсов нет
            if ((method & InitSynapsMethod.INIT_KAIMING) == InitSynapsMethod.INIT_KAIMING)
            {
                foreach (var neuron in neurons_array_)
                {
                    neuron.InitSynapsKaiming(previous_layer == null ? 0
                                            : previous_layer.LayerSize);
                }
            }
            else { throw new InvalidOperationException("ERROR! Only Kaiming so far!"); }
        }


        //-----------------------------------------
        // Загружает данные иконки в нейроны слоя с соответствующими проверками
        //
        public void LoadIconData(in NNIcon icon)
        {
            if (Position != LayerPosition.Input)
            {
                throw new InvalidOperationException("Trying to load data to the hidden layer!");
            }
            if (LayerSize != Program.NN_ICON_SIZE)
            {
                throw new InvalidOperationException("Layer size does not match the same of Icon!");
            }
            for (int i = 0; i < LayerSize; i++)//Для всех кроме bias
            {
                neurons_array_[i].Val = icon.data_[i];
            }

        }

        //-------------------------------------------
        // Расчитывает значения нейронов слоя по данным предыдущего
        //
        public void CalculateForward()
        {
            if (Position == LayerPosition.Input)
            {
                return; // Считать первый слой не нужно
            }
            for (int i = 0; i < LayerSize; i++) // не обрабатываем bias 
            {
                neurons_array_[i].CalcValue(previous_layer); // проверка на null выше
            }
        }

        //---------------------------------------------
        // Расчитывает значения ошибки нейронов выходного слоя, считает среднее квадратов отклонений
        // значений нейронов от правильного ответа, а также определяет угадано ли значение иконки
        // возвращает (среднее квадратов отклонений, угадано значение?)
        // ы
        public (float, bool) CalcOutputError(in NNIcon icon)
        {
            if (Position != LayerPosition.Out)
            {
                throw new InvalidOperationException("Error! Trying to calculate out error for hidden or input layer");
            }

            float[] right_unswer = new float[this.LayerSize];
            right_unswer[icon.val_] = 1F; // остальные 0 при автоматической инициализации

            float output_err = 0F;
            var find_max = (index: 0, max_val: 0F);
            for (int i = 0; i < LayerSize; i++)
            {
                neurons_array_[i].CalcOutputError(right_unswer[i]);
                output_err += (float)Math.Pow(right_unswer[i] - neurons_array_[i].Val, 2F);
                if (find_max.max_val < neurons_array_[i].Val)
                {
                    find_max = (i, neurons_array_[i].Val);
                }
            }
            return (output_err /= LayerSize, (find_max.index == icon.val_));
        }

        public void BackPropagate(float learn_rate, float momentum)
        {
            if (Position == LayerPosition.Input)
            {
                return; //Ничего делать не нужно
            }
            foreach (var prev_layer_neuron in previous_layer.neurons_array_)  //Проваерка уже сделана выше
            {
                float f_error = 0.0F;
                float f_gradient = 0.0F;
                float f_correction = 0.0F;

                foreach (var this_layer_neuron in neurons_array_)
                {
                    if (this_layer_neuron.IsBias) continue; // bias нейрон не обрабатываем
                    f_error += this_layer_neuron.Error * this_layer_neuron.GetSynapsValForNeuron(prev_layer_neuron.Index);
                }

                prev_layer_neuron.Error = f_error * prev_layer_neuron.ActivateStrat.ActivateDerivateError(prev_layer_neuron.Val); ;

                foreach (var this_layer_neuron in neurons_array_)
                {
                    if (this_layer_neuron.IsBias) continue; // bias нейрон не обрабатываем
                    f_gradient = this_layer_neuron.Error * prev_layer_neuron.Val;
                    f_correction = learn_rate * f_gradient + momentum * this_layer_neuron.GetSynapsDiffForNeuron(prev_layer_neuron.Index);

                    this_layer_neuron.CorrectSynapsForNeuron(prev_layer_neuron.Index, f_correction);
                }
            }
        }
    }



}
