using BepInEx;
using BepInEx.Configuration;
using System;
using xiaoye97;

namespace DumpProtoData
{
    [BepInDependency(LDBToolPlugin.MODGUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class Plugin: BaseUnityPlugin
    {
        public GetRecipes GetRecipes { get; } = new GetRecipes();

        public void Start()
        {
            LDBTool.PostAddDataAction = (Action)Delegate.Combine(LDBTool.PostAddDataAction, new Action(GetRecipes.Run));
        }
    }
}
