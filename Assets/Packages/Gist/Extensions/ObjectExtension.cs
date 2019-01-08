using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace nobnak.Gist.ObjectExt {

    public static class ObjectExtension {
        public static void Destroy(this Object obj, float t = 0f) {
			if (obj != null) {
				if (Application.isPlaying)
					Object.Destroy(obj, t);
				else
					Object.DestroyImmediate(obj);
			}
        }
		public static void Destroy(this Component comp, float t = 0f) {
			if (comp != null)
				comp.gameObject.Destroy(t);
		}
		public static T DeepCopy<T>(this T src) {
			var json = JsonUtility.ToJson(src);
			return JsonUtility.FromJson<T>(json);
		}

#if UNITY_EDITOR
		public static string AssetFolderName(this Object obj) {
			string folder = null;
			try {
				folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
				while (!Directory.Exists(folder)) {
					if (string.IsNullOrEmpty(folder)) {
						folder = "Assets";
						break;
					}
					folder = Directory.GetParent(folder).FullName;
				}
			} catch {
				folder = "Assets";
			}

			return folder;
		}
		public static IEnumerable<T> GetAssets<T>(this Object obj) where T : Object {
			var path = AssetDatabase.GetAssetPath(obj);
			foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path)) {
				if (asset is T)
					yield return (T)asset;
			}
		}
#endif
	}
}
