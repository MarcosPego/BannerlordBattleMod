using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerlordBattleMod
{
    public class ExtentionsHelper
    {
        public static IEnumerable<IEnumerable<T>> SplitHelper<T>(IEnumerable<T> source, int chunkSize)
        {
            var chunk = new List<T>(chunkSize);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk.Clear();
                }
            }
            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }
    }
}
