using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    public class NNData
    {
        private bool prepared_ = false;
        public bool is_prepared_ { get => prepared_; }
        public List<NNIcon> icons_list_ = new();  // Список иконок

        public int ReadFromFile(string file_name)
        {
            string? string_read;
            int icons_count = 0;
            using (StreamReader icons_file = new(file_name))
            {
                while ((string_read = icons_file.ReadLine()) != null)
                {
                    int values_count = 0;
                    uint buff = 0;
                    icons_list_.Add(new NNIcon());
                    string[] tokens = string_read.Split(',', StringSplitOptions.TrimEntries);
                    foreach (var token in tokens)
                    {
                        if (!uint.TryParse(token, out buff))
                        {
                            Console.WriteLine("Error while parsing " + token + " to a string");
                        }
                        if (values_count < Program.NN_ICON_SIZE) //Значения пикселей
                            icons_list_[icons_count].data_[values_count++] = buff;
                        else
                            icons_list_[icons_count].val_ = buff; //Значение иконки 

                    }
                    icons_count++;
                }
            }
            //Console.WriteLine(" " + icons_count +  " icons read");
            prepared_ = false;
            return icons_count;
        }

        public void Prepare(PrepareType prep_type)
        {
            if (is_prepared_) return;
            if (icons_list_.Count == 0) return;

            float min;
            float max;
            float wa_icon;
            float wa = 0F;
            float fmax;

            min = max = icons_list_[0].data_[0];
            foreach (var icon in icons_list_)
            {
                wa_icon = 0f;
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    min = icon.data_[i] < min ? icon.data_[i] : min;
                    max = icon.data_[i] > max ? icon.data_[i] : max;
                    wa_icon += icon.data_[i];
                }
                wa_icon /= Program.NN_ICON_SIZE;
                wa += wa_icon / icons_list_.Count;
            }
            wa -= min;

            float f_divider = (max - wa) > (wa - min) ? max - wa : wa - min;
            foreach (var icon in icons_list_)
            {
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    if ((prep_type & PrepareType.PREP_UNSIG) == PrepareType.PREP_UNSIG)
                    {
                        icon.data_[i] = (icon.data_[i] - min) / (max - min);
                    }
                    else if ((prep_type & PrepareType.PREP_ZERO_AV) == PrepareType.PREP_ZERO_AV)
                    {
                        fmax = max - min;
                        icon.data_[i] = (icon.data_[i] - min - (fmax / 2.0F)) / (fmax / 2.0F);
                    }
                    else if ((prep_type & PrepareType.PREP_ZERO_WAV) == PrepareType.PREP_ZERO_WAV)
                    {
                        icon.data_[i] = (icon.data_[i] - min - wa) / f_divider;
                    }
                    else
                        return;

                }
            }
            prepared_ = true;
        }
    }
}
