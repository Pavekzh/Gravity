using UnityEngine;


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
    }
}
