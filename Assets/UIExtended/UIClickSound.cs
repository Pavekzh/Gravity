using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.UIExtended
{
    public class UIClickSound : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] AudioSource audioSource;

        public void OnPointerDown(PointerEventData eventData)
        {
            audioSource.Play();
        }

    }
}