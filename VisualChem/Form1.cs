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

namespace VisualChem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Chem.Molecule thisMol = new Chem.Molecule();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Chem.StringExpression l = thisMol.Lexer(txtName.Text);
                if (l.FunctionalGpTokens != null && l.ParentChainTokens != null && l.TailTokens != null)
                    txtName.Text = (l.FunctionalGpTokens.Count > 0 ? l.FunctionalGpTokens.Aggregate((a, b) => a + " " + b) : "") + " | " +
                        (l.ParentChainTokens.Count > 0 ? l.ParentChainTokens.Aggregate((a, b) => a + " " + b) : "") + " | " +
                        (l.TailTokens.Count > 0 ? l.TailTokens.Aggregate((a, b) => a + " " + b) : "");
                Chem.TokenizedExpression tl = thisMol.Tokenize(l);
                txtName.Text = "";
                tl.FunctionalGpTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
                txtName.Text += "| ";
                tl.ParentChainTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
                txtName.Text += "| ";
                tl.TailTokens.ForEach((t) => txtName.Text += t.Type.ToString() + " ");
            }
        }
    }
}
