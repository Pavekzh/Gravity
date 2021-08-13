using UnityEngine;
using System.Collections;

namespace UIExtended
{
    public class ArrowManipulator : ManipulatorView
    {
        [SerializeField] Vector3 originPoint;
        [SerializeField] Vector3 directPoint;
        [SerializeField] Vector3 touchPoint;
        [SerializeField] float zoomFactor;
        [SerializeField] float lineWidth;

        [SerializeField] MeshFilter line;
        [SerializeField] MeshFilter arrow;
        [SerializeField] MeshFilter origin;
        [SerializeField] MeshFilter touch;

        Vector3 arrowScale;
        Vector3 originScale;
        Vector3 touchScale;
        Vector3 scaleFactor;

        private void Awake()
        {
            arrowScale = arrow.transform.localScale;
            originScale = origin.transform.localScale;
            touchScale = touch.transform.localScale;

            transform.position = Vector3.zero;
            scaleFactor = new Vector3(1 / (line.mesh.bounds.extents.x * 2), 1 / (line.mesh.bounds.extents.y * 2), 1 / (line.mesh.bounds.extents.z * 2));
        }

        private void Start()
        {
            SetManipulator(originPoint, directPoint, touchPoint, zoomFactor);
        }

        public override void SetManipulator(Vector3 originPoint, Vector3 directPoint, Vector3 touchPoint,float scale)
        {
            if (directPoint != originPoint)
            {
                float angle = Vector3.SignedAngle(directPoint - originPoint, Vector3.forward, Vector3.down);

                line.transform.position = (directPoint + touchPoint) / 2;
                line.transform.eulerAngles = new Vector3(0, angle, 0);
                line.transform.localScale = new Vector3(lineWidth * scaleFactor.x * scale, lineWidth * scaleFactor.y * scale, Vector3.Distance(directPoint, touchPoint) * scaleFactor.z);
                arrow.transform.localScale = arrowScale *  scale;
                arrow.transform.position = directPoint;
                arrow.transform.eulerAngles = new Vector3(0, angle, 0);
                origin.transform.localScale = originScale * scale;
                origin.transform.position = originPoint;
                origin.transform.eulerAngles = new Vector3(0, angle, 0);
                touch.transform.localScale = touchScale * scale;
                touch.transform.position = touchPoint;
                touch.transform.eulerAngles = new Vector3(0, angle, 0);
            }
            else
            {
                line.transform.position = originPoint;
                line.transform.eulerAngles = Vector3.zero;
                line.transform.localScale = Vector3.zero;
                arrow.transform.localScale = Vector3.zero;
                arrow.transform.position = originPoint;
                arrow.transform.eulerAngles = Vector3.zero;
                origin.transform.localScale = originScale /** zoomFactor*/;
                origin.transform.position = originPoint;
                origin.transform.eulerAngles = Vector3.zero;
                touch.transform.localScale = touchScale /** zoomFactor*/;
                touch.transform.position = originPoint;
                touch.transform.eulerAngles = Vector3.zero;
            }
        }

        public override void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }
    }
}