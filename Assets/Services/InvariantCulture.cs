using System.Collections;
using UnityEngine;
using System.Globalization;

namespace Assets.Services
{
    public class InvariantCulture : MonoBehaviour
    {
        void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

    }
}