using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class FadeSceneTransition : ScriptableObjectInstaller<FadeSceneTransition>, ISceneTransition,
		IInitializable, IDisposable {
		[SerializeField] private GameObject _prefab;
		[SerializeField] private float _animationTime;

		private CanvasGroup _fade;

		public async UniTask StartTransition() {
			_fade.DOKill();
			_fade.alpha = 0f;
			_fade.gameObject.SetActive(true);
			await _fade.DOFade(1f, _animationTime).AsyncWaitForCompletion();
		}

		public async UniTask EndTransition() {
			_fade.DOKill();
			_fade.alpha = 1f;
			await _fade.DOFade(0f, _animationTime).AsyncWaitForCompletion();
			_fade.gameObject.SetActive(false);
		}

		public void Initialize() {
			var canvas = Instantiate(_prefab, ProjectContext.Instance.transform);
			_fade = canvas.GetComponentInChildren<CanvasGroup>(true);
		}

		public void Dispose() {
			_fade.DOKill();
			_fade = null;
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<FadeSceneTransition>().FromInstance(this).AsSingle();
		}
	}
}
