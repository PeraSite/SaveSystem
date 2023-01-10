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

		private SlotScope _slotScope;
		private SaveManager _saveManager;

		private void Start() {
			_slotScope.CurrentSlot = SlotScope.NOT_SELECTED_SLOT;

			for (var i = 0; i < _slotButtons.Length; i++) {
				var slot = i;

				var hasData = _slotScope.Snapshot.ContainsKey(slot);

				_slotSceneNames[slot].text =
					hasData ? _slotScope.GetMetadata(slot).SceneName : "Empty";

				_slotButtons[i].onClick.AddListener(() => {
					_slotScope.CurrentSlot = slot;

					if (hasData) {
						_saveManager.StartGame(_slotScope.GetMetadata(slot).SceneName);
					} else {
						_saveManager.NewGame(_startingScene);
					}
				});
			}
		}

		[Inject]
		private void Construct(SaveManager saveManager, SlotScope slotScope) {
			_saveManager = saveManager;
			_slotScope = slotScope;
		}
	}
}
