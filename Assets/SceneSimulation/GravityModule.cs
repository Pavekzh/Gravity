using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneSimulation
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityModule:Module
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private GravityInteractor data;


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
                if (isSimulationEnabled)
                {                    
                    return Rigidbody.velocity;
                }
                else
                {
                    return savedVelocity;
                }
            }
            set
            {
                if (isSimulationEnabled)
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
                this.positionBinding.ChangeValue(value, this);          
                this.transform.position = value;
                this.data.Position = value.GetVectorXZ();
            } 
        }

        private bool isSimulationEnabled;
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
        private GravityModuleData moduleData;

        private void Start()
        {
            Services.TimeManager.Instance.TimeStateChanged += UpdateSimulationState;
            Services.GravityManager.Instance.GravityInteractors.Add(this);

            this.Mass = data.Mass;
            this.Velocity = data.Velocity.GetVector3();    
        }

        private void OnDestroy()
        {
            Services.TimeManager.Instance.TimeStateChanged -= UpdateSimulationState;

            if(positionBinding != null)
                positionBinding.ValueChanged -= SetPosition;
            if(velocityBinding != null)
                velocityBinding.ValueChanged -= SetVelocity;
            if(massBinding != null)
                massBinding.ValueChanged -= SetMass;
        }

        private void FixedUpdate()
        {
            positionBinding.ChangeValue(Position.GetVectorXZ(), this);
            velocityBinding.ChangeValue(Velocity.GetVectorXZ(), this);
            if (isSimulationEnabled)
            {
                Vector3 force = ComputeForce(Services.GravityManager.Instance.GravityInteractors, Services.GravityManager.Instance.GravityRatio);
                Rigidbody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);

                positionBinding.ChangeValue(Position.GetVectorXZ(), this);
                velocityBinding.ChangeValue(Velocity.GetVectorXZ(), this);
                data.Velocity = rigidbody.velocity.GetVectorXZ();
                data.Position = transform.position.GetVectorXZ();
            }
        }

        public virtual Vector3 ComputeForce(IEnumerable<SceneSimulation.GravityModule> Environment, float GravityRatio)
        {
            Vector3 force = Vector3.zero;
            foreach (GravityModule obj in Environment)
            {
                if (obj.isSimulationEnabled)
                {
                    float distance = Vector3.Distance(Position, obj.Position);
                    if (distance != 0)
                    {
                        float forceValue = GravityRatio * (Mass * obj.Mass) / (distance * distance);

                        Vector3 forceDirection = (obj.Position - Position).normalized;
                        force += forceDirection * forceValue;
                    }
                }

            }
            return force;
        }

        //reaction to time manager stop/resume
        public void UpdateSimulationState(bool state,object sender)
        {
            isSimulationEnabled = state;
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

        public void SetModuleData(GravityModuleData moduleData)
        {            
            positionBinding = moduleData.PositionProperty.Binding;
            velocityBinding = moduleData.VelocityProperty.Binding;
            massBinding = moduleData.MassProperty.Binding;
            
            Mass = moduleData.Mass;
            Velocity = new Vector3(moduleData.Velocity.x,0,moduleData.Velocity.y);
            Position = new Vector3(moduleData.Position.x, 0, moduleData.Position.y) ;

            positionBinding.ValueChanged += SetPosition;
            massBinding.ValueChanged += SetMass;
            velocityBinding.ValueChanged += SetVelocity;

            this.moduleData = moduleData;
        }

        public override ModuleData InstatiateModuleData() 
        {
            GravityModuleData moduleData = new GravityModuleData();
            moduleData.Position = transform.position.GetVectorXZ();
            moduleData.Velocity = data.Velocity;
            moduleData.Mass = data.Mass;
            return moduleData;
        }

        private void SetPosition(Vector2 value, object sender)
        {
            if (sender != (object)this)
            { 
                this.transform.position =value.GetVector3();
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
                this.Rigidbody.velocity = value.GetVector3();
                this.savedVelocity = value.GetVector3();
                this.data.Velocity = value;
            }
        }
    }
}
