using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decklan.ML.Core.Helpers
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// In a 2D matrix T[row, column], gets an array of values from T[*,<paramref name="columnNumber"/>].
        /// </summary>
        /// <param name="columnNumber">The index of the column to return.</param>
        public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        /// <summary>
        /// In a 2D matrix T[row, column], gets an array of values from T[<paramref name="columnNumber"/>,*].
        /// </summary>
        /// <param name="rowNumber">The index of the row to return.</param>
        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
