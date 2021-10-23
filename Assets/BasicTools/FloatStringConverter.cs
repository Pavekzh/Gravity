using System;
using System.Globalization;

namespace BasicTools
{
    public class FloatStringConverter : IDataPresenterConverter<float, string[]>
    {
        private static CultureInfo culture = new CultureInfo("en-US");

        public string[] ConvertDataToPresenter(float data)
        {
            return new string[1] { data.ToString(culture) };

        }

        public float ConvertPresenterToData(string[] presenter)
        {
            if (presenter[0] != "" && presenter[0] != "")
                return float.Parse(presenter[0], culture);
            else
                return 0;
        }
    }
}
