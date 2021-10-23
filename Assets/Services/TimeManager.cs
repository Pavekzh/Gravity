using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace Assets.Services
{
    public class TimeManager : BasicTools.Singleton<TimeManager>
    {
        private float defaultFixedDeltaTime;

        public Binding<float> TimeBinding { get; private set; }
        [SerializeField] private float timeScale = 1;
        [SerializeField] private bool simulationState = true;
        [SerializeField] private float maxTimeScale = 2;

        public float TimeScale { get => timeScale; }
        public bool SimulationState { get => simulationState; }

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

        //method for external pause game
        public void StopPhysics()
        {
            TimeBinding.ChangeValue(0, this);
            LocalStopPhysics();
        }
        //method for external resume game
        public void ResumePhysics()
        {
            TimeBinding.ChangeValue(Time.timeScale, this);
            LocalResumePhysics();
        }

        public void ChangePhysicsState()
        {
            if (simulationState)
            {
                StopPhysics();
            }
            else
            {
                ResumePhysics();
            }
        }

        private bool ValidateTimeChanges(float value, object source)
        {
            if (value <= maxTimeScale)
            {
                return true;
            }
            else
            {
                ResetTimeScale(maxTimeScale, null);
                TimeBinding.ChangeValue(maxTimeScale, this);
                return false;
            }
        }

        protected  override void Awake()
        {
            base.Awake();
            TimeBinding = new Binding<float>();
            defaultFixedDeltaTime = Time.fixedDeltaTime;
            TimeBinding.ValueChanged += ResetTimeScale;
            TimeBinding.ValidateValue += ValidateTimeChanges;
            ResetTimeScale(timeScale);

            if (simulationState == false)
            {
                LocalStopPhysics();
            }
            else
            {
                LocalResumePhysics();
            }
        }

        private void ResetTimeScale(float value)
        {
            if (value != 0 && timeScale != 0)
            {
                timeScale = value;
                Time.timeScale = value;
                Time.fixedDeltaTime = defaultFixedDeltaTime * value;
            }
            else if (value != 0 && timeScale == 0)
            {
                timeScale = value;
                Time.timeScale = value;
                Time.fixedDeltaTime = defaultFixedDeltaTime * value;
                LocalResumePhysics();
            }
            else
            {
                LocalStopPhysics();
            }

        }

        private void ResetTimeScale(float value, object source)
        {
            if (source != (System.Object)this)
            {
                if (value != 0 && timeScale != 0)
                {
                    timeScale = value;
                    Time.timeScale = value;
                    Time.fixedDeltaTime = defaultFixedDeltaTime * value;
                }
                else if (value != 0 && timeScale == 0)
                {
                    timeScale = value;
                    Time.timeScale = value;
                    Time.fixedDeltaTime = defaultFixedDeltaTime * value;
                    LocalResumePhysics();
                }
                else
                {
                    LocalStopPhysics();
                }
            }
        }

        //method used when time stoped by setting TimeScale to 0
        private void LocalStopPhysics()
        {
            timeStateChanged?.Invoke(false,this);
            
            timeScale = 0;
            simulationState = false;
        }
        //method used when time resume by setting TimeScale not to 0
        private void LocalResumePhysics()
        {
            timeStateChanged?.Invoke(true,this);
            timeScale = Time.timeScale;
            simulationState = true;
        }
    }
}
