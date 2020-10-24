using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Helpers
{
    /// <summary>
    /// Provides extension methods for dealing with arrays and collections of values.
    /// </summary>
    internal static class ArrayExtensions
    {
        public static bool AreEqual<T>(T[] a, T[] b, Func<T, T, bool> equalityFunc = null)
        {
            if (a == b) return true;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if(equalityFunc != null)
                {
                    if (!equalityFunc(a[i], b[i]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!a[i].Equals(b[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
