using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Strategies
{
    public class PrepareIconsWeightedAverageNull : IPrepareIcons
    {
        public void PrepareIcons(List<NNIcon> icons_list)
        {
            if (icons_list.Count == 0) return;

            float min;
            float max;
            float wa_icon;
            float wa = 0F;

            min = max = icons_list[0].data_[0];
            foreach (var icon in icons_list)
            {
                wa_icon = 0f;
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    min = icon.data_[i] < min ? icon.data_[i] : min;
                    max = icon.data_[i] > max ? icon.data_[i] : max;
                    wa_icon += icon.data_[i];
                }
                wa_icon /= Program.NN_ICON_SIZE;
                wa += wa_icon / icons_list.Count;
            }
            wa -= min;

            float f_divider = (max - wa) > (wa - min) ? max - wa : wa - min;
            foreach (var icon in icons_list)
            {
                for (int i = 0; i < Program.NN_ICON_SIZE; i++)
                {
                    icon.data_[i] = (icon.data_[i] - min - wa) / f_divider;
                }
            }

        }
    }
}
