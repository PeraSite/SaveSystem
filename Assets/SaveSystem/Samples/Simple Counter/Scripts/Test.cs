using System;
using SaveSystem.Runtime;
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
			_manager = manager;
		}
	}
}
