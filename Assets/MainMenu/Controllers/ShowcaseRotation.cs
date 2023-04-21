using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Menu.Controllers
{
    public class ShowcaseRotation : MonoBehaviour
    {
        [SerializeField] Vector3 axis = Vector3.up;
        [SerializeField] float pointerRotationSpeed = 30;
        [SerializeField] float angularSpeed = 1;
        [SerializeField] float smoothness = 0.3f;
        
        public Vector3 Axis { get => axis; set => axis = value; }
        public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }
        public float PointerRotationSpeed { get => pointerRotationSpeed; set => pointerRotationSpeed = value; }
        public float Smoothness { get => smoothness; set => smoothness = value; }
         
        private bool isDrag;
        private Vector2 lastPointerPosition;
        private Camera mainCamera;
        private float screenDiagonalPx { get => new Vector2(Screen.width, Screen.height).magnitude; }
        private const float pointerSpeed = 10000;

        private Vector3 smoothVelocity;
        private Vector3 currentRotation;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (transform.position + axis.normalized) *  3);
        }

        private void Start()
        {
            Vector3 rotation = axis * angularSpeed * Time.deltaTime;
            currentRotation = rotation;
        }

        private void Update()
        {
            if (!isDrag)
            {
                Vector3 rotation = axis * angularSpeed * Time.deltaTime;
                currentRotation = Vector3.SmoothDamp(currentRotation, rotation, ref smoothVelocity, smoothness);
                transform.Rotate(currentRotation, Space.World);
            }

        }

        private void OnMouseDown()
        {
            lastPointerPosition = new Vector2(Input.mousePosition.x / screenDiagonalPx, Input.mousePosition.y / screenDiagonalPx);
            isDrag = true;
        }

        private void OnMouseUp()
        {
            isDrag = false;
        }

        private void OnMouseDrag()
        {

            Vector2 pointerPosition = new Vector2(Input.mousePosition.x / screenDiagonalPx, Input.mousePosition.y / screenDiagonalPx);
            Vector2 delta = pointerPosition - lastPointerPosition;

            Vector3 cameraUp = mainCamera.transform.rotation * Vector3.up;
            Vector3 cameraRight = mainCamera.transform.rotation * Vector3.right;

            Vector3 yRotation = (-delta.x) * pointerRotationSpeed * pointerSpeed * Time.deltaTime * cameraUp;
            Vector3 xRotation = delta.y * pointerRotationSpeed * pointerSpeed  *Time.deltaTime * cameraRight;

            currentRotation = Vector3.SmoothDamp(currentRotation, xRotation + yRotation, ref smoothVelocity, smoothness);
            transform.Rotate(currentRotation,Space.World);
            lastPointerPosition = pointerPosition;
        }


    }
}