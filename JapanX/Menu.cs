using System;
using System.Reflection;
using Binding.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JapanX
{
	// Token: 0x02000004 RID: 4
	public class Menu : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002684 File Offset: 0x00000884
		public void SwitchToJpXMenu()
		{
			if (this.menuContext != null)
			{
				this.menuContext.GetType().GetMethod("SwitchPageTo", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.menuContext, new object[]
				{
					-1
				});
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026D4 File Offset: 0x000008D4
		private GameObject CreateButton(Transform basebtn, string name)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(basebtn.gameObject, this.JapanXMenu.transform);
			gameObject.name = name;
			GameObject gameObject2 = gameObject.FindChildWithName("SelectedBG");
			foreach (object obj in gameObject2.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			VisibilityBoolBinding component = gameObject2.GetComponent<VisibilityBoolBinding>();
			foreach (ColorToggleBinding obj2 in gameObject.GetComponentsInChildren<ColorToggleBinding>(true))
			{
				this.CTBPath.SetValue(obj2, name);
			}
			Graphic component2 = gameObject2.GetComponent<Image>();
			this.VBBPath.SetValue(component, name);
			component2.color = new Color(0f, 0f, 0f, 255f);
			return gameObject;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000027C8 File Offset: 0x000009C8
		private void FixOffsets(GameObject btn, Vector2 offMin, Vector2 offMax)
		{
			RectTransform component = btn.GetComponent<RectTransform>();
			component.offsetMin = offMin;
			component.offsetMax = offMax;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000027E0 File Offset: 0x000009E0
		public void BuildJpXMenu(UIMainMenuContext context)
		{
			this.menuContext = context;
			if (this.menuContext && this.JapanXMenu == null)
			{
				GameObject gameObject = this.menuContext.transform.Find("Menus/CenterMenu/SingleplayerMenu").gameObject;
				GameObject gameObject2 = this.menuContext.transform.Find("Menus/CenterMenu/MultiplayerMenuConsole").gameObject;
				this.JapanXMenu = UnityEngine.Object.Instantiate<GameObject>(gameObject.gameObject);
				this.JapanXMenu.name = "JapanXMenu";
				this.JapanXMenu.transform.SetParent(gameObject.transform.parent, false);
				this.JapanXMenu.GetComponent<VisibilityBoolBinding>().reference = -1.0;
				foreach (object obj in this.JapanXMenu.transform)
				{
					UnityEngine.Object.Destroy(((Transform)obj).gameObject);
				}
				GameObject gameObject3 = this.CreateButton(gameObject.transform.Find("TrainingMode"), "jpXSingleplayer");
				this.FixOffsets(gameObject3, new Vector2(100f, -188f), new Vector2(645f, 0f));
				GameObject gameObject4 = this.CreateButton(gameObject2.transform.Find("Rooms"), "jpXMultiplayer");
				this.FixOffsets(gameObject4, new Vector2(100f, -383f), new Vector2(645f, -195f));
				GameObject gameObject5 = this.CreateButton(gameObject.transform.Find("Garage"), "jpXGarage");
				ElementNavigate component = gameObject3.GetComponent<ElementNavigate>();
				ElementNavigate component2 = gameObject4.GetComponent<ElementNavigate>();
				ElementNavigate component3 = gameObject5.GetComponent<ElementNavigate>();
				component.up = null;
				component.down = component2;
				component.left = null;
				component.right = null;
				component2.up = component;
				component2.down = component3;
				component2.left = null;
				component2.right = null;
				component3.up = component2;
				component3.down = null;
				component3.left = null;
				component3.right = null;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002A14 File Offset: 0x00000C14
		private void OnGUI()
		{
			if (this.ShowWatermark && SceneManager.GetActiveScene().name == "SelectCar")
			{
				GUI.Label(this.labelrect, string.Format("JapanX {0} (Preview)\nDevelopment version", JapanX.version));
			}
		}

		// Token: 0x0400000E RID: 14
		private FieldInfo VBBPath = typeof(VisibilityBoolBinding).BaseType.BaseType.GetField("m_path", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400000F RID: 15
		private FieldInfo CTBPath = typeof(ColorToggleBinding).BaseType.BaseType.GetField("m_path", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000010 RID: 16
		private UIMainMenuContext menuContext;

		// Token: 0x04000011 RID: 17
		private GameObject JapanXMenu;

		// Token: 0x04000012 RID: 18
		public bool ShowWatermark;

		// Token: 0x04000013 RID: 19
		private Rect labelrect = new Rect(0f, 0f, 400f, 200f);
	}
}
