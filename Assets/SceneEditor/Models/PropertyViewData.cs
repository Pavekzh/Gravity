using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.SceneEditor.Models
{
    public abstract class PropertyViewData
    {
        public abstract string Name { get; set; }
        public abstract string[] Components { get; set; }

        public abstract void ChangePresenter(string[] dataProvider, object source);
        public abstract event BasicTools.ValueChangedHandler<string[]> ValueChanged;
    }
}
