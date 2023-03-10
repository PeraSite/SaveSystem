#if ODIN_INSPECTOR
using System.Text;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Zenject;

namespace SaveSystem.Runtime {
	public class OdinDataSerializer : IDataSerializer {
		[InjectOptional]
		private DataFormat _dataFormat = DataFormat.JSON;

		public string Serialize<T>(T value) {
			return Encoding.UTF8.GetString(SerializationUtility.SerializeValue(value, _dataFormat));
		}

		public T Deserialize<T>(string text) {
			if (text.IsNullOrWhitespace()) return default;
			return SerializationUtility.DeserializeValue<T>(Encoding.UTF8.GetBytes(text), _dataFormat);
		}

		public object Deserialize(string text) {
			if (text.IsNullOrWhitespace()) return null;
			return SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(text), _dataFormat);
		}
	}
}

#endif
