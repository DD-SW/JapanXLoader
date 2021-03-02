using System;
using HarmonyLib;
using UnityEngine;

namespace JapanX
{
	// Token: 0x02000005 RID: 5
	internal class Patches
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002ADA File Offset: 0x00000CDA
		[HarmonyPatch(typeof(ModelValidator), "isValid", MethodType.Getter)]
		[HarmonyPrefix]
		private static bool ModelValidator_isValid_Getter_Patch(ref bool __result)
		{
			__result = !JapanX.IsJapanXActive;
			Debug.Log(string.Format("IsValid: {0}", __result));
			return false;
		}

		// Token: 0x0600000F RID: 15
		[HarmonyPatch(typeof(UITopPanelContext), "ShowPanel")]
		[HarmonyPrefix]
		private static bool UITopPanelContext_ShowPanel_Patch(ETopPanelState newState, string title, ref HeaderActionData[] actions)
		{
			if (actions != null && actions[0].text.ToLower() == LocalizationKeys.screen_title_singleplayer.Localized().ToLower())
			{
				HeaderActionData[] array = (HeaderActionData[])actions.Clone();
				actions = new HeaderActionData[]
				{
					array[0],
					array[1],
					new HeaderActionData
					{
						text = "JapanX",
						action = new Action(JapanX.jpX_Menu.SwitchToJpXMenu)
					},
					array[2]
				};
			}
			return true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002BC0 File Offset: 0x00000DC0
		[HarmonyPatch(typeof(UIMainMenuContext), "OnActivate")]
		[HarmonyPostfix]
		private static void UIMainMenuContext_OnActivate_Patch(ref UIMainMenuContext __instance)
		{
			if (JapanX.jpX_Menu != null)
			{
				JapanX.jpX_Menu.BuildJpXMenu(__instance);
				return;
			}
			Debug.LogError("Error building JapanX Menu");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002BE6 File Offset: 0x00000DE6
		[HarmonyPatch(typeof(UIMainMenuContext), "SwitchPageTo")]
		[HarmonyPostfix]
		private static void UIMainMenuContext_SwitchPageTo_Patch(int index)
		{
			JapanX.jpX_Loader.mapLoaded = false;
			if (index == -1)
			{
				JapanX.IsJapanXActive = true;
				JapanX.jpX_Menu.ShowWatermark = true;
				return;
			}
			JapanX.IsJapanXActive = false;
			JapanX.jpX_Menu.ShowWatermark = false;
		}
	}
}
