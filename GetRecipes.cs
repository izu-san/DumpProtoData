using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace DumpProtoData
{
    public class GetRecipes
    {
        private static readonly string _dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public void Run()
        {
            SaveListToXmlFile(Path.Combine(_dir, "Items", "Items.xml"), GetItemFromGame());
            SaveListToXmlFile(Path.Combine(_dir, "Machines", "Machines.xml"), GetMachineFromGame());
            SaveListToXmlFile(Path.Combine(_dir, "Recipes", "Recipes.xml"), GetRecipeFromGame());
            SaveTechIcon();
        }

        private static List<Item> GetItemFromGame()
        {
            var items = new List<Item>();
            foreach (var itemProto in LDB.items.dataArray)
            {
                if (itemProto == null) continue;
                SaveSpriteToPngFile(Path.Combine(_dir, "Items", "Icons", $"{itemProto.ID}.png"), itemProto.iconSprite);
                PrefabDesc prefabDesc = itemProto.prefabDesc;
                if (prefabDesc != null && (prefabDesc.isPowerConsumer || prefabDesc.isPowerExchanger))
                {
                    continue;
                }
                var item = new Item
                {
                    id = itemProto.ID,
                    name = itemProto.Name.Translate(),
                    Type = (EItemType)itemProto.Type,
                    miningFrom = itemProto.MiningFrom.Translate(),
                    produceFrom = itemProto.ProduceFrom.Translate(),
                    isRaw = itemProto.isRaw,
                    isFluid = itemProto.IsFluid,
                    HeatValue = itemProto.HeatValue
                };
                items.Add(item);
            }
            return items;
        }

        private static List<Machine> GetMachineFromGame()
        {
            var machines = new List<Machine>();
            foreach (var itemProto in LDB.items.dataArray)
            {
                if (itemProto == null) continue;
                SaveSpriteToPngFile(Path.Combine(_dir, "Machines", "Icons", $"{itemProto.ID}.png"), itemProto.iconSprite);
                PrefabDesc prefabDesc = itemProto.prefabDesc;
                if (prefabDesc != null && (prefabDesc.isPowerConsumer || prefabDesc.isPowerExchanger || prefabDesc.isPowerGen))
                {
                    var machine = new Machine
                    {
                        id = itemProto.ID,
                        name = itemProto.Name.Translate(),
                        Type = (EItemType)itemProto.Type,
                        miningFrom = itemProto.MiningFrom.Translate(),
                        produceFrom = itemProto.ProduceFrom.Translate(),
                        isRaw = itemProto.isRaw,
                        assemblerSpeed = prefabDesc.assemblerSpeed,
                        workEnergyPerTick = prefabDesc.workEnergyPerTick,
                        idleEnergyPerTick = prefabDesc.idleEnergyPerTick,
                        exchangeEnergyPerTick = prefabDesc.exchangeEnergyPerTick,
                        genEnergyPerTick = prefabDesc.genEnergyPerTick,
                        useFuelPerTick = prefabDesc.useFuelPerTick,
                        isPowerConsumer = prefabDesc.isPowerConsumer,
                        isPowerExchanger = prefabDesc.isPowerExchanger,
                        isPowerGen = prefabDesc.isPowerGen
                    };
                    machines.Add(machine);
                }
            }
            return machines;
        }

        private static List<Recipe> GetRecipeFromGame()
        {
            var recipes = new List<Recipe>();
            foreach (var recipeProto in LDB.recipes.dataArray)
            {
                if (recipeProto == null) continue;
                List<Item> items = GetItemRecipes(recipeProto.Items, recipeProto.ItemCounts);
                List<Item> results = GetItemRecipes(recipeProto.Results, recipeProto.ResultCounts);
                var recipe = new Recipe
                {
                    SID = recipeProto.SID,
                    name = recipeProto.Name.Translate(),
                    Type = (ERecipeType)recipeProto.Type,
                    Explicit = recipeProto.Explicit,
                    TimeSpend = recipeProto.TimeSpend,
                    Items = items,
                    Results = results,
                    GridIndex = recipeProto.GridIndex,
                    productive = recipeProto.productive
                };
                recipes.Add(recipe);
                SaveSpriteToPngFile(Path.Combine(_dir, "Recipes", "Icons", $"{recipeProto.SID}.png"), recipeProto.iconSprite);
            }
            return recipes;
        }

        private static void SaveTechIcon()
        {
            foreach (var techProto in LDB.techs.dataArray)
            {
                if (techProto == null) continue;
                try
                {
                    SaveSpriteToPngFile(Path.Combine(_dir, "Techs", "Icons", $"{techProto.name}.png"), techProto.iconSprite);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static List<Item> GetItemRecipes(int[] itemIds, int[] itemCounts)
        {
            List<Item> list = new List<Item>();
            for (int i = 0; i < itemIds.Length; i++)
            {
                ItemProto itemProto = LDB.items.Select(itemIds[i]);
                list.Add(new Item
                {
                    id = itemProto.ID,
                    name = itemProto.name,
                    count = itemCounts[i],
                    isRaw = itemProto.isRaw
                });
            }
            return list;
        }

        private static void SaveSpriteToPngFile(string filePath, Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
            {
                return;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Texture2D texture2D = sprite.texture;
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }

        private void SaveListToXmlFile<T>(string filePath, List<T> list)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, list);
            }
        }
    }
}
