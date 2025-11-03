using System;

namespace DumpProtoData
{
    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int count;
        public EItemType Type;
        public string miningFrom;
        public string produceFrom;
        public bool isRaw;
        public bool isFluid;
        public long HeatValue;
    }
}
