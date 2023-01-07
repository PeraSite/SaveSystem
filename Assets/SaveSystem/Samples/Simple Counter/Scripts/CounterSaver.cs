using SaveSystem.Runtime;
using TMPro;
using UnityEngine;

namespace SaveSystem.Samples {
	public class CounterSaver : Saver<int> {
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private int _value;

		public void Increase() {
			_value++;
		}

		public override void ResetData() {
			_value = 0;
		}

		public override void ApplyData(int data) {
			_value = data;
		}

		public override int SaveData() {
			return _value;
		}
	}
}
