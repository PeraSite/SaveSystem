using System.Collections.Generic;
using SaveSystem.Runtime;
using SaveSystem.Samples.Serialization;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SaveSystemInstaller : MonoInstaller<SaveSystemInstaller> {
		[SerializeField] private DataFormat _dataFormat;
		[SerializeField] private CanvasGroup _fade;
		[SerializeField] private float _animationTime;

		public override void InstallBindings() {
			// Base
			Container.BindInterfacesTo<OdinDataSerializer>().AsSingle().WithArguments(_dataFormat).NonLazy();
			Container.BindInterfacesTo<PlayerPrefDataStorage>().AsSingle().NonLazy();
			Container.BindInterfacesTo<FadeSceneTransition>().AsSingle().WithArguments(_fade, _animationTime).NonLazy();
			Container.Bind<SceneTransitionManager>().AsSingle().NonLazy();
			Container.BindInterfacesTo<AddressableReferenceResolver>().AsSingle().NonLazy();

			// Scope
			Container.BindInterfacesAndSelfTo<GlobalScope>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<SlotScope>().AsSingle().NonLazy();
			Container.Bind<BaseScope>()
				.WithId("RootScope")
				.To<RootScope>().AsSingle().NonLazy();

			// Core Manager
			Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
		}
	}
}
