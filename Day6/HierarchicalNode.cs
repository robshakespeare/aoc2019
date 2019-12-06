using System;
using System.Collections.Generic;

namespace Day6
{
    public class HierarchicalNode
    {
        private HierarchicalNode? parentNode;
        private readonly IList<HierarchicalNode> childNodes = new List<HierarchicalNode>();

        public HierarchicalNode(bool isRootNode, string id)
        {
            IsRootNode = isRootNode;
            Id = id;
        }

        public HierarchicalNode? ParentNode
        {
            get => parentNode;
            set
            {
                if (parentNode != null)
                {
                    throw new InvalidOperationException(
                        $"Unable to set nodes parent, it already has a parent. {new {ThisNodeId = Id, ParentNodeId = parentNode.Id}}");
                }

                parentNode = value;
            }
        }

        public string Id { get; }

        public bool IsRootNode { get; }

        public IEnumerable<HierarchicalNode> ChildNodes => childNodes;

        public int Depth => ParentNode?.Depth + 1 ?? 0;

        public void AddChild(HierarchicalNode child)
        {
            child.ParentNode = this;
            childNodes.Add(child);
        }

        public IEnumerable<HierarchicalNode> Parents
        {
            get
            {
                var self = this;
                HierarchicalNode? parent;

                while ((parent = self.ParentNode) != null)
                {
                    yield return parent;
                    self = parent;
                }
            }
        }
    }
}
