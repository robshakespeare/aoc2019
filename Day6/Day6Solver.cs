using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day6
{
    public class Day6Solver : SolverReadAllLines
    {
        public override long? SolvePart1(string[] mapLines)
        {
            // What is the total number of direct and indirect orbits?
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
                var parent = GetOrAddNode(pair.parentId, nodes);
                var child = GetOrAddNode(pair.childId, nodes);

                parent.AddChild(child);
            }

            return nodes;
        }

        private static HierarchicalNode GetOrAddNode(string nodeId, IDictionary<string, HierarchicalNode> nodes)
        {
            if (!nodes.TryGetValue(nodeId, out var node))
            {
                nodes.Add(nodeId, node = new HierarchicalNode(false, nodeId));
            }
            return node;
        }

        public override long? SolvePart2(string[] mapLines)
        {
            // What is the minimum number of orbital transfers required to move from the object YOU are orbiting to the object SAN is orbiting?
            // (Between the objects they are orbiting - not between YOU and SAN.)

            var nodeDictionary = BuildHierarchy(mapLines);

            var me = nodeDictionary["YOU"];
            var santa = nodeDictionary["SAN"];

            var closestIntersection = me.Parents
                .Intersect(santa.Parents, new HierarchicalNodeEqualityComparer())
                .OrderByDescending(x => x.Depth)
                .First();

            // Note the -1 because its between the objects they are orbiting, not themselves
            var meToIntersection = me.Depth - 1 - closestIntersection.Depth;
            var santaToIntersection = santa.Depth - 1 - closestIntersection.Depth;

            return meToIntersection + santaToIntersection;
        }
    }
}
