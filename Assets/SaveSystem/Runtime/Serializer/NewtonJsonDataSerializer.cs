using Newtonsoft.Json;
using Zenject;

namespace SaveSystem.Runtime {
	public class NewtonJsonDataSerializer : ScriptableObjectInstaller<NewtonJsonDataSerializer>, IDataSerializer {
		private struct Wrapper<T> {
			public T Value;
		}

		public string Serialize<T>(T value) {
			var wrapped = new Wrapper<T> {Value = value};
			return JsonConvert.SerializeObject(wrapped);
		}

		public T Deserialize<T>(string text) {
			return JsonConvert.DeserializeObject<Wrapper<T>>(text).Value;
		}

		public object Deserialize(string text) {
			return JsonConvert.DeserializeObject<Wrapper<object>>(text).Value;
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<NewtonJsonDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}
