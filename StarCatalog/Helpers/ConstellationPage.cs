using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog
{
    class CustomPage<T>
    {
        public int Number { get; }
        public T Element { get; }

        public CustomPage(int number, T element)
        {
            this.Number = number;
            this.Element = element;
        }
    }
}
