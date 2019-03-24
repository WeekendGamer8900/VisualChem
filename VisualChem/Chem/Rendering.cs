using System;
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
            public bool simpleMode = false;
            public bool randomStruct = false;
            public bool fixCamera = false;
            public float RefScale = 1f;
            public PointF Cursor;
            public Node Selected;
            public PointF origin = new PointF(0, 0);

            int PointToNum(float dx, float dy)
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
            PointF NumToPoint(int num)
            {
                if (num == 1)
                {
                    return new PointF(1, 0);
                }
                else if (num == 2)
                {
                    return new PointF(0, 1);
                }
                else if (num == 3)
                {
                    return new PointF(-1, 0);
                }
                else if (num == 4)
                {
                    return new PointF(0, -1);
                }
                else
                {
                    return new PointF(0, 0);
                }
            }

            Dictionary<Structure.Node,Node> visited = new Dictionary<Structure.Node,Node>();

            void DrawGraph(Structure.Molecule molecule, Bond lastBond, Structure.Bond lastInfo, Node baseNode, Structure.Node info)
            {
                List<Structure.Bond> connections = molecule.GetOther(info);
                if (connections.Count == 0) return;
                if (connections.Count > Structure.GetBondCount(info.Type))
                {
                    return;
                }

                List<int> availableDirs = new int[] { 1, 3, 4, 2 }.ToList();
                if (lastBond != null)
                {
                    availableDirs.Remove(PointToNum(lastBond.GetOther(baseNode).Location.X - baseNode.Location.X, lastBond.GetOther(baseNode).Location.Y - baseNode.Location.Y));
                    if (connections.Contains(lastInfo)) connections.Remove(lastInfo);
                }

                if (!randomStruct)
                {
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
                        Node tmpNd;
                        if (visited.Keys.Contains(bd.GetOther(info)))
                        {
                            tmpNd = visited[bd.GetOther(info)];
                        }
                        else
                        {
                            tmpNd = new Node(bd.GetOther(info).Type, new PointF(baseNode.Location.X + dir, baseNode.Location.Y));
                            Nodes.Add(tmpNd);
                        }
                        availableDirs.Remove(PointToNum(dir, 0));
                        dir *= -1;
                        Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                        Bonds.Add(tmpbd);
                        if (!visited.Keys.Contains(bd.GetOther(info)))
                        {
                            DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                            visited.Add(bd.GetOther(info), tmpNd);
                        }
                    }

                    List<Structure.Bond> verticals = connections.Where(p => p.Orientation == Structure.Orientation.Vertical).ToList();
                    int verticalCnt = (availableDirs.Contains(2) ? 1 : 0) + (availableDirs.Contains(4) ? 1 : 0);
                    if (verticals.Count > verticalCnt)
                    {
                        verticals = verticals.Take(verticalCnt).ToList();
                    }
                    connections = connections.Except(verticals).ToList();
                    dir = -1;
                    foreach (Structure.Bond bd in verticals)
                    {
                        if (!availableDirs.Contains(PointToNum(0, dir))) dir *= -1;
                        Node tmpNd;
                        if (visited.Keys.Contains(bd.GetOther(info)))
                        {
                            tmpNd = visited[bd.GetOther(info)];
                        }
                        else
                        {
                            tmpNd = new Node(bd.GetOther(info).Type, new PointF(baseNode.Location.X, baseNode.Location.Y + dir));
                            Nodes.Add(tmpNd);
                        }
                        availableDirs.Remove(PointToNum(0, dir));
                        dir *= -1;
                        Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                        Bonds.Add(tmpbd);
                        if (!visited.Keys.Contains(bd.GetOther(info)))
                        {
                            DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                            visited.Add(bd.GetOther(info), tmpNd);
                        }
                    }
                }
                else
                {
                    connections.Shuffle();
                }

                foreach (Structure.Bond bd in connections)
                {
                    PointF offset = NumToPoint(availableDirs[0]);
                    Node tmpNd;
                    if (visited.Keys.Contains(bd.GetOther(info)))
                    {
                        tmpNd = visited[bd.GetOther(info)];
                    }
                    else
                    {
                        tmpNd = new Node(bd.GetOther(info).Type, new PointF(baseNode.Location.X + offset.X, baseNode.Location.Y + offset.Y));
                        Nodes.Add(tmpNd);
                    }
                    Bond tmpbd = new Bond(baseNode, tmpNd, bd.Type);
                    Bonds.Add(tmpbd);
                    if (!visited.Keys.Contains(bd.GetOther(info)))
                    {
                        DrawGraph(molecule, tmpbd, bd, tmpNd, bd.GetOther(info));
                        visited.Add(bd.GetOther(info), tmpNd);
                    }
                    availableDirs.RemoveAt(0);
                }
            }

            public Graph Recenter()
            {
                float minX = float.PositiveInfinity, minY = float.PositiveInfinity, maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;
                foreach (Node n in Nodes)
                {
                    minX = Math.Min(minX, n.Location.X);
                    minY = Math.Min(minY, n.Location.Y);
                    maxX = Math.Max(maxX, n.Location.X);
                    maxY = Math.Max(maxY, n.Location.Y);
                }
                foreach (Node n in Nodes)
                {
                    n.Location = n.Location.Subtract(new PointF((maxX + minX) / 2, (maxY + minY) / 2));
                }
                return this;
            }

            public Bitmap GetImage(int width, int height, Font font, float offsetX, float offsetY, float scale)
            {
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                float minX = float.PositiveInfinity, minY = float.PositiveInfinity, maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;
                foreach (Node n in Nodes)
                {
                    minX = Math.Min(minX, n.Location.X);
                    minY = Math.Min(minY, n.Location.Y);
                    maxX = Math.Max(maxX, n.Location.X);
                    maxY = Math.Max(maxY, n.Location.Y);
                }
                if (!fixCamera)
                {
                    float s = scale;
                    scale = 1f;
                    origin = new PointF((minX + maxX) * 30 * scale / 2, (minY + maxY) * 30 * scale / 2);
                    scale = Math.Min(width / 2 / (maxX * 30 * scale - origin.X), height / 2 / (maxY * 30 * scale - origin.Y)) * 0.9f;
                    scale *= s;
                    RefScale = scale;
                    origin = new PointF((minX + maxX) * 30 * scale / 2, (minY + maxY) * 30 * scale / 2);
                }
                else
                {
                    scale = RefScale;
                }
                if (Math.Abs((minX + maxX) * 30 * scale / 2) > 10000f || Math.Abs((minY + maxY) * 30 * scale / 2) > 10000f)
                {
                    Recenter();
                    minX = float.PositiveInfinity;
                    minY = float.PositiveInfinity;
                    maxX = float.NegativeInfinity;
                    maxY = float.NegativeInfinity;
                    foreach (Node n in Nodes)
                    {
                        minX = Math.Min(minX, n.Location.X);
                        minY = Math.Min(minY, n.Location.Y);
                        maxX = Math.Max(maxX, n.Location.X);
                        maxY = Math.Max(maxY, n.Location.Y);
                    }
                    if (!fixCamera)
                    {
                        float s = scale;
                        scale = 1f;
                        origin = new PointF((minX + maxX) * 30 * scale / 2, (minY + maxY) * 30 * scale / 2);
                        scale = Math.Min(width / 2 / (maxX * 30 * scale - origin.X), height / 2 / (maxY * 30 * scale - origin.Y)) * 0.9f;
                        scale *= s;
                        RefScale = scale;
                        origin = new PointF((minX + maxX) * 30 * scale / 2, (minY + maxY) * 30 * scale / 2);
                    }
                }
                g.TranslateTransform(width / 2 + offsetX - origin.X, height / 2 + offsetY - origin.Y);
                bool found = false;
                foreach (Node n in Nodes)
                {
                    if (simpleMode && n.Type != Elements.Hydrogen || !simpleMode)
                    {
                        if (found || Selected != null && Selected.Locked)
                        {
                            g.DrawString(n.Type.ToDString(), font, Brushes.Black, n.Location.X * 30 * scale, n.Location.Y * 30 * scale, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                        }
                        else if (Cursor.Distance(n.Location.Scale(30f * scale).Add(new PointF(width / 2 + offsetX, height / 2 + offsetY)).Subtract(origin)) < 10f)
                        {
                            found = true;
                            Selected = n;
                        }
                    }
                    g.DrawString(n.Type.ToDString(), font, n == Selected ? Brushes.Silver : Brushes.Black, n.Location.X * 30 * scale, n.Location.Y * 30 * scale, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                }
                if ((Selected == null || !Selected.Locked) && !found) Selected = null;
                foreach (Bond b in Bonds)
                {
                    PointF Dir21 = b.Node2.Location.Subtract(b.Node1.Location).Normalize();
                    PointF gap = Dir21.Scale(10);
                    if (simpleMode && b.Node1.Type != Elements.Hydrogen && b.Node2.Type != Elements.Hydrogen || !simpleMode)
                    {
                        if (b.Type == BondType.Single || b.Type == BondType.Triple)
                        {
                            g.DrawLine(Pens.Black,
                                gap.X + b.Node1.Location.X * 30 * scale,
                                gap.Y + b.Node1.Location.Y * 30 * scale,
                                -gap.X + b.Node2.Location.X * 30 * scale,
                                -gap.Y + b.Node2.Location.Y * 30 * scale);
                        }
                        if (b.Type == BondType.Triple)
                        {
                            double angle = Math.Atan2(Dir21.Y, Dir21.X);
                            PointF lateral = new PointF((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)).Scale(4);
                            g.DrawLine(Pens.Black,
                                lateral.X + gap.X + b.Node1.Location.X * 30 * scale,
                                lateral.Y + gap.Y + b.Node1.Location.Y * 30 * scale,
                                lateral.X - gap.X + b.Node2.Location.X * 30 * scale,
                                lateral.Y - gap.Y + b.Node2.Location.Y * 30 * scale);
                            g.DrawLine(Pens.Black,
                                -lateral.X + gap.X + b.Node1.Location.X * 30 * scale,
                                -lateral.Y + gap.Y + b.Node1.Location.Y * 30 * scale,
                                -lateral.X - gap.X + b.Node2.Location.X * 30 * scale,
                                -lateral.Y - gap.Y + b.Node2.Location.Y * 30 * scale);
                        }
                        if (b.Type == BondType.Double)
                        {
                            double angle = Math.Atan2(Dir21.Y, Dir21.X);
                            PointF lateral = new PointF((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)).Scale(2);
                            g.DrawLine(Pens.Black,
                                lateral.X + gap.X + b.Node1.Location.X * 30 * scale,
                                lateral.Y + gap.Y + b.Node1.Location.Y * 30 * scale,
                                lateral.X - gap.X + b.Node2.Location.X * 30 * scale,
                                lateral.Y - gap.Y + b.Node2.Location.Y * 30 * scale);
                            g.DrawLine(Pens.Black,
                                -lateral.X + gap.X + b.Node1.Location.X * 30 * scale,
                                -lateral.Y + gap.Y + b.Node1.Location.Y * 30 * scale,
                                -lateral.X - gap.X + b.Node2.Location.X * 30 * scale,
                                -lateral.Y - gap.Y + b.Node2.Location.Y * 30 * scale);
                        }
                    }
                }
                for (int i = 0; i < 10; i++)
                    Tick();
                return bmp;
            }

            public float G = 0.1f;
            public float K = 0.1f;
            public float length = 2f;

            public Graph Shake()
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Nodes[i].Velocity = new PointF(Helper.Rnd(-10, 10), Helper.Rnd(-10, 10));
                }
                return this;
            }

            public Graph Tick()
            {
                //Resistance between nodes
                for (int i = 0; i < Nodes.Count; i++)
                {
                    for (int j = i + 1; j < Nodes.Count; j++)
                    {
                        Node a = Nodes[i];
                        Node b = Nodes[j];
                        float force = G / Math.Max(0.3f, a.Location.Distance(b.Location)).Sqr();
                        a.Velocity = a.Velocity.Add(a.Location.Subtract(b.Location).Normalize().Scale(force));
                        b.Velocity = b.Velocity.Add(b.Location.Subtract(a.Location).Normalize().Scale(force));
                    }
                }

                //Bonds
                for (int i = 0; i < Bonds.Count; i++)
                {
                    Node a = Bonds[i].Node1;
                    Node b = Bonds[i].Node2;
                    a.Velocity = a.Velocity.Add(b.Location.Subtract(a.Location).Normalize().Scale(K * (a.Location.Distance(b.Location) - length)));
                    b.Velocity = b.Velocity.Add(a.Location.Subtract(b.Location).Normalize().Scale(K * (a.Location.Distance(b.Location) - length)));
                }

                //Process velocity
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (!Nodes[i].Locked)
                        Nodes[i].Location = Nodes[i].Location.Add(Nodes[i].Velocity);
                    Nodes[i].Velocity = Nodes[i].Velocity.Scale(0.9f);
                }

                return this;
            }

            public Graph FromStructure(Structure.Molecule molecule)
            {
                Structure.Node pivot = molecule.Nodes.First();
                if (pivot == null)
                {
                    return this;
                }
                Node n = new Node(pivot.Type, new Point(0, 0));
                Nodes.Add(n);
                visited.Add(pivot, n);
                DrawGraph(molecule, null, null, n, pivot);
                return this;
            }

            public List<Bond> GetOther(Node thisNode)
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
            public PointF Location;
            public PointF Velocity = new PointF(0, 0);
            public Elements Type;
            public bool Locked = false;
            public Node(Elements t, PointF loc = default(PointF))
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
