using SaveSystem.Runtime;
using Sirenix.Serialization;

namespace SaveSystem.Samples {
	public class InventorySaver : SerializedSaver<Inventory, SlotScope> {
		[OdinSerialize]
		public Inventory Inventory;

		public override void ApplyData(Inventory data) {
			Inventory = data;
		}

		public override Inventory SaveData() {
			return Inventory;
		}
	}
}
