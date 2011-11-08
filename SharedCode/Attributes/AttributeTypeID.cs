using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedCode.Attributes
{
    public enum AttributeTypeID : byte
    {
        Int = 0,
        Float = 1,
        Long = 2,
        Vector2 = 3,
        Rectangle = 4,
        String = 5,
        List = 6,
        Bool = 7
    }
}
