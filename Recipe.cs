using System;
using System.Collections.Generic;

namespace DumpProtoData
{
    [Serializable]
    public class Recipe
    {
        public string SID;
        public string name;
        public ERecipeType Type;
        public bool Explicit;
        public int TimeSpend;
        public List<Item> Items;
        public List<Item> Results;
        public int GridIndex;
        public bool productive;
    }
}
