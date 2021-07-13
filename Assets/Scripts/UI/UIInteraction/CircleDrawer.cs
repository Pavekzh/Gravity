using UnityEngine;
using System.Collections;

public class CircleDrawer : MonoBehaviour
{
    [SerializeField] new LineRenderer renderer;
    [SerializeField] int pointsCount;
    [SerializeField] Vector2 radius;

    private void Start()
    {
        renderer.positionCount = pointsCount;
        CreatePoints();
    }

    public void CreatePoints()
    {
        Keyframe[] keys = new Keyframe[pointsCount];
        float x;
        float y;
        float segments = pointsCount - 1;
        float angle = 0;

        for(int i = 0;i < pointsCount; i++)
        {
            x = radius.x * Mathf.Cos(Mathf.Deg2Rad * angle);
            y = radius.y * Mathf.Sin(Mathf.Deg2Rad * angle);
            angle += 180 / segments;

            keys[i] = new Keyframe((x + radius.x) / (radius.x * 2),(y * 2) / radius.y);
            renderer.SetPosition(i, new Vector3(x,0,0));
        }
        renderer.widthCurve = new AnimationCurve(keys);
    }
}
