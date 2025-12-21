using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
	
namespace TourBooker.Logic
{
	public class ConcurrentLinkedList<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		private readonly LinkedList<T> _list = new LinkedList<T>();
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public sealed class Node
		{
			internal LinkedListNode<T> Inner { get; }
			internal Node(LinkedListNode<T> inner) { Inner = inner; }
			public T Value => Inner.Value;
		}

		public int Count
		{
			get
			{
				_lock.EnterReadLock();
				try { return _list.Count; }
				finally { _lock.ExitReadLock(); }
			}
		}

		public Node GetNthNode(int index)
		{
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
			_lock.EnterReadLock();
			try
			{
				var node = _list.First;
				for (int i = 0; node != null && i < index; i++)
					node = node.Next;
				return node == null ? null : new Node(node);
			}
			finally { _lock.ExitReadLock(); }
		}

		public void AddLast(T value)
		{
			T[] snapshot;
			int newCount;
			_lock.EnterWriteLock();
			try
			{
				_list.AddLast(value);
				newCount = _list.Count;
				snapshot = _list.ToArray();
			}
			finally { _lock.ExitWriteLock(); }

			// raise notifications outside lock
			OnPropertyChanged(nameof(Count));
			OnCollectionReset(snapshot);
		}

		public Node AddBefore(Node node, T value)
		{
			if (node == null) throw new ArgumentNullException(nameof(node));
			LinkedListNode<T> newNode;
			T[] snapshot;
			int newCount;
			_lock.EnterWriteLock();
			try
			{
				if (node.Inner.List != _list) throw new InvalidOperationException("Node does not belong to this list.");
				newNode = _list.AddBefore(node.Inner, value);
				newCount = _list.Count;
				snapshot = _list.ToArray();
			}
			finally { _lock.ExitWriteLock(); }

			OnPropertyChanged(nameof(Count));
			OnCollectionReset(snapshot);
			return new Node(newNode);
		}

		public void Remove(Node node)
		{
			if (node == null) return;
			bool removed = false;
			T[] snapshot;
			int newCount;
			_lock.EnterWriteLock();
			try
			{
				if (node.Inner.List == _list)
				{
					_list.Remove(node.Inner);
					removed = true;
				}
				newCount = _list.Count;
				snapshot = _list.ToArray();
			}
			finally { _lock.ExitWriteLock(); }

			if (removed)
			{
				OnPropertyChanged(nameof(Count));
				OnCollectionReset(snapshot);
			}
		}

		public T[] ToArray()
		{
			_lock.EnterReadLock();
			try { return _list.ToArray(); }
			finally { _lock.ExitReadLock(); }
		}

		public void Clear()
		{
			_lock.EnterWriteLock();
			try { _list.Clear(); }
			finally { _lock.ExitWriteLock(); }

			OnPropertyChanged(nameof(Count));
			OnCollectionReset(new T[0]);
		}

		public IEnumerator<T> GetEnumerator()
		{
			// iterate over a snapshot to avoid holding locks during UI enumeration
			var snapshot = ToArray();
			foreach (var v in snapshot) yield return v;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		// Helpers to raise events
		private void OnCollectionReset(T[] snapshot)
		{
			// Use Reset so WPF will rebuild the view. Passing null is allowed for Reset.
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}