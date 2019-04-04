using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualChem;

namespace VisualChemApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            imgOut.MouseWheel += ImgOut_MouseWheel;
        }

        private void ImgOut_MouseWheel(object sender, MouseEventArgs e)
        {
            scale *= (float)Math.Pow(1.0001, e.Delta);
        }

        Structure.Molecule thisMol = new Structure.Molecule();
        Rendering.Graph graph = new Rendering.Graph();
        float scale = 1f;

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                try
                {
                    string tmp = txtName.Text.ToLower();
                    thisMol = new Structure.Molecule();
                    graph = new Rendering.Graph();
                    graph.scaleFont = true;
                    /*
                    Structure.StringExpression l = thisMol.Lexer(tmp);
                    if (l.FunctionalGpTokens != null && l.ParentChainTokens != null && l.TailTokens != null)
                        txtName.Text = (l.FunctionalGpTokens.Count > 0 ? l.FunctionalGpTokens.Aggregate((a, b) => a + " " + b) : "") + " | " +
                            (l.ParentChainTokens.Count > 0 ? l.ParentChainTokens.Aggregate((a, b) => a + " " + b) : "") + " | " +
                            (l.TailTokens.Count > 0 ? l.TailTokens.Aggregate((a, b) => a + " " + b) : "");
                    Structure.TokenizedExpression tl = thisMol.Tokenize(l);
                    txtName.Text = "";
                    tl.FunctionalGpTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
                    txtName.Text += "| ";
                    tl.ParentChainTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
                    txtName.Text += "| ";
                    tl.TailTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
                    */
                    thisMol.FromName(tmp);
                    graph.FromStructure(thisMol);
                    imgOut.Image = graph.GetImage(imgOut.Width, imgOut.Height, Font, 0, 0, scale);
                    imgOut.Refresh();
                    timerAnimation.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            imgOut.Image = graph.GetImage(imgOut.Width, imgOut.Height, Font, 0, 0, scale);
            imgOut.Refresh();
            Text = "VisualChem - " + "(" + graph.origin.X.ToString("F2") + ", " + graph.origin.Y.ToString("F2") + ") " + graph.TimeElapsed.ToString() + " - " + (1f / graph.RefScale).ToString("F2");
        }

        private void txtName_MouseMove(object sender, MouseEventArgs e)
        {
            txtName.Focus();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && !txtName.Focused)
            {
                graph.Shake();
            }
        }

        PointF lastPt;
        Rendering.Node S1;

        private void imgOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (graph.Selected != null)
                {
                    graph.Selected.Locked = true;
                    //graph.fixCamera = true;
                    lastPt = e.Location.F();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (graph.Selected != null)
                {
                    S1 = graph.Selected;
                }
            }
        }

        private void imgOut_MouseMove(object sender, MouseEventArgs e)
        {
            if (graph.Nodes.Count > 0)
                imgOut.Focus();
            graph.Cursor = e.Location.F();
            if (e.Button == MouseButtons.Left)
                if (graph.Selected != null)
                {
                    graph.Selected.Location = graph.Selected.Location.Add(e.Location.F().Subtract(lastPt).Scale(1f / 30f / scale));
                    lastPt = e.Location.F();
                }
        }

        private void imgOut_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (graph.Selected != null)
                {
                    graph.Selected.Location = graph.Selected.Location.Add(e.Location.F().Subtract(lastPt).Scale(1f / 30f / scale));
                    graph.Selected.Locked = false;
                    //graph.fixCamera = false;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (graph.Selected != null && S1 != null)
                {
                    if (graph.Selected == S1) return;
                    if (graph.GetOther(S1).Select(b => b.GetOther(S1)).Contains(graph.Selected))
                    {
                        Rendering.Bond bd = graph.Bonds.Where(b => b.GetOther(S1) == graph.Selected).First();
                        if (bd.Type == BondType.Triple) graph.Bonds.Remove(bd);
                        else bd.Type = (BondType)((int)bd.Type + 1);
                    }
                    else
                    {
                        graph.Bonds.Add(new Rendering.Bond(S1, graph.Selected, BondType.Single));
                        S1 = null;
                    }
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                if (graph.Selected != null)
                {
                    graph.Bonds.RemoveAll(b => b.GetOther(graph.Selected) != null);
                    graph.Nodes.Remove(graph.Selected);
                    graph.Selected = null;
                }
            }
        }
    }
}
