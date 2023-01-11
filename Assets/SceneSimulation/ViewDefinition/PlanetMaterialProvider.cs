using System;
using UnityEngine;

namespace Assets.SceneSimulation
{
    [Serializable]
    public class PlanetMaterialProvider:ICloneable
    {
        private const int textureResolution = 50;
        private Material loadedMaterial;
        private Gradient landGradient = new Gradient();
        private Gradient waterGradient = new Gradient();

        public Gradient LandGradient { get => landGradient; set => landGradient = value; }
        public Gradient WaterGradient { get => waterGradient; set => waterGradient = value; }

        public void UpdateMinMax(Vector2 MinMax)
        {
            loadedMaterial.SetVector("_elevationMinMax", MinMax);
            RecalculateTexture();
        }

        public void RecalculateTexture()
        {
            Color[] colors = new Color[textureResolution * 2];
            for (int i = 0; i < textureResolution * 2; i++)
            {
                if (i < textureResolution)
                {
                    colors[i] = waterGradient.Evaluate(i / (textureResolution - 1f));
                }
                else
                {
                    colors[i] = landGradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }

            }
            Texture2D planetTexture = new Texture2D(textureResolution * 2, 1, TextureFormat.RGBA32, false);
            planetTexture.SetPixels(colors);
            planetTexture.Apply();
            loadedMaterial.SetTexture("_texture", planetTexture);
        }

        public PlanetMaterialProvider(Gradient landGradient,Gradient waterGradient)
        {
            loadedMaterial = new Material(Resources.Load<Material>("ViewModule/PlanetView/PlanetMaterial"));
            this.landGradient = landGradient;
            this.waterGradient = waterGradient;
        }

        public PlanetMaterialProvider()
        {
            loadedMaterial = new Material(Resources.Load<Material>("ViewModule/PlanetView/PlanetMaterial"));

            GradientColorKey startLand = new GradientColorKey(new Color(0.5f,0.5f,0.5f), 0);
            GradientColorKey endLand = new GradientColorKey(new Color(1f, 1f, 1f), 1);            
            landGradient.colorKeys = new GradientColorKey[2] {startLand,endLand };

            GradientColorKey startWater = new GradientColorKey(new Color(0f, 0f, 0f), 0);
            GradientColorKey endWater = new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1);
            waterGradient.colorKeys = new GradientColorKey[2] { startWater, endWater };

        }

        public Material GetMaterial()
        {
            return loadedMaterial;
        }

        public void Highlight()
        {
            loadedMaterial.SetFloat("_Enable",1);
        }

        public void Lessen()
        {
            loadedMaterial.SetFloat("_Enable",0);
        }

        public object Clone()
        {
            PlanetMaterialProvider materialProvider = new PlanetMaterialProvider(this.landGradient, this.waterGradient);
            return materialProvider;
        }
    }
}
