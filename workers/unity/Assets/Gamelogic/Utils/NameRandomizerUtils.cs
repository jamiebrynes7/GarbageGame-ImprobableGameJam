using System;
using System.Collections.Generic;

namespace Assets.Gamelogic.Utils
{
    public static class NameRandomizerUtils
    {

        private static List<string> prefixes = new List<string> {"Stinky", "Trashy", "Sticky", "Disgusting", "Horrific"};
        private static List<string> basenames = new List<string> {"Trash", "Garbage", "Rubbish"};
        private static List<string> suffixes = new List<string> {"Bag", "Tip", "Dumpster", ""};

        private static Random rnd = new Random();

        public static string GenerateName()
        {
            int preIndex = rnd.Next(prefixes.Count);
            int baseIndex = rnd.Next(basenames.Count);
            int suffixIndex = rnd.Next(suffixes.Count);

            return prefixes[preIndex] + basenames[baseIndex] + suffixes[suffixIndex];
        }
    }
}