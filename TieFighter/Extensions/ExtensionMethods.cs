using System;
using System.Collections.Generic;
using System.Linq;

namespace TieFighter.Extensions
{
    public static class ExtensionMethods
    {
        public static List<T> GetPaginatedItems<T,U>(int pageNumber, int productsPerPage, bool descending, Func<T,U> sortFunc, IEnumerable<T> source)
        {
            if (sortFunc == null || pageNumber < 0 || productsPerPage <= 0)
                return Slice(source, pageNumber, productsPerPage);

            List<T> orderedProducts = null;

            if (descending)
                orderedProducts = source.AsQueryable().OrderByDescending(el => sortFunc(el)).ToList();
            else
                orderedProducts = source.AsQueryable().OrderBy(el => sortFunc(el)).ToList();

            orderedProducts = Slice(orderedProducts, pageNumber, productsPerPage);
            return orderedProducts;

        }

        public static List<T> Slice<T>(IEnumerable<T> list, int pageNumber, int numberOfItemsPerPage)
        {
            var sourceList = new List<T>(list);

            if (sourceList.Count < numberOfItemsPerPage)
            {
                if (pageNumber == 0)
                    return sourceList;
                else
                    throw new ArgumentOutOfRangeException("The page number and item count per page exceeded the number of items in the list.");
            }

            int removeBeforeLength = 0;
            if (pageNumber != 0)
                removeBeforeLength = pageNumber * numberOfItemsPerPage - 1;
            int removeAfterStartingPoint = removeBeforeLength + numberOfItemsPerPage;
            int removeAfterLength = sourceList.Count - removeAfterStartingPoint;

            sourceList.RemoveRange(removeAfterStartingPoint, removeAfterLength);
            if (pageNumber != 0)
                sourceList.RemoveRange(0, removeBeforeLength);
            return sourceList;
        }
    }
}
