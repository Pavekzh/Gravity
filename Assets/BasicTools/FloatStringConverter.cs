using System;
using System.Globalization;

namespace BasicTools
{
    public class FloatStringConverter : IDataPresenterConverter<float, string[]>
    {
        public string[] ConvertDataToPresenter(float data)
        {
            return new string[1] { data.ToString() };

        }

        public float ConvertPresenterToData(string[] presenter)
        {
            if (presenter[0] != "" && presenter[0] != "")
                return float.Parse(presenter[0]);
            else
                return 0;
        }
    }
}
