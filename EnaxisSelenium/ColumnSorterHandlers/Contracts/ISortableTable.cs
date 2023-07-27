using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.Foo.Contracts
{
    public interface ISortableTable
    {
        bool IsLinkPresentInHeader(int columnIndex);
        void SortByColumn(int columnIndex);
    }
}
