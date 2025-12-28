namespace OOP_RPG
{
    partial class FrmBattleField
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtTraceLog = new TextBox();
            lblName = new Label();
            lstAction = new ListBox();
            lstTarget = new ListBox();
            btnPause = new Button();
            btnLogClear = new Button();
            btnQuit = new Button();
            Timer = new System.Windows.Forms.Timer(components);
            panelHero = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtTraceLog
            // 
            txtTraceLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            txtTraceLog.BackColor = SystemColors.ActiveCaptionText;
            txtTraceLog.Font = new Font("Yu Gothic UI", 12F);
            txtTraceLog.ForeColor = SystemColors.HighlightText;
            txtTraceLog.Location = new Point(0, 0);
            txtTraceLog.Multiline = true;
            txtTraceLog.Name = "txtTraceLog";
            txtTraceLog.ReadOnly = true;
            txtTraceLog.ScrollBars = ScrollBars.Vertical;
            txtTraceLog.Size = new Size(251, 797);
            txtTraceLog.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblName.AutoSize = true;
            lblName.BackColor = SystemColors.ActiveCaptionText;
            lblName.BorderStyle = BorderStyle.Fixed3D;
            lblName.Font = new Font("Yu Gothic UI", 16F);
            lblName.ForeColor = SystemColors.HighlightText;
            lblName.Location = new Point(265, 574);
            lblName.Name = "lblName";
            lblName.Size = new Size(96, 32);
            lblName.TabIndex = 1;
            lblName.Text = "lblName";
            // 
            // lstAction
            // 
            lstAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lstAction.BackColor = SystemColors.InactiveCaptionText;
            lstAction.Font = new Font("Yu Gothic UI", 14F);
            lstAction.ForeColor = SystemColors.HighlightText;
            lstAction.FormattingEnabled = true;
            lstAction.IntegralHeight = false;
            lstAction.Location = new Point(265, 618);
            lstAction.Name = "lstAction";
            lstAction.Size = new Size(270, 170);
            lstAction.TabIndex = 2;
            lstAction.Click += lstAction_SelectedIndexChanged;
            // 
            // lstTarget
            // 
            lstTarget.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lstTarget.BackColor = SystemColors.InactiveCaptionText;
            lstTarget.Font = new Font("Yu Gothic UI", 14F);
            lstTarget.ForeColor = SystemColors.HighlightText;
            lstTarget.FormattingEnabled = true;
            lstTarget.IntegralHeight = false;
            lstTarget.Location = new Point(554, 618);
            lstTarget.Margin = new Padding(0);
            lstTarget.Name = "lstTarget";
            lstTarget.Size = new Size(270, 170);
            lstTarget.TabIndex = 3;
            lstTarget.Click += lstTarget_SelectedIndexChanged;
            // 
            // btnPause
            // 
            btnPause.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnPause.Location = new Point(1490, 618);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(72, 23);
            btnPause.TabIndex = 4;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnLogClear
            // 
            btnLogClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLogClear.Location = new Point(1490, 692);
            btnLogClear.Name = "btnLogClear";
            btnLogClear.Size = new Size(72, 23);
            btnLogClear.TabIndex = 5;
            btnLogClear.Text = "ログクリア";
            btnLogClear.UseVisualStyleBackColor = true;
            btnLogClear.Click += btnLogClear_Click;
            // 
            // btnQuit
            // 
            btnQuit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnQuit.Location = new Point(1490, 765);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(72, 23);
            btnQuit.TabIndex = 6;
            btnQuit.Text = "終了";
            btnQuit.UseVisualStyleBackColor = true;
            btnQuit.Click += btnQuit_Click;
            // 
            // Timer
            // 
            Timer.Interval = 1000;
            Timer.Tick += Timing_Tick;
            // 
            // panelHero
            // 
            panelHero.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panelHero.BackColor = SystemColors.ActiveCaptionText;
            panelHero.BorderStyle = BorderStyle.Fixed3D;
            panelHero.Font = new Font("Yu Gothic UI", 16F);
            panelHero.ForeColor = SystemColors.HighlightText;
            panelHero.Location = new Point(845, 618);
            panelHero.Name = "panelHero";
            panelHero.Size = new Size(650, 170);
            panelHero.TabIndex = 13;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1297, 411);
            flowLayoutPanel1.TabIndex = 14;
            flowLayoutPanel1.WrapContents = false;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Location = new Point(265, 129);
            panel1.Name = "panel1";
            panel1.Size = new Size(1297, 411);
            panel1.TabIndex = 15;
            // 
            // FrmBattleField
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.pipo_battlebg002b;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1574, 797);
            Controls.Add(panel1);
            Controls.Add(panelHero);
            Controls.Add(btnQuit);
            Controls.Add(btnLogClear);
            Controls.Add(btnPause);
            Controls.Add(lstTarget);
            Controls.Add(lstAction);
            Controls.Add(lblName);
            Controls.Add(txtTraceLog);
            Name = "FrmBattleField";
            Text = "BattleField";
            TopMost = true;
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTraceLog;
        private Label lblName;
        private ListBox lstAction;
        private ListBox lstTarget;
        private Button btnPause;
        private Button btnLogClear;
        private Button btnQuit;
        private System.Windows.Forms.Timer Timer;
        private Panel panelHero;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
    }
}
