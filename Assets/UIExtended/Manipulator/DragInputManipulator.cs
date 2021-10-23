using System;
using UnityEngine;

namespace Assets.UIExtended
{
    public class DragInputManipulator : InputManipulator
    {
        [SerializeField][Tooltip("Line object must be aligned along the z axis")] private MeshFilter line;
        [SerializeField][Tooltip("Object must be aligned along the z axis")] private MeshFilter touchPointer;
        [SerializeField] private Transform origin;
        [SerializeField] private Vector3 originPosition = Vector3.zero;
        [SerializeField] private float lineWidth = 1f;
        [SerializeField] private float manipulatorScale = 10f;

        private Collider pointerCollider;
        private Vector3 lineScaleFactor;
        private Vector3 touchScale;
        private bool isVisible;

        public override bool IsVisible
        {
            get => isVisible;
            set
            {
                if (line != null)
                    line.gameObject.SetActive(value);
                if (touchPointer != null)
                    touchPointer.gameObject.SetActive(value);
                isVisible = value;
            }
        }
        public override Transform Origin { get => origin; set => origin = value; }
        public Vector3 OriginPosition
        {
            get
            {
                if (origin != null)
                {
                    return origin.position;
                }
                else
                {
                    return originPosition;
                }
            }
        }

        public override event EventHandler OnManipulatorActivates;

        void Start()
        {
            pointerCollider = touchPointer.GetComponent<Collider>();
            if (pointerCollider == null)
                BasicTools.ErrorManager.Instance.ShowErrorMessage("Touch pointer have no collider", this);

            touchScale = touchPointer.transform.localScale;
            lineScaleFactor = new Vector3(1 / (line.mesh.bounds.extents.x * 2), 1 / (line.mesh.bounds.extents.y * 2), 1 / (line.mesh.bounds.extents.z * 2));
            IsVisible = false;
        }

        public override Vector3 GetInputByTouch(Touch touch,float scale)
        {
            if(IsManipulatorActive == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitinfo;
                if (pointerCollider.Raycast(ray, out hitinfo, Mathf.Infinity))
                {
                    IsManipulatorActive = true;
                    OnManipulatorActivates(this, EventArgs.Empty);
                }
                return Vector3.zero;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                float distance = 0;
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                if (plane.Raycast(ray, out distance))
                {
                    //getting touch position on plane
                    Vector3 touchPosition = ray.GetPoint(distance);
                    UpdateView(OriginPosition, touchPosition, scale);
                    return OriginPosition - touchPosition;
                }
                return Vector3.zero;
            }

        }

        public override void Deactivate(Touch touch)
        {
            IsManipulatorActive = false;   
        }

        public override void UpdateView(Vector3 origin,Vector3 touch, float scaleFactor)
        {
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


    }
}