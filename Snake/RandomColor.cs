using System;
using System.Reflection;
using System.Windows.Media;

namespace Snake
{

    class RandomColor
    {

        public Brush PickBrush()
        { //LOSOWANIE KOLORU
            Brush result = Brushes.Transparent;
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            if (result == Brushes.White || result == Brushes.Black) return PickBrush();
                else return result;        
        }
    }
}
