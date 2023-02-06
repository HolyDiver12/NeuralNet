using System;
using System.Numerics;
namespace NeuralNet
{
    class RollingAverage<T> where T : IAdditionOperators<T, T, T>, IConvertible // System.Numerics.IDivisionOperators<T,double,double>
    {
        int filled_by_ = 0;
        int position_ = 0;
        T[] array_;
        public RollingAverage(int size) => array_ = new T[size];
        public void AddValue(T val_to_add)
        {
            if (filled_by_ < array_.Length) //Если массив еще не заполнен
            {
                array_[filled_by_++] = val_to_add;
            }
            else
            {
                array_[position_++] = val_to_add;
            }
            if (position_ == array_.Length)
                position_ = 0;
        }
        public double Average
        {
            get
            {
                T average = array_[0];
                for (int i = 1; i < filled_by_; i++) average = average + array_[i];
                return average.ToDouble(null) / (double)filled_by_;
            }
        }
    }

}

