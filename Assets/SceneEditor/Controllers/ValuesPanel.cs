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

        protected override void DoClose()
        {
            foreach(Transform child in containerTransform)
            {
                GameObject.Destroy(child.gameObject);
            }
            visibleManager.State = BasicTools.State.Default;
        }

        protected override void DoOpen()
        {
            visibleManager.State = BasicTools.State.Changed;
            Services.PlanetSelectSystem.Instance.SelectedPlanet.OpenView(this.containerTransform);
        }
    }
}