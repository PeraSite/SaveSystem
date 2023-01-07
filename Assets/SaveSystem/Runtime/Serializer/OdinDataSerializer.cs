#if ODIN_INSPECTOR
using System.Text;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class OdinDataSerializer : ScriptableObjectInstaller<OdinDataSerializer>, IDataSerializer {
		[SerializeField] private DataFormat _dataFormat;

		public string Serialize<T>(T value) {
			return Encoding.UTF8.GetString(SerializationUtility.SerializeValue(value, _dataFormat));
		}

		public T Deserialize<T>(string text) {
			return SerializationUtility.DeserializeValue<T>(Encoding.UTF8.GetBytes(text), _dataFormat);
		}

		public object Deserialize(string text) {
			return SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(text), _dataFormat);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<OdinDataSerializer>().FromInstance(this).AsSingle();
		}
	}
}

#endif
