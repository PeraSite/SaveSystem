using System.Collections.Generic;

namespace SaveSystem.Samples {
	public class Inventory {
		public Dictionary<int, ItemStack> Items = new Dictionary<int, ItemStack>();
	}

	public struct ItemStack {
		public Item Item;
		public int Amount;
	}
}
