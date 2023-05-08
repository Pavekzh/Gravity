using System;
using UnityEngine;
using Assets.Services;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    class HighlighterController:StateChanger
    {
        private PlanetSelector selector;

        public override State State 
        {
            get => state;
            set
            {
                if(value == State.Default)
                {
                    LessenSelectedPlanet();
                    state = value;
                }
                else if(value == State.Changed)
                {
                    HighlightSelectedPlanet();
                    state = value;
                }
            }
        }

        [Zenject.Inject]
        private void Construct(PlanetSelector selector)
        {
            this.selector = selector;
        }

        public void HighlightSelectedPlanet()
        {
            selector.HighlightSelected();
        }

        public void LessenSelectedPlanet()
        {
            selector.LessenSelected();
        }
    }
}
