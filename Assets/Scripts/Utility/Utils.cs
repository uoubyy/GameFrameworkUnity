using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Utils
    {
        public static bool TryConvertVal<A, B>(A a, out B b)
        {
            if(a is B localB)
            {
                b = localB;
                return true;
            }

            b = default(B);
            return false;
        }
    }
}
