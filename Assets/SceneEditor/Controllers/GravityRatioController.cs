using System;
using UnityEngine;
using UIExtended;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    class GravityRatioController:MonoBehaviour
    {
        [SerializeField] BindedInputField gravityInputField;

        private GravityComputer gravityComputer;

        [Zenject.Inject]
        private void Construct(GravityComputer gravityComputer)
        {
            this.gravityComputer = gravityComputer;
        }

        protected void Start()
        {
            gravityInputField.Binding = gravityComputer.GravityBinding;
            gravityInputField.Binding.ForceUpdate();
        }

    }
}
