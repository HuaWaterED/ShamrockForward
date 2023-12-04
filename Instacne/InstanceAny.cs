using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shamrock_Forward.Instacne
{
    internal class InstanceAny<T> where T : new()
    {
        static T instance = new();
        static bool isSeted = false;
        public static T Instance => instance ??= new();
        public static void SetInstance(T t)
        {
            if (!isSeted)
            {
                isSeted = true;
                instance = t;
            }
        }
    }
}
