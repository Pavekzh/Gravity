using UnityEngine;
using System.Collections;

public abstract class ManipulatorView : MonoBehaviour
{
    public abstract void SetManipulator(Vector3 originPoint, Vector3 directPoint, Vector3 touchPoint);
    public abstract void SetActive(bool isActive);
}
