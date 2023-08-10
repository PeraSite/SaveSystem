using System;
using System.Collections.Generic;
using SaveSystem.Runtime;

namespace SaveSystem.Samples {
	[Serializable]
	public struct Inventory {
		public List<Item> Items;
	}

	public class InventorySaver : Saver<Inventory, SlotScope> {
		public Inventory Inventory;

		public override void ApplyData(Inventory data) {
			Inventory = data;
		}

		public override Inventory SaveData() {
			return Inventory;
		}
	}
}
