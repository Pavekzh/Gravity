using System;
using UnityEngine;

namespace Assets.SceneSimulation
{
    public class PlanetMaterialProvider : IMaterialProvider
    {
        private const int textureResolution = 50;
        private Material loadedMaterial;
        private Gradient LandGradient = new Gradient();
        private Gradient WaterGradient = new Gradient();
        
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
                    colors[i] = WaterGradient.Evaluate(i / (textureResolution - 1f));
                }
                else
                {
                    colors[i] = LandGradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }

            }
            Texture2D planetTexture = new Texture2D(textureResolution * 2, 1, TextureFormat.RGBA32, false);
            planetTexture.SetPixels(colors);
            planetTexture.Apply();
            loadedMaterial.SetTexture("_texture", planetTexture);
        }

        public PlanetMaterialProvider(Gradient landGradient,Gradient waterGradient)
        {
            loadedMaterial = new Material(Resources.Load<Material>("Source/ViewModule/PlanetView/PlanetMaterial"));
            this.LandGradient = landGradient;
            this.WaterGradient = waterGradient;
        }

        public PlanetMaterialProvider()
        {
            loadedMaterial = Resources.Load<Material>("Source/ViewModule/PlanetView/PlanetMaterial");

            GradientColorKey startLand = new GradientColorKey(new Color(0.5f,0.5f,0.5f), 0);
            GradientColorKey endLand = new GradientColorKey(new Color(1f, 1f, 1f), 1);            
            LandGradient.colorKeys = new GradientColorKey[2] {startLand,endLand };

            GradientColorKey startWater = new GradientColorKey(new Color(0f, 0f, 0f), 0);
            GradientColorKey endWater = new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1);
            WaterGradient.colorKeys = new GradientColorKey[2] { startWater, endWater };

        }

        public Material GetMaterial()
        {
            return loadedMaterial;
        }
    }
}
