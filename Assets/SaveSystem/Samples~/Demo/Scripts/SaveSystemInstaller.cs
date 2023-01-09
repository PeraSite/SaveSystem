using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SaveSystemInstaller : MonoInstaller<SaveSystemInstaller> {
		[SerializeField] private CanvasGroup _fade;
		[SerializeField] private float _animationTime;

		public override void InstallBindings() {
			// 구현체는 Self Bind 하지 않고 인터페이스만 등록
			Container.BindInterfacesTo<OdinDataSerializer>().AsSingle().NonLazy();
			Container.BindInterfacesTo<PlayerPrefDataStorage>().AsSingle().NonLazy();
			Container.BindInterfacesTo<FadeSceneTransition>().AsSingle().WithArguments(_fade, _animationTime).NonLazy();

			// SaveManager를 Self Bind
			Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
		}
	}
}
