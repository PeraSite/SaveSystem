using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class UnityJsonDataSerializer : ScriptableObjectInstaller<UnityJsonDataSerializer>, IDataSerializer {
		public string Serialize<T>(T value) {
			return JsonUtility.ToJson(value);
		}

		public T Deserialize<T>(string text) {
			return JsonUtility.FromJson<T>(text);
		}

		public object Deserialize(string text) {
			return JsonUtility.FromJson<object>(text);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<UnityJsonDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}
