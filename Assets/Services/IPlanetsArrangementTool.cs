using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Services
{
    public interface IPlanetsArrangementTool<T>
    {
        bool IsShowing { get; }
        void HideArrangement();
        void ShowArrangement(IPlanetEstimator<T> estimator);
    }
}
