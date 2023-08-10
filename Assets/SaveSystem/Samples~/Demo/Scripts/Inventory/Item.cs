using UnityEngine;

namespace SaveSystem.Samples {
	public class Item : ScriptableObject {
		[field: SerializeField] public string DisplayName { get; private set; }
	}
}
