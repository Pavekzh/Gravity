using System.Collections;
using System;
using UnityEngine;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class ValuesPanel : PanelController
    {
        [SerializeField] BasicTools.StateChanger visibleManager;
        [SerializeField] RectTransform containerTransform;

        private PlanetSelector selector;

        [Zenject.Inject]
        private void Construct(PlanetSelector selector)
        {
            this.selector = selector;           
            selector.SelectedPlanetChanged += SelectedPlanetChanged;
        }

        private void Start()
        {
            if(containerTransform == null)
                containerTransform = this.GetComponent<RectTransform>();
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
            selector.LessenSelected();
        }

        protected override void DoOpen()
        {
            visibleManager.State = BasicTools.State.Changed;
            selector.HighlightSelected();
            selector.SelectedPlanet.OpenView(this.containerTransform);
        }
    }
}