using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Helper
{
    public static class CollectionExt
    {
        //Inspiration:  https://github.com/DidacticalEnigma/DidacticalEnigma.Core/blob/main/Utility/Utils/EnumerableExt.cs#L73
        //Credits:      https://github.com/milleniumbug
        public static IEnumerable<T> ChunkBy<T>(this Queue<T> input, uint n)
        {
            int initCount = input.Count;
            for (int i = 0; i < initCount; i++)
            {
                if (i == n)
                    break;
                yield return input.Dequeue();
            }
        }
    }
}
