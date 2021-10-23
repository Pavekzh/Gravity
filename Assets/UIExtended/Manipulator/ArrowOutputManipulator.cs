using System.Collections;
using UnityEngine;

namespace Assets.UIExtended
{
    public class ArrowOutputManipulator : OutputManipulator
    {
        [SerializeField] MeshFilter line;
        [SerializeField] MeshFilter arrow;
        [SerializeField] MeshFilter origin;
        [SerializeField] float lineWidth = 1;
        [SerializeField] float manipulatorScale = 10f;

        private Vector3 arrowScale;
        private Vector3 originScale;
        private Vector3 lineScale;
        private bool isVisible;

        private void Start()
        {
            if(arrow != null)
                arrowScale = arrow.transform.localScale;
            if(origin != null)
                originScale = origin.transform.localScale;
            if(line != null)
                lineScale = new Vector3(1 / (line.mesh.bounds.extents.x * 2), 1 / (line.mesh.bounds.extents.y * 2), 1 / (line.mesh.bounds.extents.z * 2));

            IsVisible = false;
        }

        public override bool IsVisible 
        {
            get => isVisible;
            set
            {
                if (line != null)
                    line.gameObject.SetActive(value);
                if (arrow != null)
                    arrow.gameObject.SetActive(value);
                if (origin != null)
                    origin.gameObject.SetActive(value);
                isVisible = value;
            }
        }

        public override void UpdateManipulatorView(Vector3 origin, Vector3 directVector, float scaleFactor)
        {
            scaleFactor *= this.manipulatorScale;
            Vector3 directPoint = origin + directVector;
            if(origin != directPoint)
            {
                float angle = Vector3.SignedAngle(directPoint - origin, Vector3.forward, Vector3.down);
                if(this.line != null)
                {
                    this.line.transform.position = (directPoint + origin) / 2;
                    this.line.transform.eulerAngles = new Vector3(0, angle, 0);
                    this.line.transform.localScale = new Vector3(lineWidth * lineScale.x * scaleFactor, lineWidth * lineScale.y * scaleFactor, Vector3.Distance(directPoint, origin) * lineScale.z);
                }
                if (this.arrow != null)
                {
                    this.arrow.transform.localScale = arrowScale * scaleFactor;
                    this.arrow.transform.position = directPoint;
                    this.arrow.transform.eulerAngles = new Vector3(0, angle, 0);
                }
                if (this.origin != null)
                {
                    this.origin.transform.localScale = originScale * scaleFactor;
                    this.origin.transform.position = origin;
                    this.origin.transform.eulerAngles = new Vector3(0, angle, 0);
                }

            }
            else
            {
                if (this.line != null)
                {
                    this.line.transform.localScale = Vector3.zero;
                }
                if (this.arrow != null)
                {
                    this.arrow.transform.localScale = Vector3.zero;
                }
                if (this.origin != null)
                {
                    this.origin.transform.localScale = originScale * scaleFactor;
                    this.origin.transform.position = origin;
                }
            }
        }
    }
}