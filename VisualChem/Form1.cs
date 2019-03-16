using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualChem.Chem;

namespace VisualChem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Structure.Molecule thisMol = new Structure.Molecule();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                thisMol = new Structure.Molecule();
                string tmp = txtName.Text;
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
                thisMol.FromName(tmp);
                Rendering.Graph graph = new Rendering.Graph();
                graph.FromStructure(thisMol);
                Bitmap bmp = new Bitmap(imgOut.Width, imgOut.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.TranslateTransform(imgOut.Width / 2, imgOut.Height / 2);
                foreach (Rendering.Node n in graph.Nodes)
                {
                    g.DrawString(n.Type.ToDString(), Font, Brushes.Black, n.Location.X * 30, n.Location.Y * 30, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                }
                foreach (Rendering.Bond b in graph.Bonds)
                {
                    g.DrawLine(Pens.Black,
                        Math.Sign(b.Node2.Location.X - b.Node1.Location.X) * 7 + b.Node1.Location.X * 30,
                        Math.Sign(b.Node2.Location.Y - b.Node1.Location.Y) * 7 + b.Node1.Location.Y * 30,
                        Math.Sign(b.Node1.Location.X - b.Node2.Location.X) * 7 + b.Node2.Location.X * 30,
                        Math.Sign(b.Node1.Location.Y - b.Node2.Location.Y) * 7 + b.Node2.Location.Y * 30);
                }
                imgOut.Image = bmp;
                imgOut.Refresh();
                Console.WriteLine("");
            }
        }
    }
}
