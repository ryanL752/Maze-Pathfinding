using UnityEngine;
using System.Collections.Generic;

public class Dijkstra
{
    private List<Node> CalculatePath(Node node)
    {
        List<Node> list = new();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    public List<Node> FindPath(Node start, Node goal)
    {

        NodePriorityQueue openList = new NodePriorityQueue();
        openList.Enqueue(start);
        start.costSoFar = 0.0f;

        HashSet<Node> closedList = new();
        Node node = null;

        while (openList.Length != 0) {
            node = openList.Dequeue();

            if (node.position == goal.position) {
                return CalculatePath(node);
            }

            var neighbours = GridManager.instance.GetNeighbours(node);

            foreach (Node neighbourNode in neighbours) {
                if (!closedList.Contains(neighbourNode))  {
                    float totalCost = node.costSoFar + GridManager.instance.StepCost;

                    if (totalCost < neighbourNode.costSoFar) {
                        neighbourNode.costSoFar = totalCost;
                        neighbourNode.parent = node;
                    }

                    if (!closedList.Contains(neighbourNode)) {
                        openList.Enqueue(neighbourNode);
                    }
                }
            }

            closedList.Add(node);
        }

        if (node.position != goal.position) {
            Debug.LogError("Goal Not Found");
            return null;
        }

        return CalculatePath(node);
    }
}