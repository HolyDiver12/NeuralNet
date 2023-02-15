using NeuralNet.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    public class NNData
    {
        public bool is_prepared_ { get; private set; }
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
            is_prepared_ = false;
            return icons_count;
        }

        public void Prepare(PrepareType prep_type)
        {
            if (is_prepared_) return;
            PrepIconsFuncCollection func_collection = new();
            IPrepareIcons prepIconsFunc = func_collection.GetPrepareIconsClass(prep_type);
            prepIconsFunc.PrepareIcons(icons_list_);
            is_prepared_ = true;
        }
    }
}
