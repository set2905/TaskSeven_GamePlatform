namespace TaskSeven_GamePlatform.Helpers
{
    public static class CollectionHelper
    {

        public static int GetForwardCircularIndex(int lastIndex, int index)
        {
            if (index>lastIndex)
            {
                index %= lastIndex;
                return index;
            }
            return index;
        }
    }
}
