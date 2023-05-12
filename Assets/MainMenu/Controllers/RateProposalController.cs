using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace Assets.Menu.Controllers
{
    class RateProposalController:MonoBehaviour
    {
        [SerializeField] StateChanger opener;

        public void Close()
        {
            opener.State = State.Default;
        }

        public void Open()
        {
            opener.State = State.Changed;
        }

        public void Rate()
        {
            Application.OpenURL("market://details?id=com.Zhukovin.Gravity");
            opener.State = State.Default;
        }
    }
}
