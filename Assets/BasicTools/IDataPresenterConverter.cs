using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools
{
    public interface IDataPresenterConverter<T,U>
    {
        T ConvertPresenterToData(U presenter);
        U ConvertDataToPresenter(T data);
    }
}
