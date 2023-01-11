using System;
using UnityEngine;
using Assets.Services;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    class SelectedPlanetController:StateChanger
    {
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

        public void HighlightSelectedPlanet()
        {
            PlanetSelectSystem.Instance.HighlightSelected();
        }

        public void LessenSelectedPlanet()
        {
            PlanetSelectSystem.Instance.LessenSelected();
        }
    }
}
