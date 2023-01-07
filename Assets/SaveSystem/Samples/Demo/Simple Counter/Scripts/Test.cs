using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SaveSystem.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class Test : MonoBehaviour {
		public SaveManager _manager;

		private void Awake() {
			Debug.Log("awake");
		}

		private void Start() {
			Debug.Log("start");
		}

		[Inject]
		public void Construct(SaveManager manager) {
			Debug.Log("construct");
			_manager = manager;
		}

		[Button]
		public void TestSerialize(SaveData dict) {
			Debug.Log(JsonConvert.SerializeObject(dict));
		}
	}
}
