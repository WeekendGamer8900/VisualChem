namespace VisualChem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imgOut = new System.Windows.Forms.PictureBox();
            this.txtName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgOut)).BeginInit();
            this.SuspendLayout();
            // 
            // imgOut
            // 
            this.imgOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgOut.BackColor = System.Drawing.Color.White;
            this.imgOut.Location = new System.Drawing.Point(0, 0);
            this.imgOut.Name = "imgOut";
            this.imgOut.Size = new System.Drawing.Size(800, 389);
            this.imgOut.TabIndex = 0;
            this.imgOut.TabStop = false;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtName.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(0, 392);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(800, 23);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 415);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.imgOut);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "VisualChem";
            ((System.ComponentModel.ISupportInitialize)(this.imgOut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgOut;
        private System.Windows.Forms.TextBox txtName;
    }
}

