using System.Collections.Generic;

namespace SaveSystem.Runtime {
	public interface IScope : ISaver {
		public Dictionary<string, ISaver> Savers { get; }
		public object WeakSnapshot { get; }

		public void RegisterSaver(ISaver saver);
		public void UnregisterSaver(ISaver saver);
		public void UnregisterAllSaver();
	}

	public interface IScope<T> : ISaver<T>, IScope {
		public T Snapshot { get; set; }
		object IScope.WeakSnapshot => Snapshot;
	}
}
