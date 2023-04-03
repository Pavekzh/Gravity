using System;
using UnityEngine;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    class GravityRatioController:MonoBehaviour
    {
        [SerializeField] BindedInputField gravityInputField;

        protected void Start()
        {
            gravityInputField.Binding = Services.GravityManager.Instance.GravityBinding;
            gravityInputField.Binding.ForceUpdate();
        }

    }
}
