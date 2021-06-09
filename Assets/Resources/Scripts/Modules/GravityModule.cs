using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Resources.Library;
using System;

public class GravityModule : GravityObject
{
    [SerializeField]private Vector3 velocity;

    public override float Mass { get { return Rigidbody.mass; } }
    public override Vector3 Velocity { get { return Rigidbody.velocity; } }
    public override Vector3 Position { get { return transform.position; } }

    private Rigidbody Rigidbody;
    void Start()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            ErrorManager.Instance.ShowErrorMessage("Gravity object must have Rigidbody component");
        }
        GravityManager.Instance.Objects.Add(this);
        Rigidbody.AddForce(velocity,ForceMode.VelocityChange);
    }
    private void FixedUpdate()
    {
        Vector3 force = ComputeForce(GravityManager.Instance.Objects, GravityManager.Instance.GravityRatio);
        Rigidbody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    public Vector3 ComputeForce(IEnumerable<GravityObject> Environment, float GravityRatio)
    {
        Vector3 force = Vector3.zero;
        foreach (GravityObject obj in Environment)
        {
            float distance = Vector3.Distance(Position, obj.Position);
            if (distance != 0)
            {

                float forceValue = GravityRatio * (Mass * obj.Mass) / (distance * distance);

                Vector3 forceDirection = (obj.Position - Position).normalized;
                force += forceDirection * forceValue;
            }
        }

        return force;
    }
}
