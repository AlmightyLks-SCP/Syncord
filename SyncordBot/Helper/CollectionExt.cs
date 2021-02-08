using System.Collections.Generic;

namespace SyncordBot.Helper
{
    public static class CollectionExt
    {
        //Inspiration:  https://github.com/DidacticalEnigma/DidacticalEnigma.Core/blob/main/Utility/Utils/EnumerableExt.cs#L73
        //Credits:      https://github.com/milleniumbug
        public static T[] ChunkBy<T>(this Queue<T> input, int n)
        {
            int initCount = (n > input.Count ? input.Count : n);
            T[] result = new T[initCount];
            for (int i = 0; i < initCount; i++)
                result[i] = input.Dequeue();
            return result;
        }
    }
}
