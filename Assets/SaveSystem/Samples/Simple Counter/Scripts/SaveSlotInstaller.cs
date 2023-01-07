using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SaveSlotInstaller : ScriptableObjectInstaller<SaveSlotInstaller> {
		[SerializeField] private string _slotName = "Custom Slot";

		public override void InstallBindings() {
			Container.BindInstance(_slotName).WhenInjectedInto<SaveManager>();
		}
	}
}
