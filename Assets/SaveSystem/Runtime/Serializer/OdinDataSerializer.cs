using System.Text;
using Sirenix.Serialization;
using Zenject;

namespace SaveSystem.Runtime {
	public class OdinDataSerializer : ScriptableObjectInstaller<OdinDataSerializer>, IDataSerializer {
		public string Serialize<T>(T value) {
			var bytes = SerializationUtility.SerializeValue(value, DataFormat.JSON);
			return Encoding.UTF8.GetString(bytes);
		}

		public T Deserialize<T>(string text) {
			var bytes = Encoding.UTF8.GetBytes(text);
			return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<OdinDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}
