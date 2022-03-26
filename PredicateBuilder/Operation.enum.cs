using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateBuilder
{    public enum Operation : byte
    {
        EQUALS = 1,
        NOT_EQUALS = 2,
        LESS_THAN = 3,
        GREATER_THAN = 4,
        LESS_THAN_OR_EQUEAL = 5,
        GREATER_THAN_OR_EQUEAL = 6,
    }
}
