using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasicTools
{
    public class NoiseSetsStringConverter : IDataPresenterConverter<NoiseSettings, string[]>
    {
        string[] IDataPresenterConverter<NoiseSettings, string[]>.ConvertDataToPresenter(NoiseSettings data)
        {
            return new string[] { data.Frequency.ToString(), data.Lacunarity.ToString(), data.Persistence.ToString(), data.Center.x.ToString(), data.Center.y.ToString(), data.Center.z.ToString(), data.Octaves.ToString(), data.MinValue.ToString(), data.Strength.ToString() };
        }

        NoiseSettings IDataPresenterConverter<NoiseSettings, string[]>.ConvertPresenterToData(string[] presenter)
        {
            NoiseSettings settings;

            float Frequency = 0;
            float Lacunarity = 0;
            float Persistence = 0;
            Vector3 Center = Vector3.zero;
            float x = 0;
            float y = 0;
            float z = 0;

            int Octaves = 0;
            float MinValue = 0;
            float Strength = 0;

            if (presenter[0] != "" && presenter[0] != "-")
                Frequency = float.Parse(presenter[0]);
            if (presenter[1] != "" && presenter[1] != "-")
                Lacunarity = float.Parse(presenter[1]);
            if (presenter[2] != "" && presenter[2] != "-")
                Persistence = float.Parse(presenter[2]);

            if (presenter[3] != "" && presenter[3] != "-")
                x = float.Parse(presenter[3]);
            if (presenter[4] != "" && presenter[4] != "-")
                y = float.Parse(presenter[4]);
            if (presenter[5] != "" && presenter[5] != "-")
                z = float.Parse(presenter[5]);

            if (presenter[6] != "" && presenter[6] != "-")
                Octaves = int.Parse(presenter[6]);
            if (presenter[7] != "" && presenter[7] != "-")
                MinValue = float.Parse(presenter[7]);
            if (presenter[8] != "" && presenter[8] != "-")
                Strength = float.Parse(presenter[8]);

            Center = new Vector3(x, y, z);
            settings = new NoiseSettings(Frequency, Lacunarity, Persistence, Center, Octaves, MinValue, Strength);
            return settings;
        }
    }
}
