using BasicTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIExtended
{
    public abstract class DragInputManipulator<T> : InputManipulator<T>
    {
        [SerializeField] [Tooltip("Line object must be aligned along the z axis")] private MeshFilter line;
        [SerializeField] [Tooltip("Object must be aligned along the z axis")] private MeshFilter touchPointer;
        [SerializeField] private CombinedInteraction UIVisibleStateManager;
        [SerializeField] private Vector3 originPosition = Vector3.zero;
        [SerializeField] private float lineWidth = 1f;
        [SerializeField] private float manipulatorScale = 10f;

        private bool isManipulatorActive;
        private Collider pointerCollider;
        private Vector3 lineScaleFactor;
        private Vector3 touchScale;
        private bool isVisible;
        private Binding<Vector2> originBinding;
        private float scaleFactor = 1;


        public Binding<Vector2> OriginBinding
        {
            get => originBinding;
            set
            {
                if (originBinding == null && value != null)
                {
                    this.originBinding = value;
                    this.originBinding.ValueChanged += SetOriginPosition;
                    this.originBinding.ForceUpdate();
                }
                if (originBinding != null)
                {
                    if (value == null)
                    {
                        this.originBinding.ValueChanged -= SetOriginPosition;
                        this.originBinding = value;
                    }
                    else
                    {
                        this.originBinding.ValueChanged -= SetOriginPosition;
                        this.originBinding = value;
                        this.originBinding.ValueChanged += SetOriginPosition;
                        this.originBinding.ForceUpdate();

                    }
                }

            }
        }
        public override event Action InputReadingStarted;
        public override event Action InputReadingStoped;
        public override float ScaleFactor 
        { 
            get => scaleFactor; 
            set
            {                
                UpdateScaleFactor(value);
                scaleFactor = value;
            }
        }


        public bool IsEnabled
        {
            get => isVisible;
            protected set
            {
                if (line != null)
                    line.gameObject.SetActive(value);
                if (touchPointer != null)
                    touchPointer.gameObject.SetActive(value);
                isVisible = value;
            }
        }
        public Vector3 OriginPosition
        {
            get
            {
                return originPosition;
            }
        }

        protected virtual void Awake()
        {
            pointerCollider = touchPointer.GetComponent<Collider>();
            if (pointerCollider == null)
                BasicTools.MessagingSystem.Instance.ShowErrorMessage("Touch pointer have no collider", this);

            touchScale = touchPointer.transform.localScale;
            lineScaleFactor = new Vector3(1 / (line.mesh.bounds.extents.x * 2), 1 / (line.mesh.bounds.extents.y * 2), 1 / (line.mesh.bounds.extents.z * 2));
            IsEnabled = false;
        }

        private void Update()
        {
            if (!isManipulatorActive && IsEnabled && Input.touchCount == 1)
            {
                isManipulatorActive = IsTouchOverPointer(Input.GetTouch(0));
                if (isManipulatorActive)
                    this.InputReadingStarted?.Invoke();
            }
            if (isManipulatorActive && IsEnabled && Input.touchCount != 1)
            {
                isManipulatorActive = false;
                this.InputReadingStoped?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (IsEnabled && isManipulatorActive && Input.touchCount == 1)
            {
                Vector3 touch = GetTouchPosition(Input.GetTouch(0));
                WriteInput(touch);
            }
        }

        protected virtual void WriteInput(Vector3 touchPosition)
        {
            UpdateView(touchPosition);
        }

        private void SetOriginPosition(Vector2 value, object sender)
        {
            this.originPosition = value.GetVector3();
        }

        public override void Enable(Binding<Vector2> originBinding)
        {
            if (this.IsEnabled)
                this.Disable();

            UIVisibleStateManager.State = State.Changed;
            OriginBinding = originBinding;
            this.IsEnabled = true;
        }

        public override void Disable()
        {
            if (IsEnabled)
            {
                UIVisibleStateManager.State = State.Default;
                OriginBinding = null;
                this.isManipulatorActive = false;
                this.IsEnabled = false;
            }
        }

        public bool IsTouchOverPointer(Touch touch)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitInfo;
            if (pointerCollider.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                return true;
            }
            return false;
        }

        private Vector3 GetTouchPosition(Touch touch)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            float distance;
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out distance))
            {
                //getting touch position on plane
                Vector3 touchPosition = ray.GetPoint(distance);
                return touchPosition;
            }
            return Vector3.zero;
        }

        protected virtual void UpdateView(Vector3 input)
        {
            Vector3 origin = originPosition;
            Vector3 touch = origin - input;
            float scaleFactor = ScaleFactor;

            scaleFactor *= this.manipulatorScale;
            if (origin != touch)
            {
                float angle = Vector3.SignedAngle(origin - touch, Vector3.forward, Vector3.down);

                touchPointer.transform.localScale = touchScale * scaleFactor;
                touchPointer.transform.position = touch;
                touchPointer.transform.eulerAngles = new Vector3(0, angle, 0);


                line.transform.position = (origin + touch) / 2;
                line.transform.eulerAngles = new Vector3(0, angle, 0);
                line.transform.localScale = new Vector3(lineWidth * lineScaleFactor.x * scaleFactor, lineWidth * lineScaleFactor.y * scaleFactor, Vector3.Distance(origin, touch) * lineScaleFactor.z);
            }
            else
            {
                touchPointer.transform.position = touch;
                touchPointer.transform.localScale = touchScale * scaleFactor;
                line.transform.localScale = Vector3.zero;
            }

        }

        protected void UpdateScaleFactor(float scaleFactor)
        {

            line.transform.localScale = new Vector3(line.transform.localScale.x / ScaleFactor * scaleFactor, line.transform.localScale.y / ScaleFactor * scaleFactor, line.transform.localScale.z);
            touchPointer.transform.localScale = touchPointer.transform.localScale / ScaleFactor * scaleFactor;
        }
    }
}
