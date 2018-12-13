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
        bool bFirstTime;
		public override void Initialise()
		{
            bFirstTime = true;
			PluginName = "StatEnumerator";
		}

        private Tuple<string, string> ParseRawName(string strRawName)
        {
            string strPrettyName = "";
            string strCodeName = "";
            // force the first char to be a capital letter
            bool bNextLetterIsCap = true;
            foreach (char c in strRawName)
            {
                if (char.IsLetter(c))
                {
                    if (bNextLetterIsCap)
                    {
                        strPrettyName += char.ToUpper(c);
                    }
                    else
                    {
                        strPrettyName += c;
                    }
                    bNextLetterIsCap = false;
                }
                else if (c =='%')
                {
                    strPrettyName += c;
                    strCodeName += "Pct";
                }
                else if (c == '+')
                {
                    strPrettyName += c;
                    strCodeName += "Pos";
                }
                else if (c == '-')
                {
                    strPrettyName += c;
                    strCodeName += "Neg";
                }
                else if (c == '_')
                {
                    strPrettyName += ' ';
                    // code name drops the '_' altogether
                    bNextLetterIsCap = true;
                }
            }
            return new Tuple<string, string>(strPrettyName, strCodeName);
        }

        public override void Render()
        {
            if (bFirstTime)
            {
                // enumerate stats
                StreamWriter file = new StreamWriter("GameStats.txt");

                int iCounter = 1;
                foreach (var statRecord in GameController.Instance.Files.Stats.records)
                {
                    var strStrings = ParseRawName(statRecord.Key);
                    string strPrettyString = "[Description(\"" + strStrings.Item1 + "\")] " + strStrings.Item2 + " = " + iCounter.ToString() + ";";
                    file.WriteLine(strPrettyString);
                    iCounter++;
                }
                bFirstTime = false;
            }
        }
	}
}
