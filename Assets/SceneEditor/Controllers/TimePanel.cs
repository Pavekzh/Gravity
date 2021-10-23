using System;
using UnityEngine;
using UIExtended;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class TimePanel:MonoBehaviour
    {
        [SerializeField] private BindedInputField timeSet;
        [SerializeField] private BindedSlider timeSlider;
        [SerializeField] private ChangeImage buttonImage;
        [SerializeField] private UnityEngine.UI.Button button;

        private void Awake()
        {
            timeSet.Binding = TimeManager.Instance.TimeBinding;
            timeSlider.Binding = TimeManager.Instance.TimeBinding;
            button.onClick.AddListener(ChangeSimulationFlow);

            TimeManager.Instance.TimeStateChanged += SimulationFlowChanged;
        }

        private void SimulationFlowChanged(bool value, object source)
        {
            if (value == true)
                buttonImage.State = BasicTools.State.Default;
            else
                buttonImage.State = BasicTools.State.Changed;
        }

        private void ChangeSimulationFlow()
        {
            TimeManager.Instance.ChangePhysicsState();
        }


    }
}