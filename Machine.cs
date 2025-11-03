using System;

namespace DumpProtoData
{
    [Serializable]
    public class Machine: Item
    {
        public int assemblerSpeed;
        public long workEnergyPerTick;
        public long idleEnergyPerTick;
        public long exchangeEnergyPerTick;
        public long genEnergyPerTick;
        public long useFuelPerTick;
        public bool isPowerConsumer;
        public bool isPowerExchanger;
        public bool isPowerGen;
    }
}
