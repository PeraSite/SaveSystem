using Cysharp.Threading.Tasks;

namespace SaveSystem.Runtime {
	public interface ISceneTransition {
		public UniTask StartTransition();
		public UniTask EndTransition();
	}
}
