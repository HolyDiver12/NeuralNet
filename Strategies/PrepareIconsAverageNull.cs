using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies 
{
    public class PrepareIconsAverageNull : IPrepareIcons
    {
        public void PrepareIcons(List<NNIcon> icons_list)
        {
            if (icons_list.Count == 0) return;

            float min;
            float max;
            float fmax;

            min = max = icons_list[0].data_[0];
            foreach (var icon in icons_list)
            {
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    min = icon.data_[i] < min ? icon.data_[i] : min;
                    max = icon.data_[i] > max ? icon.data_[i] : max;
                }
            }

            fmax = max - min;
            foreach (var icon in icons_list)
            {
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    icon.data_[i] = (icon.data_[i] - min - (fmax / 2.0F)) / (fmax / 2.0F);
                }
            }
        }
    }
}
