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
            imgOut.MouseWheel += ImgOut_MouseWheel;
        }

        private void ImgOut_MouseWheel(object sender, MouseEventArgs e)
        {
            scale *= (float)Math.Pow(1.0001, e.Delta);
        }

        Structure.Molecule thisMol = new Structure.Molecule();
        Rendering.Graph graph = new Rendering.Graph();
        float scale = 1f;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                try
                {
                    string tmp = txtName.Text.ToLower();
                    thisMol = new Structure.Molecule();
                    graph = new Rendering.Graph();
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
        }

        private void imgOut_MouseMove(object sender, MouseEventArgs e)
        {
            imgOut.Focus();
        }

        private void txtName_MouseMove(object sender, MouseEventArgs e)
        {
            txtName.Focus();
        }
    }
}
