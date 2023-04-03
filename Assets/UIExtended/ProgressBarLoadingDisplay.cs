using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;
using UnityEngine.UI;
using TMPro;

namespace UIExtended
{
    public class ProgressBarLoadingDisplay : LoadingProgressDisplay
    {
        [SerializeField] TMPro.TMP_Text statusText;
        [SerializeField] StateChanger opener;
        [SerializeField] Slider progressBar;


        public override void Display()
        {
            opener.State = State.Changed;
            progressBar.value = 0;
            if (statusText != null)
                statusText.text = "Loading";
        }

        public override void Display(float progress)
        {
            progressBar.value = progress;
            if(progress >= 0.9f)
                statusText.text = "Initialization";

        }

        public override void Hide()
        {

            opener.State = State.Default;
        }
    }
}
