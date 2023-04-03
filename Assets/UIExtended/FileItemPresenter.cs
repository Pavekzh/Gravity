using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIExtended
{
    public abstract class FileItemPresenter : IItemPresenter,ICloneable
    {

        public virtual string Path { get; set; }

        public abstract object Clone();

        public abstract RectTransform GetItemView(RectTransform parent);
    }
}
