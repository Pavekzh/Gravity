using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;
using TMPro;

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
            planetView = GameObject.Instantiate(ValuesPanelTemplate.Instance.EmptyPrefab,setValuesPanel);
            planetView.name = PlanetData.Name + "View";
            planetView.anchorMin = new Vector2(0, 0);
            planetView.anchorMax = new Vector2(1, 1);
            planetView.pivot = new Vector2(0.5f, 0.5f);
            TMP_InputField nameField = GameObject.Instantiate(ValuesPanelTemplate.Instance.PlanetNameFieldPrefab, planetView);
            nameField.text = PlanetData.Name;
            UnityEngine.Events.UnityAction<string> nameChangedAction = new UnityEngine.Events.UnityAction<string>(NameChanged);
            nameField.onValueChanged.AddListener(nameChangedAction);

            float offset = ValuesPanelTemplate.Instance.StartMargin + nameField.GetComponent<RectTransform>().rect.height;
            foreach(ModuleController controller in controllers)
            {
                controller.CreateView(planetView, offset);
                offset += controller.ModuleOffset;
            }
        }

        private void NameChanged(string name)
        {
            this.PlanetData.Name = name;
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