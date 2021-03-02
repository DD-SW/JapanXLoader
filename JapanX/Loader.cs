using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BepInEx;
using CarX;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JapanX
{
	// Token: 0x02000003 RID: 3
	public class Loader : MonoBehaviour
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002150 File Offset: 0x00000350
		private void CreateMaterials(Transform t, SurfaceType surf, float f1, float f2, float f3, float f4, float f5)
		{
			foreach (Transform transform in t.GetComponentsInChildren<Transform>())
			{
				CarX.Material material = transform.gameObject.AddComponent<CarX.Material>();
				transform.gameObject.AddComponent<CARXSurface>();
				material.SetParameters(surf, f1, f2, f3, f4, f5);
				transform.gameObject.layer = 11;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021AC File Offset: 0x000003AC
		private void LoadJapanXMap()
		{
			this.mapLoaded = false;
			this.spawns.Clear();
			Stopwatch stopwatch = Stopwatch.StartNew();
			if (this.prefab == null)
			{
				this.mapBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "data.japanx"), 0U, (ulong)JapanX.MapOffset);
				if (this.mapBundle == null)
				{
					UnityEngine.Debug.Log("Failed to load AssetBundle!");
					return;
				}
				this.prefab = this.mapBundle.LoadAsset<GameObject>("ModMapRoot");
				this.mapBundle.Unload(false);
			}
			this.map = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			foreach (object obj in this.map.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == "road")
				{
					this.CreateMaterials(transform, SurfaceType.Asphalt, 1f, 0.01f, 0f, 0f, 100f);
				}
				else if (transform.gameObject.name == "kerb")
				{
					this.CreateMaterials(transform, SurfaceType.Asphalt, 1f, 0.015f, -0.007f, 0f, 12f);
				}
				else if (transform.gameObject.name == "sand")
				{
					this.CreateMaterials(transform, SurfaceType.Sand, 1f, 0.13f, 0.06f, 0.5f, 25f);
				}
				else if (transform.gameObject.name == "snow")
				{
					this.CreateMaterials(transform, SurfaceType.Snow, 2f, 0.1f, -0.01f, -0.1f, 30f);
				}
				else if (transform.gameObject.name == "grass")
				{
					this.CreateMaterials(transform, SurfaceType.Grass, 1f, 0f, 0.05f, 0.04f, 25f);
				}
				else if (transform.gameObject.name == "gravel")
				{
					this.CreateMaterials(transform, SurfaceType.Sand, 2f, 0.1f, -0.01f, -0.01f, 30f);
				}
				else if (transform.gameObject.name == "icyroad")
				{
					this.CreateMaterials(transform, SurfaceType.Asphalt, 0.73f, 0.025f, 0f, 0f, 30f);
				}
				else if (transform.gameObject.name == "dirt")
				{
					this.CreateMaterials(transform, SurfaceType.Earth, 1f, 0.025f, -0.007f, 0f, 12f);
				}
				if (transform.gameObject.name.StartsWith("spawn_"))
				{
					this.spawns.Add(transform.gameObject);
				}
			}
			GameObject.Find("ReflectionProbe").SetActive(false);
			GameObject go = GameObject.Find("Main Camera");
			this.rtProbe = go.FindChildWithName("realtimeReflectionProbe");
			this.rtProbe.SetActive(true);
			this.mapLoaded = true;
			stopwatch.Stop();
			UnityEngine.Debug.Log(string.Format("[JapanX]: INFO: Map load took {0}ms", stopwatch.ElapsedMilliseconds));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002510 File Offset: 0x00000710
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F8) && SceneManager.GetActiveScene().name != "SelectCar" && JapanX.IsJapanXActive)
			{
				if (this.mapLoaded)
				{
					this.rtProbe.SetActive(false);
					this.rtProbe.SetActive(true);
					return;
				}
				if (SceneManager.GetActiveScene().name != "Airfield")
				{
					UnityEngine.Object.Destroy(GameObject.Find("map"));
					this.LoadJapanXMap();
					if (this.spawns.Count <= 0)
					{
						UnityEngine.Debug.LogWarning("[JapanX]: WARNING: Map does not have spawns");
						return;
					}
					RaceCar raceCar = null;
					RaceCar[] array = UnityEngine.Object.FindObjectsOfType<RaceCar>();
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].isNetworkCar)
						{
							raceCar = array[i];
							break;
						}
					}
					if (raceCar != null)
					{
						raceCar.getTransform.position = this.spawns[0].transform.position;
						raceCar.getTransform.rotation = this.spawns[0].transform.rotation;
						Rigidbody getRigidbody = raceCar.carX.getRigidbody;
						getRigidbody.velocity = Vector3.zero;
						getRigidbody.angularVelocity = Vector3.zero;
						return;
					}
				}
				else
				{
					UIMessageBox.ShowHint("Navaro is not supported", "JapanX", null, null);
					UnityEngine.Debug.LogError("[JapanX]: ERROR: Maps not supported for Navaro");
				}
			}
		}

		// Token: 0x04000008 RID: 8
		public bool mapLoaded;

		// Token: 0x04000009 RID: 9
		private List<GameObject> spawns = new List<GameObject>();

		// Token: 0x0400000A RID: 10
		private GameObject rtProbe;

		// Token: 0x0400000B RID: 11
		private AssetBundle mapBundle;

		// Token: 0x0400000C RID: 12
		private GameObject prefab;

		// Token: 0x0400000D RID: 13
		private GameObject map;
	}
}
