using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using System;

public class GravityModule : Module
{
    [SerializeField] private GenericModulePresenter modulePresenter;

    public float Mass { get { return Rigidbody.mass; } }
    public Vector3 Velocity
    {
        get
        {
            if (IsPhysicsActive)
                return Rigidbody.velocity;
            else
                return savedVelocity;
        }
        set
        {
            if (IsPhysicsActive)
            {
                Rigidbody.velocity = value;
                velocityBinding.ChangeValue(new Vector2(value.x, value.z), this);
            }
            else
            {
                savedVelocity = value;
                velocityBinding.ChangeValue(new Vector2(value.x, value.z), this);
            }

        }
    }
    public Vector3 Position { get { return transform.position; } }
    public override Planet Planet
    {
        get => planet;
        set
        {
            value.GravityModule = this;
            planet = value;
            planet.Modules.Add(this);
            planet.Presenters.Add(modulePresenter);
            ErrorManager.Instance.ShowWarningMessage("Garvity module doesn't bind ui presenter",this);
        }
    }

    private Vector3 savedVelocity;
    private Rigidbody Rigidbody;
    private Binding<Vector2> positionBinding;
    private Binding<Vector2> velocityBinding;
    private Binding<float> massBinding;

    public override void Awake()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Gravity object must have Rigidbody component",this);
        }
        GravityManager.Instance.Objects.Add(this);
        if(Planet != null)
        {
            Planet.Presenters.Add(modulePresenter);
            ErrorManager.Instance.ShowWarningMessage("Garvity module doesn't bind ui presenter", this);
        }
        InitModulePresenter();
        base.Awake();
    }
    private void FixedUpdate()
    {
        if (IsPhysicsActive)
        {
            Vector3 force = ComputeForce(GravityManager.Instance.Objects, GravityManager.Instance.GravityRatio);
            Rigidbody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        positionBinding.ChangeValue(new Vector2(Position.x, Position.z), this);
    }
    private void InitModulePresenter()
    {
        modulePresenter.ModuleName = this.GetType().Name;
        Binding<float> massBinding = new Binding<float>();
        string[] massLabels = new string[1] { "" };
        ModuleProperty<float> massProperty = new ModuleProperty<float>(massLabels, "Mass", ModuleProperty<float>.FloatFromInputConverter, ModuleProperty<float>.FloatToInputConverter, massBinding,Mass);
        massBinding.ValueChanged += SetMass;
        this.massBinding = massBinding;

        Binding<Vector2> positionBinding = new Binding<Vector2>();
        string[] positionLabels = new string[2] {"X","Y" };
        ModuleProperty<Vector2> positionProperty = new ModuleProperty<Vector2>(positionLabels, "Postion", ModuleProperty<Vector2>.Vector2FromInputConverter, ModuleProperty<Vector2>.Vector2ToInputConverter, positionBinding, new Vector2(Position.x,Position.z));
        positionBinding.ValueChanged += SetPosition;
        this.positionBinding = positionBinding;

        Binding<Vector2> velocityBinding = new Binding<Vector2>();
        ModuleProperty<Vector2> velocityProperty = new ModuleProperty<Vector2>(positionLabels, "Velocity", ModuleProperty<Vector2>.Vector2FromInputConverter, ModuleProperty<Vector2>.Vector2ToInputConverter, velocityBinding, new Vector2(Velocity.x, Velocity.z));
        velocityBinding.ValueChanged += SetVelocity;
        this.velocityBinding = velocityBinding;

        modulePresenter.AddProperty<float>(massProperty);
        modulePresenter.AddProperty<Vector2>(positionProperty);
        modulePresenter.AddProperty<Vector2>(velocityProperty);

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

    private void SetPosition(Vector2 value,object sender)
    {
        if(sender != (object)this)
        {
            this.Rigidbody.position = new Vector3(value.x,0,value.y);
        }
    }
    private void SetMass(float value,object sender)
    {
        if (sender != (object)this)
            this.Rigidbody.mass = value;
    }
    private void SetVelocity(Vector2 value,object sender)
    {
        if(sender != (object)this)
        {
            Velocity = new Vector3(value.x, 0, value.y);
        }
    }
}
