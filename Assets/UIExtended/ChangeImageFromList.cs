using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UIExtended
{
    public class ChangeImageFromList:MonoBehaviour
    {
        [SerializeField] private List<Sprite> imageList;
        [SerializeField] private Image image;
        [SerializeField] private int state = 0;

        private void Start()
        {
            SwitchState(state);
        }

        public void SwitchState()
        {
            if (state < imageList.Count - 1)
                image.sprite = imageList[state += 1];
            else
                image.sprite = imageList[state = 0];
        }

        public void SwitchState(int index)
        {
            if (state < imageList.Count)
                image.sprite = imageList[index];
            else
                image.sprite = imageList[state = imageList.Count - 1];
        }
    }
}
