using Newtonsoft.Json;
using Zenject;

namespace SaveSystem.Runtime {
	public class NewtonJsonDataSerializer : ScriptableObjectInstaller<NewtonJsonDataSerializer>, IDataSerializer {
		public string Serialize<T>(T value) {
			return JsonConvert.SerializeObject(value);
		}

		public T Deserialize<T>(string text) {
			return JsonConvert.DeserializeObject<T>(text);
		}

		public object Deserialize(string text) {
			return JsonConvert.DeserializeObject(text);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<NewtonJsonDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}
