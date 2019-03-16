﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VisualChem.Chem
{
    class Rendering
    {
        public class Graph
        {
            public List<Node> Nodes = new List<Node>();
            public List<Bond> Bonds = new List<Bond>();

            int PointToNum(int dx,int dy)
            {
                if (dx == 0)
                {
                    if (dy > 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (dx > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 3;
                    }
                }
            }
            Point NumToPoint(int num)
            {
                if (num == 1)
                {
                    return new Point(1, 0);
                }
                else if (num == 2)
                {
                    return new Point(0, 1);
                }
                else if (num == 3)
                {
                    return new Point(-1, 0);
                }
                else if (num == 4)
                {
                    return new Point(0, -1);
                }
                else
                {
                    return new Point(0, 0);
                }
            }

            void DrawGraph(Structure.Molecule molecule, Bond lastBond, Structure.Bond lastInfo, Node baseNode, Structure.Node info)
            {
                List<Structure.Bond> connections = molecule.GetOther(info);
                if (connections.Count == 0) return;
                if (connections.Count > Structure.GetBondCount(info.Type))
                {
                    return;
                }

                List<int> availableDirs = new int[] { 1, 2, 3, 4 }.ToList();
                if (lastBond != null)
                {
                    availableDirs.Remove(PointToNum(lastBond.GetOther(baseNode).Location.X - baseNode.Location.X, lastBond.GetOther(baseNode).Location.Y - baseNode.Location.Y));
                    if (connections.Contains(lastInfo)) connections.Remove(lastInfo);
                }

                List<Structure.Bond> horizontals = connections.Where(p => p.Orientation == Structure.Orientation.Horizontal).ToList();
                int horizontalCnt = (availableDirs.Contains(1) ? 1 : 0) + (availableDirs.Contains(3) ? 1 : 0);
                if (horizontals.Count > horizontalCnt)
                {
                    horizontals = horizontals.Take(horizontalCnt).ToList();
                }
                connections = connections.Except(horizontals).ToList();
                int dir = 1;
                foreach (Structure.Bond bd in horizontals)
                {
                    if (!availableDirs.Contains(PointToNum(dir, 0))) dir *= -1;
                    Node tmpNd = new Node(bd.GetOther(info).Type, new Point(baseNode.Location.X + dir, baseNode.Location.Y));
                    Nodes.Add(tmpNd);
                    availableDirs.Remove(PointToNum(dir, 0));
                    dir *= -1;
                    Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                    Bonds.Add(tmpbd);
                    DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                }

                List<Structure.Bond> verticals = connections.Where(p => p.Orientation == Structure.Orientation.Vertical).ToList();
                int verticalCnt = (availableDirs.Contains(2) ? 1 : 0) + (availableDirs.Contains(4) ? 1 : 0);
                if (verticals.Count > verticalCnt)
                {
                    verticals = verticals.Take(verticalCnt).ToList();
                }
                connections = connections.Except(verticals).ToList();
                dir = 1;
                foreach (Structure.Bond bd in verticals)
                {
                    if (!availableDirs.Contains(PointToNum(0, dir))) dir *= -1;
                    Node tmpNd = new Node(bd.GetOther(info).Type, new Point(baseNode.Location.X, baseNode.Location.Y + dir));
                    Nodes.Add(tmpNd);
                    availableDirs.Remove(PointToNum(0, dir));
                    dir *= -1;
                    Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                    Bonds.Add(tmpbd);
                    DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                }
                
                foreach (Structure.Bond bd in connections)
                {
                    Point offset = NumToPoint(availableDirs[0]);
                    Node tmpNd = new Node(bd.GetOther(info).Type, new Point(baseNode.Location.X + offset.X, baseNode.Location.Y + offset.Y));
                    Nodes.Add(tmpNd);
                    Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                    Bonds.Add(tmpbd);
                    DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                    availableDirs.RemoveAt(0);
                }
            }

            public Graph FromStructure(Structure.Molecule molecule)
            {
                Structure.Node pivot = molecule.Nodes.FirstOrDefault(i => i.Type == Elements.Carbon);
                if (pivot == null)
                {
                    return this;
                }
                Node n = new Node(pivot.Type, new Point(0, 0));
                Nodes.Add(n);
                DrawGraph(molecule, null, null, n, pivot);
                return this;
            }

            List<Bond> GetOther(Node thisNode)
            {
                List<Bond> ret = new List<Bond>();
                foreach (Bond b in Bonds)
                {
                    Node n = b.GetOther(thisNode);
                    if (n != null) ret.Add(b);
                }
                return ret;
            }
        }

        public class Node
        {
            public Point Location;
            public Elements Type;
            public Node(Elements t,Point loc = default(Point))
            {
                Type = t;
                Location = loc;
            }
        }

        public class Bond
        {
            public BondType Type;
            public Node Node1, Node2;

            public Bond(Node thisNode, Node thatNode, BondType t)
            {
                Node1 = thisNode;
                Node2 = thatNode;
                Type = t;
            }

            public Node GetOther(Node thisNode)
            {
                if (thisNode == Node1) return Node2;
                else if (thisNode == Node2) return Node1;
                else return null;
            }
        }
    }
}