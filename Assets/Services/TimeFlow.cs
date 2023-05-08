using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;
using BasicTools.Validation;

namespace Assets.Services
{
    public class TimeFlow:MonoBehaviour
    {
        private float defaultFixedDeltaTime;
        private string timeLockerName;

        public Binding<float> TimeBinding { get; private set; } 

        [SerializeField] private bool simulationState = true;
        [SerializeField] private float maxTimeScale = 2;

        private bool savedSimulationState;

        public bool SimulationState { get => simulationState; }
        public bool TimeFlowLocked { get; private set; } = false;

        private ValueChangedHandler<bool> timeStateChanged;
        public event ValueChangedHandler<bool> TimeStateChanged
        {
            add
            {
                timeStateChanged += value;
                value(this.simulationState, this);
            }
            remove
            {
                timeStateChanged -= value;
            }
        }

        public void Initialize()
        {
            TimeBinding = new Binding<float>();
            defaultFixedDeltaTime = Time.fixedDeltaTime;
            TimeBinding.ValueChanged += ResetTimeScale;
            TimeBinding.ValidationRules.Add(new ValidationRule<float>(maxTimeScale, ValidateTimeChanges));

            if (simulationState == false)
            {
                SetTimeFlow(false);
            }
            else
            {
                SetTimeFlow(true);
            }
        }

        public void SetPhysicsState(bool state)
        {
            if(state)
            {
                TimeBinding.ChangeValue(Time.timeScale, this);
                SetTimeFlow(true);
            }
            else
            {
                TimeBinding.ChangeValue(0, this);
                SetTimeFlow(false);
            }
        }

        public void ChangePhysicsState()
        {
            SetPhysicsState(!simulationState);
        }

        public void LockTimeFlow(string lockerName)
        {
            if (!TimeFlowLocked)
            {
                timeLockerName = lockerName;
                savedSimulationState = simulationState;
                SetPhysicsState(false);
                TimeFlowLocked = true;
            }
        }

        public void UnlockTimeFlow()
        {
            if (TimeFlowLocked)
            {
                TimeFlowLocked = false;
                SetPhysicsState(savedSimulationState);
                savedSimulationState = simulationState;
            }
        }

        private bool ValidateTimeChanges(float value)
        {
            return value <= maxTimeScale;
        }



        private void ResetTimeScale(float value)
        {
            if (Mathf.Approximately(value, 0))
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = defaultFixedDeltaTime;
                SetTimeFlow(false);
            }
            else
            {                    
                Time.timeScale = value;
                Time.fixedDeltaTime = defaultFixedDeltaTime * value;
                if (!simulationState)
                {
                    SetTimeFlow(true);
                }
            }

        }

        private void ResetTimeScale(float value, object source)
        {
            if (source != (System.Object)this)
            {
                ResetTimeScale(value);
            }
        }

        private void SetTimeFlow(bool state)
        {
            if (!TimeFlowLocked)
            {
                if (simulationState != state)
                {
                    timeStateChanged?.Invoke(state, this);
                    simulationState = state;
                }
            }
            else
            {
                if (this.simulationState == true)
                    TimeBinding.ChangeValue(Time.timeScale, this);
                else
                    TimeBinding.ChangeValue(0, this);
                MessagingSystem.Instance.ShowMessage("Time flow locked by " + timeLockerName,this);
            }

        }

    }
}
