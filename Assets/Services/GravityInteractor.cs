﻿using UnityEngine;


namespace Assets.Services
{
    [System.Serializable]
    public struct GravityInteractor
    {
        [SerializeField]private Vector2 position;
        [SerializeField]private Vector2 velocity;
        [SerializeField]private float mass;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public float Mass { get => mass; set => mass = value; }

        public GravityInteractor(Vector2 position,Vector2 velocity, float mass)
        {
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
        }

        public GravityInteractor(GravityInteractor data)
        {
            this.position = data.Position;
            this.mass = data.Mass;
            this.velocity = data.Velocity;
        }

        public static GravityInteractor Lerp(GravityInteractor state1,GravityInteractor state2,float t)
        {
            Vector2 position = Vector2.Lerp(state1.Position, state2.Position, t);
            Vector2 velocity = Vector2.Lerp(state1.Velocity, state2.Velocity, t);
            float mass = state1.mass + (state2.mass - state1.mass) * t;

            return new GravityInteractor(position, velocity, mass);
        }
    }
}