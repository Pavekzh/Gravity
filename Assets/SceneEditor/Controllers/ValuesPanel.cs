using System.Collections;
using System;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class ValuesPanel : PanelController
    {
        [SerializeField] BasicTools.StateChanger visibleManager;
        [SerializeField] RectTransform containerTransform;

        private void Start()
        {
            if(containerTransform == null)
                containerTransform = this.GetComponent<RectTransform>();

            Services.PlanetSelectSystem.Instance.SelectedPlanetChanged += SelectedPlanetChanged;
        }

        private void SelectedPlanetChanged(object sender,PlanetController planet)
        {
            if(visibleManager.State == BasicTools.State.Changed)
            {
                this.Close();
                this.Open();
            }
        }

        public void ChangeState()
        {
            if (visibleManager.State == BasicTools.State.Default)
                Open();
            else if (visibleManager.State == BasicTools.State.Changed)
                Close();
        }

        public override void Close()
        {
            foreach(Transform child in containerTransform)
            {
                GameObject.Destroy(child.gameObject);
            }
            visibleManager.State = BasicTools.State.Default;
        }

        public override void Open()
        {
            EditorController.Instance.Panel = this;
            visibleManager.State = BasicTools.State.Changed;
            Services.PlanetSelectSystem.Instance.SelectedPlanet.OpenView(this.containerTransform);
        }
    }
}