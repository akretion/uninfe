namespace NFe.UI.Formularios
{
    partial class userConfiguracao_resptecnico
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCnpj = new MetroFramework.Controls.MetroLabel();
            this.txtCnpj = new MetroFramework.Controls.MetroTextBox();
            this.txtContato = new MetroFramework.Controls.MetroTextBox();
            this.lblContato = new MetroFramework.Controls.MetroLabel();
            this.txtEmail = new MetroFramework.Controls.MetroTextBox();
            this.lblEmail = new MetroFramework.Controls.MetroLabel();
            this.txtTelefone = new MetroFramework.Controls.MetroTextBox();
            this.lblTelefone = new MetroFramework.Controls.MetroLabel();
            this.txtIdCSRT = new MetroFramework.Controls.MetroTextBox();
            this.lblIdCSRT = new MetroFramework.Controls.MetroLabel();
            this.txtCSRT = new MetroFramework.Controls.MetroTextBox();
            this.lblCSRT = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // lblCnpj
            // 
            this.lblCnpj.AutoSize = true;
            this.lblCnpj.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCnpj.Location = new System.Drawing.Point(11, 17);
            this.lblCnpj.Name = "lblCnpj";
            this.lblCnpj.Size = new System.Drawing.Size(33, 15);
            this.lblCnpj.TabIndex = 0;
            this.lblCnpj.Text = "CNPJ";
            // 
            // txtCnpj
            // 
            this.txtCnpj.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCnpj.Lines = new string[0];
            this.txtCnpj.Location = new System.Drawing.Point(13, 33);
            this.txtCnpj.MaxLength = 18;
            this.txtCnpj.Name = "txtCnpj";
            this.txtCnpj.PasswordChar = '\0';
            this.txtCnpj.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCnpj.SelectedText = "";
            this.txtCnpj.Size = new System.Drawing.Size(122, 23);
            this.txtCnpj.TabIndex = 1;
            this.txtCnpj.UseSelectable = true;
            this.txtCnpj.TextChanged += new System.EventHandler(this.txtCnpj_TextChanged);
            this.txtCnpj.Leave += new System.EventHandler(this.txtCnpj_Leave);
            // 
            // txtContato
            // 
            this.txtContato.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContato.Lines = new string[0];
            this.txtContato.Location = new System.Drawing.Point(151, 33);
            this.txtContato.MaxLength = 60;
            this.txtContato.Name = "txtContato";
            this.txtContato.PasswordChar = '\0';
            this.txtContato.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtContato.SelectedText = "";
            this.txtContato.Size = new System.Drawing.Size(469, 23);
            this.txtContato.TabIndex = 3;
            this.txtContato.UseSelectable = true;
            this.txtContato.TextChanged += new System.EventHandler(this.txtContato_TextChanged);
            // 
            // lblContato
            // 
            this.lblContato.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContato.AutoSize = true;
            this.lblContato.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblContato.Location = new System.Drawing.Point(149, 16);
            this.lblContato.Name = "lblContato";
            this.lblContato.Size = new System.Drawing.Size(48, 15);
            this.lblContato.TabIndex = 2;
            this.lblContato.Text = "Contato";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtEmail.Lines = new string[0];
            this.txtEmail.Location = new System.Drawing.Point(13, 85);
            this.txtEmail.MaxLength = 60;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PasswordChar = '\0';
            this.txtEmail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(468, 23);
            this.txtEmail.TabIndex = 5;
            this.txtEmail.UseSelectable = true;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmail.AutoSize = true;
            this.lblEmail.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblEmail.Location = new System.Drawing.Point(11, 67);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(38, 15);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "E-mail";
            // 
            // txtTelefone
            // 
            this.txtTelefone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTelefone.Lines = new string[0];
            this.txtTelefone.Location = new System.Drawing.Point(500, 85);
            this.txtTelefone.MaxLength = 14;
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.PasswordChar = '\0';
            this.txtTelefone.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtTelefone.SelectedText = "";
            this.txtTelefone.Size = new System.Drawing.Size(52, 23);
            this.txtTelefone.TabIndex = 7;
            this.txtTelefone.UseSelectable = true;
            this.txtTelefone.TextChanged += new System.EventHandler(this.txtTelefone_TextChanged);
            // 
            // lblTelefone
            // 
            this.lblTelefone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblTelefone.Location = new System.Drawing.Point(495, 67);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(47, 15);
            this.lblTelefone.TabIndex = 6;
            this.lblTelefone.Text = "Telefone";
            // 
            // txtIdCSRT
            // 
            this.txtIdCSRT.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtIdCSRT.Lines = new string[0];
            this.txtIdCSRT.Location = new System.Drawing.Point(567, 85);
            this.txtIdCSRT.MaxLength = 2;
            this.txtIdCSRT.Name = "txtIdCSRT";
            this.txtIdCSRT.PasswordChar = '\0';
            this.txtIdCSRT.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtIdCSRT.SelectedText = "";
            this.txtIdCSRT.Size = new System.Drawing.Size(53, 23);
            this.txtIdCSRT.TabIndex = 9;
            this.txtIdCSRT.UseSelectable = true;
            this.txtIdCSRT.TextChanged += new System.EventHandler(this.txtIdCSRT_TextChanged);
            this.txtIdCSRT.Leave += new System.EventHandler(this.txtIdCSRT_Leave);
            // 
            // lblIdCSRT
            // 
            this.lblIdCSRT.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIdCSRT.AutoSize = true;
            this.lblIdCSRT.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblIdCSRT.Location = new System.Drawing.Point(565, 68);
            this.lblIdCSRT.Name = "lblIdCSRT";
            this.lblIdCSRT.Size = new System.Drawing.Size(47, 15);
            this.lblIdCSRT.TabIndex = 8;
            this.lblIdCSRT.Text = "ID CSRT";
            // 
            // txtCSRT
            // 
            this.txtCSRT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCSRT.Lines = new string[0];
            this.txtCSRT.Location = new System.Drawing.Point(13, 137);
            this.txtCSRT.MaxLength = 36;
            this.txtCSRT.Name = "txtCSRT";
            this.txtCSRT.PasswordChar = '\0';
            this.txtCSRT.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCSRT.SelectedText = "";
            this.txtCSRT.Size = new System.Drawing.Size(468, 23);
            this.txtCSRT.TabIndex = 11;
            this.txtCSRT.UseSelectable = true;
            this.txtCSRT.TextChanged += new System.EventHandler(this.txtCSRT_TextChanged);
            // 
            // lblCSRT
            // 
            this.lblCSRT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCSRT.AutoSize = true;
            this.lblCSRT.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblCSRT.Location = new System.Drawing.Point(11, 119);
            this.lblCSRT.Name = "lblCSRT";
            this.lblCSRT.Size = new System.Drawing.Size(33, 15);
            this.lblCSRT.TabIndex = 10;
            this.lblCSRT.Text = "CSRT";
            // 
            // userConfiguracao_resptecnico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCSRT);
            this.Controls.Add(this.lblCSRT);
            this.Controls.Add(this.txtIdCSRT);
            this.Controls.Add(this.lblIdCSRT);
            this.Controls.Add(this.txtTelefone);
            this.Controls.Add(this.lblTelefone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtContato);
            this.Controls.Add(this.lblContato);
            this.Controls.Add(this.txtCnpj);
            this.Controls.Add(this.lblCnpj);
            this.Name = "userConfiguracao_resptecnico";
            this.Size = new System.Drawing.Size(640, 374);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel lblCnpj;
        private MetroFramework.Controls.MetroTextBox txtCnpj;
        private MetroFramework.Controls.MetroTextBox txtContato;
        private MetroFramework.Controls.MetroLabel lblContato;
        private MetroFramework.Controls.MetroTextBox txtEmail;
        private MetroFramework.Controls.MetroLabel lblEmail;
        private MetroFramework.Controls.MetroTextBox txtTelefone;
        private MetroFramework.Controls.MetroLabel lblTelefone;
        private MetroFramework.Controls.MetroTextBox txtIdCSRT;
        private MetroFramework.Controls.MetroLabel lblIdCSRT;
        private MetroFramework.Controls.MetroTextBox txtCSRT;
        private MetroFramework.Controls.MetroLabel lblCSRT;
    }
}
