using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Services
{
    public interface ISaveSystem
    {
        void Delete(string fullPath);
        void Save(object state,string fullPath);
        object Load(string path, Type type);

        string Extension { get; }
    }
}
