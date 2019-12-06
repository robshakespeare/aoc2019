using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Streams;

namespace Day6
{
    public class Solver
    {
        public int WhatIsTheTotalNumberOfDirectAndIndirectOrbits() =>
            WhatIsTheTotalNumberOfDirectAndIndirectOrbits(StreamUtils.FileToEnumerableOfString("input.txt"));

        public int WhatIsTheTotalNumberOfDirectAndIndirectOrbits(Stream stream) =>
            WhatIsTheTotalNumberOfDirectAndIndirectOrbits(StreamUtils.StreamToEnumerableOfString(stream));

        public int WhatIsTheTotalNumberOfDirectAndIndirectOrbits(IEnumerable<string> mapLines)
        {
            var nodeDictionary = BuildHierarchy(mapLines);

            var allNodes = nodeDictionary.Values;

            return allNodes.Sum(node => node.Depth);
        }

        private static Dictionary<string, HierarchicalNode> BuildHierarchy(IEnumerable<string> mapLines)
        {
            var rootNode = new HierarchicalNode(true, "COM");
            var nodes = new Dictionary<string, HierarchicalNode> {{rootNode.Id, rootNode}};

            foreach (var pair in mapLines
                .Select(line => line.Split(')'))
                .Select(pair => new {parentId = pair[0], childId = pair[1]}))
            {
                var parent = GetOrAddNodes(nodes, pair.parentId);
                var child = GetOrAddNodes(nodes, pair.childId);

                parent.AddChild(child);
            }

            return nodes;
        }

        private static HierarchicalNode GetOrAddNodes(IDictionary<string, HierarchicalNode> nodes, string id)
        {
            if (!nodes.TryGetValue(id, out var node))
            {
                nodes.Add(id, node = new HierarchicalNode(false, id));
            }
            return node;
        }

        public int SolvePart2(IEnumerable<string> mapLines)
        {
            // What is the minimum number of orbital transfers required to move from the object YOU are orbiting to the object SAN is orbiting?
            // (Between the objects they are orbiting - not between YOU and SAN.)

            var nodeDictionary = BuildHierarchy(mapLines);

            var me = nodeDictionary["YOU"];
            var santa = nodeDictionary["SAN"];

            var closestIntersection = me.Parents.Intersect(santa.Parents, new HierarchicalNodeEqualityComparer()).OrderByDescending(x => x.Depth).First();

            var meToIntersection = me.Depth - 1 - closestIntersection.Depth;

            var santaToIntersection = santa.Depth - 1 - closestIntersection.Depth;

            return meToIntersection + santaToIntersection;
        }
    }
}