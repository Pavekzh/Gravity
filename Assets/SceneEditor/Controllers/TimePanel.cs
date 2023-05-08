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

        private TimeFlow timeFlow;

        [Zenject.Inject]
        private void Construct(TimeFlow timeFlow)
        {
            SetTimeFlow(timeFlow);
        }

        private void SetTimeFlow(TimeFlow timeFlow)
        {            
            timeSet.Binding = timeFlow.TimeBinding;
            timeSlider.Binding = timeFlow.TimeBinding;
            timeFlow.TimeBinding.ForceUpdate();
            timeFlow.TimeStateChanged += SimulationFlowChanged;
            this.timeFlow = timeFlow;
        }

        private void Awake()
        {
            button.onClick.AddListener(ChangeSimulationFlow);
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
            timeFlow.ChangePhysicsState();
        }


    }
}