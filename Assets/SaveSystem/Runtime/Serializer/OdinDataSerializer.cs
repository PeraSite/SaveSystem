#if ODIN_INSPECTOR
using System.Text;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Zenject;

namespace SaveSystem.Runtime {
	public class OdinDataSerializer : IDataSerializer {
		private DataFormat _dataFormat;
		private IExternalStringReferenceResolver _referenceResolver;

		public string Serialize<T>(T value) {
			var context = new SerializationContext {
				StringReferenceResolver = _referenceResolver
			};

			var serializedBytes = SerializationUtility.SerializeValue(value, _dataFormat, context);
			return Encoding.UTF8.GetString(serializedBytes);
		}

		public T Deserialize<T>(string text) {
			if (text.IsNullOrWhitespace()) return default;

			var context = new DeserializationContext {
				StringReferenceResolver = _referenceResolver
			};

			var serializedBytes = Encoding.UTF8.GetBytes(text);
			return SerializationUtility.DeserializeValue<T>(serializedBytes, _dataFormat, context);
		}

		public object Deserialize(string text) {
			if (text.IsNullOrWhitespace()) return null;

			var context = new DeserializationContext {
				StringReferenceResolver = _referenceResolver
			};

			var serializedBytes = Encoding.UTF8.GetBytes(text);
			return SerializationUtility.DeserializeValueWeak(serializedBytes, _dataFormat, context);
		}

		[Inject]
		private void Construct(
			IExternalStringReferenceResolver referenceResolver = null,
			DataFormat dataFormat = DataFormat.JSON
		) {
			_referenceResolver = referenceResolver;
			_dataFormat = dataFormat;
		}
	}
}

#endif
