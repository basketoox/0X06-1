using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Toolkit
{
    public class TreeNode<T>
    {
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            Value = value;
        }

        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        public TreeNode<T> Parent { get; private set; }

        public T Value { get; private set; }

        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public void AddChild(TreeNode<T> node)
        {
            node.Parent = this;
            _children.Add(node);
        }

        public void RemoveChild(TreeNode<T> node)
        {
            _children.Remove(node);
            node.Parent = null;
        }

        public void Traverse(Action<TreeNode<T>> action)
        {
            action(this);
            foreach (var child in _children)
            {
                child.Traverse(action);
            }
        }

        public IEnumerable<T> Flatten()
        {
            return new[] {Value}.Union(_children.SelectMany(x => x.Flatten()));
        }
    }
}