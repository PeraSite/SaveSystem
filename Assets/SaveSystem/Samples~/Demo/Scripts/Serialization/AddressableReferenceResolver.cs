#if ODIN_INSPECTOR
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveSystem.Samples.Serialization {
	public class AddressableReferenceResolver : IExternalStringReferenceResolver {
		public IExternalStringReferenceResolver NextResolver { get; set; }

		public bool TryResolveReference(string id, out object value) {
			var locations = Addressables.LoadResourceLocationsAsync(id).WaitForCompletion();

			if (locations.Count == 0) {
				value = null;
				return false;
			}

			var location = locations.First();
			var loadAsset = Addressables.LoadAssetAsync<ScriptableObject>(location).WaitForCompletion();

			value = loadAsset;
			return true;
		}

		public bool CanReference(object value, out string id) {
			if (value is ScriptableObject unityObject) {
				id = unityObject.name;

				var locations = Addressables.LoadResourceLocationsAsync(unityObject.name).WaitForCompletion();

				// 만약 Addressable Asset을 찾았다면 true
				if (locations.Count != 0) return true;

				Debug.LogError($"Can't find reference to {unityObject.name} from Addressable!");
				return false;

			}

			id = null;
			return false;
		}
	}
}

  #endif
