using System.Globalization;
using UnityEngine;

namespace BasicTools
{
    public class VectorStringConverter : IDataPresenterConverter<Vector2, string[]>
    {
        private static CultureInfo culture = new CultureInfo("en-US");

        public string[] ConvertDataToPresenter(Vector2 data)
        {
            return new string[2] { data.x.ToString(culture), data.y.ToString(culture) };
        }

        public Vector2 ConvertPresenterToData(string[] presenter)
        {
            float x = 0;
            float y = 0;

            if (presenter[0] != "" && presenter[0] != "-")
                x = float.Parse(presenter[0], culture);
            if (presenter[1] != "" && presenter[1] != "-")
                y = float.Parse(presenter[1], culture);

            return new Vector2(x, y);
        }
    }
}
