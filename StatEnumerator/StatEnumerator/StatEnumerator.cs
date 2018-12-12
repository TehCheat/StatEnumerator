using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PoeHUD.Controllers;
using PoeHUD.Hud.Interfaces;
using PoeHUD.Hud.Settings;
using PoeHUD.Hud.UI;
using PoeHUD.Hud;
using PoeHUD.Framework;
using PoeHUD.Framework.Helpers;
using PoeHUD.Models;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.FilesInMemory;
using PoeHUD.Poe.RemoteMemoryObjects;
using PoeHUD.Poe;
using PoeHUD.Plugins;


namespace StatEnumerator
{
	public class StatEnumerator : BaseSettingsPlugin<StatEnumeratorSettings>
	{
		public override void Initialise()
		{
			PluginName = "StatEnumerator";
		}

		public override void Render()
		{
			// enumerate stats
			StreamWriter file = new StreamWriter("GameStats.txt");

            int iCounter = 1;
			foreach (var statRecord in GameController.Instance.Files.Stats.records)
			{
                string strUserFriendlyName = statRecord.Value.UserFriendlyName;
                string strCodeName = statRecord.Value.UserFriendlyName.Replace(" ", string.Empty);
                string strPrettyString = "[Description(\"" + strUserFriendlyName + "\")] " + strCodeName + " = " + iCounter.ToString() + ";";
                file.WriteLine(strPrettyString);
                iCounter++;
			}
}
	}
}
