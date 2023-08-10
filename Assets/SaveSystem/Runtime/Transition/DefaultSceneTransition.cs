using Cysharp.Threading.Tasks;

namespace SaveSystem.Runtime {
	public class DefaultSceneTransition : ISceneTransition {
		public UniTask StartTransition() {
			return UniTask.CompletedTask;
		}

		public UniTask EndTransition() {
			return UniTask.CompletedTask;
		}
	}
}
