using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using System;

public class GravityModule : Module
{
    public float Mass { get { return Rigidbody.mass; } }
    public Vector3 Velocity { get { return Rigidbody.velocity; } }
    public Vector3 Position { get { return transform.position; } }
    public override Planet Planet
    {
        get => planet;
        set
        {
            value.GravityModule = this;
            planet = value;
            planet.Modules.Add(this);

        }
    }

    private Vector3 savedVelocity;
    private Rigidbody Rigidbody;
    public override void Awake()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Gravity object must have Rigidbody component",this);
        }
        GravityManager.Instance.Objects.Add(this);
        base.Awake();
    }
    private void FixedUpdate()
    {
        if (IsPhysicsActive)
        {
            Vector3 force = ComputeForce(GravityManager.Instance.Objects, GravityManager.Instance.GravityRatio);
            Rigidbody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
    public Vector3 ComputeForce(IEnumerable<GravityModule> Environment, float GravityRatio)
    {
        Vector3 force = Vector3.zero;
        foreach (GravityModule obj in Environment)
        {
            if (obj.IsPhysicsActive)
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
    public override void UpdatePhysicsState(bool state)
    {
        base.UpdatePhysicsState(state);
        if(state == false && Rigidbody.isKinematic == false)
        {
            savedVelocity = Rigidbody.velocity;
            Rigidbody.isKinematic = true;
        }
        else if(state == true && Rigidbody.isKinematic == true)
        {
            Rigidbody.isKinematic = false;
            Rigidbody.velocity = savedVelocity;
        }
    }
    public override void SetModule(ModuleData module)
    {
        GravityModuleData data = module as GravityModuleData;
        if (data == null)
        {
            ErrorManager.Instance.ShowErrorMessage("ModuleData must be GravityModuleData",this);
        }
        else
        {
            Rigidbody.mass = data.Mass;
            Rigidbody.velocity = data.Velocity;
            transform.position = data.Position;
            savedVelocity = data.Velocity;
        }
    }
    public override ModuleData GetModuleData()
    {
        GravityModuleData moduleData;
        if (this.IsPhysicsActive)
            moduleData = new GravityModuleData(this.Mass,this.Position,this.Velocity);
        else
            moduleData = new GravityModuleData(this.Mass, this.Position, this.savedVelocity);


        return moduleData;
    }
    public void AddVelocity(Vector3 velocity)
    {
        if (IsPhysicsActive)
            Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        else
            savedVelocity += velocity;
    }
}
