using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class FadeSceneTransition : ISceneTransition {
		private CanvasGroup _fade;
		private float _animationTime;

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

		[Inject]
		public void Construct(CanvasGroup fade, float animationTime) {
			_fade = fade;
			_animationTime = animationTime;
		}
	}
}
