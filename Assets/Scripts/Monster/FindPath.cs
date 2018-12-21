using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    public class FindPath
    {
        public static List<Node> FindingPath(Node startNode, Node endNode)
        {
            Room rm = Map.Instance.Pos2Room(endNode.WorldPos);
			if (rm == null)	return null;
            if (!rm.nodeDictionary[endNode.WorldPos].CanWalk) return null ;
			if (startNode.WorldPos == endNode.WorldPos)	return null;
            // Node startNode = grid.GetFromPosition(startPos);
            //  Node endNode = grid.GetFromPosition(endPos);
            List<Node> openSet = new List<Node>();
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost
                        || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    return GeneratePath(startNode, endNode);
                }
                foreach (var node in GetNeibourhood(currentNode))
                {
                    if (!node.CanWalk || closeSet.Contains(node)) continue;
                    int newCost = currentNode.GCost + GetDistanceNodes(currentNode, node);
                    if (newCost < node.GCost || !openSet.Contains(node))
                    {
                        node.GCost = newCost;
                        node.HCost = GetDistanceNodes(node, endNode);
                        node.Parent = currentNode;
                        if (!openSet.Contains(node))
                        {
                            openSet.Add(node);
                        }
                    }
                }

            }
            return null;
        }

        private static List<Node> GeneratePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.Parent;
            }
            path.Reverse();
            return path;
        }

        static int GetDistanceNodes(Node a, Node b)
        {
            int countX = Mathf.Abs(a.GridX - b.GridX);
            int countY = Mathf.Abs(a.GridY - b.GridY);
            if (countX > countY)
            {
                return 14 * countY + 10 * (countX - countY);
            }
            else
            {
                return 14 * countX + 10 * (countY - countX);
            }
        }

        public static List<Node> GetNeibourhood(Node node)
        {
            List<Node> neibourhood = new List<Node>();
            for (int i = -1; i <= 1;i++)
            {
                for (int j = -1; j <= 1;j++)
                {
                    if (i==0 && j==0)
                    {
                        continue;
                    }
                    int tempX = (int)node.WorldPos.x + i;
                    int tempY = (int)node.WorldPos.y + j;
                    Room rm = Map.Instance.Pos2Room(node.WorldPos);
                    if (tempX > rm.x_Point-rm.x_H_Length && tempX < rm.x_Point+rm.x_H_Length 
                        && tempY < rm.y_Point +rm.y_H_Length && tempY> rm.y_Point-rm.y_H_Length) 
                    {
                        neibourhood.Add(rm.nodeDictionary[new Vector2(tempX,tempY)]);
                    }
                }
            }
            return neibourhood;
        }
    }

    public class Node
    {
        public bool CanWalk;
        public Vector2 WorldPos;
        public int GridX, GridY;

        public int GCost;
        public int HCost;
        public int FCost
        {
            get { return GCost + HCost; }
        }

        public Node Parent;

        public Node(bool canwalk, Vector2 position, int x, int y)
        {
            CanWalk = canwalk;
            WorldPos = position;
            GridX = x;
            GridY = y;
        }
    }
}
