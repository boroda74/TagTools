using System.Collections;
using System.Collections.Generic;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        #region Finding related presets
        internal delegate T GetNextItem<T>(T current);

        private static bool ExcludeItemFromChain<T>(T selected, T current, GetNextItem<T> getNextItem) where T : class
        {
            if (selected == current)
                return false;

            T next = getNextItem(current);

            if (next != null)
                return ExcludeItemFromChain<T>(selected, next, getNextItem);

            return true;
        }

        internal delegate bool AddSkipItem<T>(IList filteredList, T initial);

        //NOTE:
        //  SortedDictionary<T1, T2> itemChain
        //
        //RETURNS:
        //  Item chain in itemChain
        internal static bool BuildItemChain<T>(ICollection<T> fullItemCollection, IList filteredList, T initial,
            AddSkipItem<T> addSkipItem, GetNextItem<T> getNextItem)
        {
            addSkipItem(filteredList, initial);
            T next = getNextItem(initial);

            if (next != null)
                BuildItemChain(fullItemCollection, filteredList, next, addSkipItem, getNextItem);

            return filteredList.Count > 0;
        }
        #endregion
    }
}
