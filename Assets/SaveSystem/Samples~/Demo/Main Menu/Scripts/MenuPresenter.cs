using SaveSystem.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SaveSystem.Samples {
	public class MenuPresenter : MonoBehaviour {
		[SerializeField] private Button[] _slotButtons;
		[SerializeField] private TextMeshProUGUI[] _slotSceneNames;
		[SerializeField] private string _startingScene;

		private SaveManager _saveManager;

		private void Start() {
			var save = _saveManager.CurrentSnapshot;

			for (var i = 0; i < _slotButtons.Length; i++) {
				var slot = i;
				var key = GetKey(slot);

				_slotSceneNames[slot].text = save.Data.ContainsKey(key) ? save.SceneName : "Empty";

				_slotButtons[i].onClick.AddListener(() => {
					_saveManager.SlotName = key;

					if (save.Data.ContainsKey(key)) {
						_saveManager.StartGame(save.SceneName);
					} else {
						_saveManager.NewGame(_startingScene);
					}
				});
			}
		}

		private static string GetKey(int slot) {
			return $"Slot {slot}";
		}

		[Inject]
		private void Construct(SaveManager saveManager) {
			_saveManager = saveManager;
		}
	}
}
