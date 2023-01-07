using SaveSystem.Runtime;
using TMPro;
using UnityEngine;

namespace SaveSystem.Samples {
	public class CounterSaver : Saver<int> {
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private int _value;

		public void Increase() {
			_value++;
			_text.text = _value.ToString();
		}

		public override void ResetData() {
			_value = 0;
			_text.text = _value.ToString();
		}

		public override void ApplyData(int data) {
			_value = data;
			_text.text = _value.ToString();
		}

		public override int SaveData() {
			return _value;
		}
	}
}
