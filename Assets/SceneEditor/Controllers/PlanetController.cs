using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class PlanetController:MonoBehaviour
    {
        public PlanetData PlanetData { get; set; }
        private RectTransform planetView;
        private List<ModuleController> controllers = new List<ModuleController>();

        public void AddModule(ModuleData moduleData)
        {
            controllers.Add(new ModuleController(moduleData));
        }

        public void OpenView(RectTransform setValuesPanel)
        {
            CloseView();
            planetView = GameObject.Instantiate(ModuleViewTemplate.Instance.EmptyPrefab,setValuesPanel);
            planetView.name = PlanetData.Name + "View";
            planetView.anchorMin = new Vector2(0, 0);
            planetView.anchorMax = new Vector2(1, 1);
            planetView.pivot = new Vector2(0.5f, 0.5f);

            float offset = ModuleViewTemplate.Instance.StartMargin;
            foreach(ModuleController controller in controllers)
            {
                controller.CreateView(planetView, offset);
                offset -= controller.ModuleOffset + ModuleViewTemplate.Instance.PropertiesMargin;
            }
        }

        public void CloseView()
        {
            if(planetView != null)
            {
                GameObject.Destroy(planetView.gameObject);
            }
        }
    }
}