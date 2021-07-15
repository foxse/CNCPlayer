namespace SceneSample
{
    partial class FormSceneSample
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSceneSample));
            this.sceneControl = new SharpGL.SceneControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cutCheckBox = new System.Windows.Forms.CheckBox();
            this.cutInCheckBox = new System.Windows.Forms.CheckBox();
            this.freeCheckBox = new System.Windows.Forms.CheckBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sceneControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sceneControl
            // 
            this.sceneControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneControl.DrawFPS = false;
            this.sceneControl.FrameRate = 30;
            this.sceneControl.Location = new System.Drawing.Point(0, 0);
            this.sceneControl.Name = "sceneControl";
            this.sceneControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.sceneControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.sceneControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.sceneControl.Size = new System.Drawing.Size(568, 402);
            this.sceneControl.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 59);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sceneControl);
            this.splitContainer1.Size = new System.Drawing.Size(708, 402);
            this.splitContainer1.SplitterDistance = 136;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(136, 402);
            this.splitContainer2.SplitterDistance = 263;
            this.splitContainer2.TabIndex = 7;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(136, 263);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(136, 135);
            this.propertyGrid1.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Interval = 20;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(708, 24);
            this.menuStrip.TabIndex = 7;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cutCheckBox);
            this.panel1.Controls.Add(this.cutInCheckBox);
            this.panel1.Controls.Add(this.freeCheckBox);
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.playButton);
            this.panel1.Controls.Add(this.openButton);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 29);
            this.panel1.TabIndex = 8;
            // 
            // cutCheckBox
            // 
            this.cutCheckBox.AutoSize = true;
            this.cutCheckBox.Checked = true;
            this.cutCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cutCheckBox.Location = new System.Drawing.Point(256, 7);
            this.cutCheckBox.Name = "cutCheckBox";
            this.cutCheckBox.Size = new System.Drawing.Size(42, 17);
            this.cutCheckBox.TabIndex = 6;
            this.cutCheckBox.Text = "Cut";
            this.cutCheckBox.UseVisualStyleBackColor = true;
            this.cutCheckBox.CheckedChanged += new System.EventHandler(this.cutCheckBox_CheckedChanged);
            // 
            // cutInCheckBox
            // 
            this.cutInCheckBox.AutoSize = true;
            this.cutInCheckBox.Checked = true;
            this.cutInCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cutInCheckBox.Location = new System.Drawing.Point(197, 7);
            this.cutInCheckBox.Name = "cutInCheckBox";
            this.cutInCheckBox.Size = new System.Drawing.Size(53, 17);
            this.cutInCheckBox.TabIndex = 5;
            this.cutInCheckBox.Text = "Cut in";
            this.cutInCheckBox.UseVisualStyleBackColor = true;
            this.cutInCheckBox.CheckedChanged += new System.EventHandler(this.cutInCheckBox_CheckedChanged);
            // 
            // freeCheckBox
            // 
            this.freeCheckBox.AutoSize = true;
            this.freeCheckBox.Checked = true;
            this.freeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.freeCheckBox.Location = new System.Drawing.Point(144, 7);
            this.freeCheckBox.Name = "freeCheckBox";
            this.freeCheckBox.Size = new System.Drawing.Size(47, 17);
            this.freeCheckBox.TabIndex = 4;
            this.freeCheckBox.Text = "Free";
            this.freeCheckBox.UseVisualStyleBackColor = true;
            this.freeCheckBox.CheckedChanged += new System.EventHandler(this.freeCheckBox_CheckedChanged);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(428, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(41, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "[]";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(381, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "| |";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(334, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(41, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = ">";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(3, 3);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(41, 23);
            this.openButton.TabIndex = 0;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Show tool path:";
            // 
            // FormSceneSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 460);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormSceneSample";
            this.Text = "GCode Player";
            ((System.ComponentModel.ISupportInitialize)(this.sceneControl)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.SceneControl sceneControl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cutCheckBox;
        private System.Windows.Forms.CheckBox cutInCheckBox;
        private System.Windows.Forms.CheckBox freeCheckBox;
        private System.Windows.Forms.Label label1;
    }
}

