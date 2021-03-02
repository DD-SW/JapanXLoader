using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace JapanX
{
	// Token: 0x02000002 RID: 2
	public class JapanX
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static JapanX()
		{
			new Harmony("com.japanx.patcher").PatchAll(typeof(Patches));
			JapanX.jpX = new GameObject("JapanXBeta");
			UnityEngine.Object.DontDestroyOnLoad(JapanX.jpX);
			JapanX.jpX_Loader = JapanX.jpX.AddComponent<Loader>();
			JapanX.jpX_Menu = JapanX.jpX.AddComponent<Menu>();
			JapanX.version = Assembly.GetExecutingAssembly().GetName().Version;
			Console.WriteLine(string.Format("[JapanXBeta]: Loaded JapanX version {0}", JapanX.version));
			JapanX.Initialize();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020DC File Offset: 0x000002DC
		private static void Initialize()
		{
			using (BinaryReader binaryReader = new BinaryReader(new FileStream(Path.Combine(Paths.PluginPath, "data.japanx"), FileMode.Open)))
			{
				binaryReader.BaseStream.Seek(44L, SeekOrigin.Begin);
				JapanX.MapOffset = binaryReader.ReadUInt32();
				JapanX.MapCRC = binaryReader.ReadUInt32();
			}
		}

		// Token: 0x04000001 RID: 1
		public static GameObject jpX;

		// Token: 0x04000002 RID: 2
		public static Loader jpX_Loader;

		// Token: 0x04000003 RID: 3
		public static Menu jpX_Menu;

		// Token: 0x04000004 RID: 4
		public static bool IsJapanXActive;

		// Token: 0x04000005 RID: 5
		public static uint MapOffset;

		// Token: 0x04000006 RID: 6
		public static uint MapCRC;

		// Token: 0x04000007 RID: 7
		public static Version version;
	}
}
