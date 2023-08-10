using SaveSystem.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem.Samples {
	public class SettingSaver : Saver<Settings, GlobalScope> {
		[SerializeField] private Slider _musicSlider;
		[SerializeField] private Slider _soundSlider;

		private Settings _settings;

		public override void ResetData() {
			ApplyData(new Settings {
				MusicVolume = 0.5f,
				SoundVolume = 0.5f
			});
		}

		public override void ApplyData(Settings data) {
			_settings = data;
			_musicSlider.value = data.MusicVolume;
			_soundSlider.value = data.SoundVolume;
		}

		public override Settings SaveData() {
			_settings.MusicVolume = _musicSlider.value;
			_settings.SoundVolume = _soundSlider.value;
			return _settings;
		}
	}

	public struct Settings {
		public float MusicVolume;
		public float SoundVolume;
	}
}
