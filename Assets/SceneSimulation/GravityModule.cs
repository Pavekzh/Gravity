﻿using System;
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
        /// <summary>
        /// Do not use it for setting properties
        /// </summary>
        public GravityInteractor data;


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
            set
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
                this.positionBinding.ChangeValue(value, this);          
                this.transform.position = value;
                this.data.Position = value.GetVectorXZ();
            } 
        }
        public bool IsSimulationEnabled { get; private set; }

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
            data.Velocity = Velocity.GetVectorXZ();
            data.Position = Position.GetVectorXZ();
            if (IsSimulationEnabled)
            {
                Vector3 force = GravityManager.Instance.ComputeForce(data);
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
