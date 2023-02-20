using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SaveSystemInstaller : MonoInstaller<SaveSystemInstaller> {
		[SerializeField] private CanvasGroup _fade;
		[SerializeField] private float _animationTime;

		public override void InstallBindings() {
			// Base
			Container.BindInterfacesTo<NewtonJsonDataSerializer>().AsSingle().NonLazy();
			Container.BindInterfacesTo<PlayerPrefDataStorage>().AsSingle().NonLazy();
			Container.BindInterfacesTo<FadeSceneTransition>().AsSingle().WithArguments(_fade, _animationTime).NonLazy();
			Container.Bind<SceneTransitionManager>().AsSingle().NonLazy();

			// Scope
			BindScope<GlobalScope>();
			BindScope<SlotScope>();
			BindScope<RootScope>(true);

			// Core Manager
			Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
		}

		private void BindScope<T>(bool isRoot = false) where T : IScope {
			var binder = Container.BindInterfacesAndSelfTo<T>().AsSingle();
			if (isRoot) {
				binder.NonLazy();
			} else {
				// root이 아니라면 SaveManager에 Inject하지 않음
				binder.WhenNotInjectedInto<SaveManager>().NonLazy();
			}
		}
	}
}
