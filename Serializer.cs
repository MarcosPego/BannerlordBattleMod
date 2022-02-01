using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle;

namespace BannerlordBattleMod
{
    public class Serializer
    {

		public static IEnumerable<CustomBattleSceneData> CustomBattleScenes
		{
			get
			{
				return _customBattleScenes;
			}
		}

		private static List<CustomBattleSceneData> _customBattleScenes;

		public static void ParseCustomBattleScenes(bool is_custom_map = true)
        {
			_customBattleScenes = new List<CustomBattleSceneData>();
			XmlDocument customBattleSceneXML = LoadCustomBattleSceneXML(is_custom_map);
            bool flag = customBattleSceneXML.ChildNodes[0].Name.ToLower().Equals("xml");
            XmlNode xmlNode = flag ? customBattleSceneXML.ChildNodes[1] : customBattleSceneXML.ChildNodes[0];
            if (xmlNode.Name == "CustomBattleScenes")
            {
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string sceneID = null;
						TextObject name = null;
						TerrainType terrain = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						bool isSiegeMap = false;
						bool isVillageMap = false;
						bool isLordsHallMap = false;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								sceneID = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "name")
							{
								name = new TextObject(xmlNode2.Attributes[i].InnerText, null);
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrain))
								{
									terrain = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array[0] = char.ToUpper(array[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "is_siege_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out isSiegeMap);
							}
							else if (xmlNode2.Attributes[i].Name == "is_village_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out isVillageMap);
							}
							else if (xmlNode2.Attributes[i].Name == "is_lords_hall_map")
							{
								bool.TryParse(xmlNode2.Attributes[i].InnerText, out isLordsHallMap);
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "flags")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType item;
									if (xmlNode4.NodeType != XmlNodeType.Comment && xmlNode4.Attributes["name"].InnerText == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["value"].InnerText, out item) && !list.Contains(item))
									{
										list.Add(item);
									}
								}
							}
						}
						_customBattleScenes.Add(new CustomBattleSceneData(sceneID, name, terrain, list, forestDensity, isSiegeMap, isVillageMap, isLordsHallMap));
					}
				}
			}

        }


        public static XmlDocument LoadCustomBattleSceneXML(bool is_custom_map = true)
        {
            var filePath = Path.Combine(SaveFolderPath(is_custom_map), "custom_battle_scenes.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            return doc;
        }




        public static void EnsureSaveDirectory()
        {
            Directory.CreateDirectory(SaveFolderPath());
        }
        public static string SaveFolderPath(bool is_custom_map = true)
        {

            if (is_custom_map)
            {
				string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

				var filePath = Path.Combine(path, "ModuleData");

				return filePath;
			}
			else
            {
				string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "CustomBattle"));

				var filePath = Path.Combine(path, "ModuleData");

				return filePath;
			}

        }
    }
}
