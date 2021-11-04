using BasicTools;
using System.Collections.Generic;
using UnityEngine;

namespace UIExtended
{
    [RequireComponent(typeof(MeshFilter))]
    public class RectangularCurveDrawer:CurveDrawer
    {
        [SerializeField] private float tipScale = 1;
        [SerializeField] private Mesh tip;
        [SerializeField] private float width = 1;
        [SerializeField] private float height = 1;

        private MeshFilter meshFilter;

        public override void Draw(StateCurve<StateCurvePoint3D> curve)
        {
            meshFilter.mesh = new Mesh();

            int sectionsNumber = curve.PointsAmount - 1;               
            //first plane + last plane + sections number * planes per section * vertices per plane         
            int verticesNumber = 4 + sectionsNumber * 4 * 4 ;
            //first plane + last plane + sections number * planes per section * triangles per plane * vertices per triangle
            int trisNumber = 6 + sectionsNumber * 4 * 2 * 3;

            Vector3[] vertices = new Vector3[verticesNumber + tip.vertices.Length];
            int[] triangles = new int[trisNumber + tip.triangles.Length];

            void WritePlaneTris(int trisIndex,int verticesIndex)
            {
                triangles[trisIndex + 0] = verticesIndex + 0;
                triangles[trisIndex + 1] = verticesIndex + 1;
                triangles[trisIndex + 2] = verticesIndex + 3;

                triangles[trisIndex + 3] = verticesIndex + 0;
                triangles[trisIndex + 4] = verticesIndex + 3;
                triangles[trisIndex + 5] = verticesIndex + 2;
            }
            void WritePlaneVertices(int index, Vector3[] planeVertices)
            {
                vertices[index + 0] = planeVertices[0];
                vertices[index + 1] = planeVertices[1];
                vertices[index + 2] = planeVertices[2];
                vertices[index + 3] = planeVertices[3];
            }

            //1 section
            Vector3[] lastSectionPlane = FormSectionPlane(curve.Points[0].Position, (curve.Points[1].Position - curve.Points[0].Position));
            WritePlaneVertices(0, lastSectionPlane);
            WritePlaneTris(0,0);

            for (int i = 1; i < curve.PointsAmount - 1; i++)
            {
                Vector3[] sectionPlane = FormSectionPlane(curve.Points[i].Position, (curve.Points[i + 1].Position - curve.Points[i].Position));

                Vector3[] leftPlane = new Vector3[4] { sectionPlane[0], lastSectionPlane[0], sectionPlane[2], lastSectionPlane[2] };
                Vector3[] rightPlane = new Vector3[4] { lastSectionPlane[1], sectionPlane[1], lastSectionPlane[3], sectionPlane[3] };
                Vector3[] topPlane = new Vector3[4] { sectionPlane[0], sectionPlane[1], lastSectionPlane[0], lastSectionPlane[1] };
                Vector3[] bottomPlane = new Vector3[4] { lastSectionPlane[2], lastSectionPlane[3], sectionPlane[2], sectionPlane[3] };

                //first plane + i - 1 * planes for section * vertices per plane
                int verticesIndex = 4 + (i - 1) * 4 * 4;
                //first plane + i - 1 * planes for section * tris per plane
                int trisIndex = 6 + (i - 1) * 4 * 6;

                WritePlaneVertices(verticesIndex, leftPlane);
                WritePlaneTris(trisIndex, verticesIndex);
                WritePlaneVertices(verticesIndex + 4, rightPlane);
                WritePlaneTris(trisIndex + 6, verticesIndex + 4);
                WritePlaneVertices(verticesIndex + 8, topPlane);
                WritePlaneTris(trisIndex + 12, verticesIndex + 8);
                WritePlaneVertices(verticesIndex + 12, bottomPlane);
                WritePlaneTris(trisIndex + 18, verticesIndex + 12);

                lastSectionPlane = sectionPlane;
            }

            Vector3 lineDirection = curve.Points[curve.PointsAmount - 1].Position - curve.Points[curve.PointsAmount - 2].Position;
            Quaternion imaginaryPlaneRotation = Quaternion.LookRotation(lineDirection, Vector3.up);
            for (int i = 0; i < tip.vertices.Length; i++)
            {
                vertices[verticesNumber + i] = curve.Points[curve.PointsAmount - 1].Position + imaginaryPlaneRotation * tip.vertices[i] * tipScale;

            }
            for (int i = 0; i < tip.triangles.Length; i++)
            {
                triangles[trisNumber + i] = tip.triangles[i] + verticesNumber;
            }


            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.triangles = triangles;
            meshFilter.mesh.uv = new Vector2[verticesNumber + tip.vertices.Length];

            meshFilter.mesh.RecalculateNormals();
        }

        private Vector3[] FormSectionPlane(Vector3 checkpoint,Vector3 direction)
        {
            Vector3[] vertices = new Vector3[4];
            Vector3 checkpointLeft = Vector3.Cross(direction, Vector3.up).normalized * (width / 2);
            Vector3 checkpointUp = Vector3.up * (height / 2);

            vertices[0] = checkpoint + checkpointLeft + checkpointUp;
            vertices[1] = checkpoint - checkpointLeft + checkpointUp;
            vertices[2] = checkpoint + checkpointLeft - checkpointUp;
            vertices[3] = checkpoint - checkpointLeft - checkpointUp;

            return vertices;
        }

        private void Awake()
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
        }
    }
}
