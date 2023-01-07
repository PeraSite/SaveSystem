#if ODIN_INSPECTOR
using System.Text;
using Sirenix.Serialization;
using Zenject;

namespace SaveSystem.Runtime {
	public class OdinDataSerializer : ScriptableObjectInstaller<OdinDataSerializer>, IDataSerializer {
		public string Serialize<T>(T value) {
			return Encoding.UTF8.GetString(SerializationUtility.SerializeValue(value, DataFormat.JSON));
		}

		public T Deserialize<T>(string text) {
			return SerializationUtility.DeserializeValue<T>(Encoding.UTF8.GetBytes(text), DataFormat.JSON);
		}

		public object Deserialize(string text) {
			return SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(text), DataFormat.JSON);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<OdinDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}

#endif
