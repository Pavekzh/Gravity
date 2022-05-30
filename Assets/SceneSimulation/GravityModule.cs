using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneSimulation
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityModule : Module
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private GravityInteractor data;

        public GravityInteractor Data { get => data; }
        public float Mass 
        { 
            get
            {
                return Rigidbody.mass; 
            } 
            set 
            {
                this.massBinding.ChangeValue(value, this);
                this.Rigidbody.mass = value;
                this.data.Mass = value;
            } 
        }
        public Vector3 Velocity
        {
            get
            {
                if (IsSimulationEnabled)
                {                    
                    return Rigidbody.velocity;
                }
                else
                {
                    return savedVelocity;
                }
            }
            private set
            {
                if (IsSimulationEnabled)
                {
                    this.Rigidbody.velocity = value;
                }
                else
                {             
                    this.savedVelocity = value;
                }
                this.data.Velocity = value.GetVectorXZ();
                this.velocityBinding.ChangeValue(value.GetVectorXZ(), this);
            }
        }
        public Vector3 Position 
        { 
            get 
            {
                return transform.position; 
            } 
            set 
            {
                this.positionBinding.ChangeValue(value.GetVectorXZ(), this);          
                this.transform.position = value;
                this.data.Position = value.GetVectorXZ();
            } 
        }
        public bool IsSimulationEnabled { get; private set; }

        private Guid guid;
        private Vector3 savedVelocity;
        private Rigidbody Rigidbody
        {
            get
            {
                if(rigidbody == null)
                {
                    rigidbody = this.GetComponent<Rigidbody>();
                }
                return rigidbody;
            }
        }
        private ConvertibleBinding<Vector2, string[]> positionBinding;
        private ConvertibleBinding<Vector2, string[]> velocityBinding;
        private ConvertibleBinding<float, string[]> massBinding;

        private void Awake()
        {
            Services.TimeManager.Instance.TimeStateChanged += UpdateSimulationState;
        }

        private void OnDestroy()
        {
            TimeManager.Instance.TimeStateChanged -= UpdateSimulationState;

            if(positionBinding != null)
                positionBinding.ValueChanged -= SetPosition;
            if(velocityBinding != null)
                velocityBinding.ValueChanged -= SetVelocity;
            if(massBinding != null)
                massBinding.ValueChanged -= SetMass;
        }

        private void FixedUpdate()
        {
            if (IsSimulationEnabled)
            {
                positionBinding.ChangeValue(Position.GetVectorXZ(), this);
                velocityBinding.ChangeValue(Velocity.GetVectorXZ(), this);
                data.Velocity = Velocity.GetVectorXZ();
                data.Position = Position.GetVectorXZ();

                Vector3 force = GravityManager.Instance.ComputeForce(Data);
                Rigidbody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);

                positionBinding.ChangeValue(Position.GetVectorXZ(), this);
                velocityBinding.ChangeValue(Velocity.GetVectorXZ(), this);
                data.Velocity = Velocity.GetVectorXZ();
                data.Position = Position.GetVectorXZ();
            }
        }

        //reaction to time manager stop/resume
        public void UpdateSimulationState(bool state,object sender)
        {
            IsSimulationEnabled = state;
            if (state == false && Rigidbody.isKinematic == false)
            {
                savedVelocity = Rigidbody.velocity;
                Rigidbody.isKinematic = true;
            }
            else if (state == true && Rigidbody.isKinematic == true)
            {
                Rigidbody.isKinematic = false;
                Rigidbody.velocity = savedVelocity;
            }
        }

        //initialization
        public void SetModuleData(GravityModuleData moduleData)
        {                     
            positionBinding = moduleData.PositionProperty.Binding;
            velocityBinding = moduleData.VelocityProperty.Binding;
            massBinding = moduleData.MassProperty.Binding;     
            
            Mass = moduleData.Mass;
            Velocity = moduleData.Velocity.GetVector3();
            Position = moduleData.Position.GetVector3();  

            positionBinding.ValueChanged += SetPosition;
            massBinding.ValueChanged += SetMass;
            velocityBinding.ValueChanged += SetVelocity;

            this.guid = moduleData.Planet.Guid;
            AddInteractor();
        }
        //saving to file
        public override ModuleData InstatiateModuleData() 
        {
            GravityModuleData moduleData = new GravityModuleData();
            moduleData.Position = transform.position.GetVectorXZ();
            moduleData.Velocity = Data.Velocity;
            moduleData.Mass = Data.Mass;

            return moduleData;
        }

        private void AddInteractor()
        {
            GravityManager.Instance.GravityInteractors.Add(guid, this);
        }

        private void SetPosition(Vector2 value, object sender)
        {
            if (sender != (object)this)
            { 
                this.transform.position = value.GetVector3();
                this.data.Position = value;
            }
        }

        private void SetMass(float value, object sender)
        {
            if (sender != (object)this)
            {
                this.Rigidbody.mass = value;
                this.data.Mass = value;
            }

        }

        private void SetVelocity(Vector2 value, object sender)
        {
            if (sender != (object)this)
            {
                if (IsSimulationEnabled)
                {
                    this.Rigidbody.velocity = value.GetVector3();
                }
                else
                {
                    this.savedVelocity = value.GetVector3();
                }

                this.data.Velocity = value;
            }
        }
    }

}
