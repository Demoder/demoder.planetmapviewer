/*
* Demoder.PlanetMapViewer
* Copyright (C) 2012 Demoder (demoder@demoder.me)
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Demoder.PlanetMapViewer.Xna;
namespace Demoder.PlanetMapViewer.Forms
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OverlayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.errorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tileDisplay1_vScrollBar = new System.Windows.Forms.VScrollBar();
            this.tileDisplay1_hScrollBar = new System.Windows.Forms.HScrollBar();
            this.tileDisplay1 = new Demoder.PlanetMapViewer.Xna.TileDisplay();
            this.magnificationLabel = new System.Windows.Forms.Label();
            this.MagnificationSlider = new System.Windows.Forms.TrackBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.RadioButtonMapSelectionAuto = new System.Windows.Forms.RadioButton();
            this.RadioButtonMapSelectionShadowlands = new System.Windows.Forms.RadioButton();
            this.RadioButtonMapSelectionRubika = new System.Windows.Forms.RadioButton();
            this.mapComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CameraFollowMission = new System.Windows.Forms.Button();
            this.CameraFollowCharacer = new System.Windows.Forms.Button();
            this.ButtonZoomOut = new System.Windows.Forms.Button();
            this.ButtonZoomIn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.followCharacter = new System.Windows.Forms.CheckedListBox();
            this.regionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rubikaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shadowlandsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cameraControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followCharactersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.OverlayTitleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.missionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitOverlayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MagnificationSlider)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.OverlayTitleContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.OverlayModeToolStripMenuItem,
            this.toolStripSeparator6,
            this.saveToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.editToolStripMenuItem.Text = "&View";
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.CheckOnClick = true;
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.fullscreenToolStripMenuItem.Text = "Fullscreen";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.MenuViewFullscreen);
            // 
            // OverlayModeToolStripMenuItem
            // 
            this.OverlayModeToolStripMenuItem.CheckOnClick = true;
            this.OverlayModeToolStripMenuItem.Name = "OverlayModeToolStripMenuItem";
            this.OverlayModeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.OverlayModeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.OverlayModeToolStripMenuItem.Text = "Overlay Mode";
            this.OverlayModeToolStripMenuItem.Click += new System.EventHandler(this.hideSidebarToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(170, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.saveToolStripMenuItem.Text = "&Screenshot";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkVersionToolStripMenuItem,
            this.readmeToolStripMenuItem,
            this.toolStripSeparator5,
            this.errorLogToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // checkVersionToolStripMenuItem
            // 
            this.checkVersionToolStripMenuItem.Name = "checkVersionToolStripMenuItem";
            this.checkVersionToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.checkVersionToolStripMenuItem.Text = "Update...";
            this.checkVersionToolStripMenuItem.Click += new System.EventHandler(this.checkVersionToolStripMenuItem_Click);
            // 
            // readmeToolStripMenuItem
            // 
            this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
            this.readmeToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.readmeToolStripMenuItem.Text = "Readme";
            this.readmeToolStripMenuItem.Click += new System.EventHandler(this.readmeToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // errorLogToolStripMenuItem
            // 
            this.errorLogToolStripMenuItem.Name = "errorLogToolStripMenuItem";
            this.errorLogToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.errorLogToolStripMenuItem.Text = "Error Log";
            this.errorLogToolStripMenuItem.Click += new System.EventHandler(this.errorLogToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 580);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 10000;
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tileDisplay1_vScrollBar);
            this.splitContainer1.Panel1.Controls.Add(this.tileDisplay1_hScrollBar);
            this.splitContainer1.Panel1.Controls.Add(this.tileDisplay1);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.magnificationLabel);
            this.splitContainer1.Panel2.Controls.Add(this.MagnificationSlider);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(784, 556);
            this.splitContainer1.SplitterDistance = 605;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // tileDisplay1_vScrollBar
            // 
            this.tileDisplay1_vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_vScrollBar.Location = new System.Drawing.Point(591, 0);
            this.tileDisplay1_vScrollBar.Name = "tileDisplay1_vScrollBar";
            this.tileDisplay1_vScrollBar.Size = new System.Drawing.Size(15, 541);
            this.tileDisplay1_vScrollBar.TabIndex = 3;
            this.tileDisplay1_vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1_hScrollBar
            // 
            this.tileDisplay1_hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_hScrollBar.Location = new System.Drawing.Point(0, 541);
            this.tileDisplay1_hScrollBar.Name = "tileDisplay1_hScrollBar";
            this.tileDisplay1_hScrollBar.Size = new System.Drawing.Size(605, 15);
            this.tileDisplay1_hScrollBar.TabIndex = 2;
            this.tileDisplay1_hScrollBar.Value = 1;
            this.tileDisplay1_hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileDisplay1.Location = new System.Drawing.Point(0, 0);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(605, 556);
            this.tileDisplay1.TabIndex = 1;
            this.tileDisplay1.Text = "PlanetMap";
            this.tileDisplay1.OnDraw += new System.EventHandler(this.tileDisplay1_OnDraw);
            // 
            // magnificationLabel
            // 
            this.magnificationLabel.AutoSize = true;
            this.magnificationLabel.Location = new System.Drawing.Point(65, 387);
            this.magnificationLabel.Name = "magnificationLabel";
            this.magnificationLabel.Size = new System.Drawing.Size(25, 13);
            this.magnificationLabel.TabIndex = 10002;
            this.magnificationLabel.Text = "100";
            // 
            // MagnificationSlider
            // 
            this.MagnificationSlider.LargeChange = 25;
            this.MagnificationSlider.Location = new System.Drawing.Point(5, 355);
            this.MagnificationSlider.Maximum = 200;
            this.MagnificationSlider.Minimum = 25;
            this.MagnificationSlider.Name = "MagnificationSlider";
            this.MagnificationSlider.Size = new System.Drawing.Size(166, 45);
            this.MagnificationSlider.SmallChange = 5;
            this.MagnificationSlider.TabIndex = 10001;
            this.MagnificationSlider.TickFrequency = 25;
            this.MagnificationSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.MagnificationSlider.Value = 100;
            this.MagnificationSlider.ValueChanged += new System.EventHandler(this.MagnificationSlider_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.RadioButtonMapSelectionAuto);
            this.groupBox4.Controls.Add(this.RadioButtonMapSelectionShadowlands);
            this.groupBox4.Controls.Add(this.RadioButtonMapSelectionRubika);
            this.groupBox4.Controls.Add(this.mapComboBox);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(168, 118);
            this.groupBox4.TabIndex = 10000;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Map Selection";
            // 
            // RadioButtonMapSelectionAuto
            // 
            this.RadioButtonMapSelectionAuto.AutoSize = true;
            this.RadioButtonMapSelectionAuto.Checked = true;
            this.RadioButtonMapSelectionAuto.Location = new System.Drawing.Point(8, 65);
            this.RadioButtonMapSelectionAuto.Name = "RadioButtonMapSelectionAuto";
            this.RadioButtonMapSelectionAuto.Size = new System.Drawing.Size(47, 17);
            this.RadioButtonMapSelectionAuto.TabIndex = 4;
            this.RadioButtonMapSelectionAuto.TabStop = true;
            this.RadioButtonMapSelectionAuto.Text = "Auto";
            this.RadioButtonMapSelectionAuto.UseVisualStyleBackColor = true;
            // 
            // RadioButtonMapSelectionShadowlands
            // 
            this.RadioButtonMapSelectionShadowlands.AutoSize = true;
            this.RadioButtonMapSelectionShadowlands.Location = new System.Drawing.Point(8, 42);
            this.RadioButtonMapSelectionShadowlands.Name = "RadioButtonMapSelectionShadowlands";
            this.RadioButtonMapSelectionShadowlands.Size = new System.Drawing.Size(89, 17);
            this.RadioButtonMapSelectionShadowlands.TabIndex = 3;
            this.RadioButtonMapSelectionShadowlands.Text = "Shadowlands";
            this.RadioButtonMapSelectionShadowlands.UseVisualStyleBackColor = true;
            this.RadioButtonMapSelectionShadowlands.CheckedChanged += new System.EventHandler(this.RadioMapTypeCheckedChanged);
            // 
            // RadioButtonMapSelectionRubika
            // 
            this.RadioButtonMapSelectionRubika.AutoSize = true;
            this.RadioButtonMapSelectionRubika.Location = new System.Drawing.Point(8, 19);
            this.RadioButtonMapSelectionRubika.Name = "RadioButtonMapSelectionRubika";
            this.RadioButtonMapSelectionRubika.Size = new System.Drawing.Size(63, 17);
            this.RadioButtonMapSelectionRubika.TabIndex = 2;
            this.RadioButtonMapSelectionRubika.Text = "Rubi-Ka";
            this.RadioButtonMapSelectionRubika.UseVisualStyleBackColor = true;
            this.RadioButtonMapSelectionRubika.CheckedChanged += new System.EventHandler(this.RadioMapTypeCheckedChanged);
            // 
            // mapComboBox
            // 
            this.mapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapComboBox.FormattingEnabled = true;
            this.mapComboBox.Location = new System.Drawing.Point(2, 91);
            this.mapComboBox.Name = "mapComboBox";
            this.mapComboBox.Size = new System.Drawing.Size(159, 21);
            this.mapComboBox.Sorted = true;
            this.mapComboBox.TabIndex = 3;
            this.mapComboBox.SelectedIndexChanged += new System.EventHandler(this.mapComboBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CameraFollowMission);
            this.groupBox2.Controls.Add(this.CameraFollowCharacer);
            this.groupBox2.Controls.Add(this.ButtonZoomOut);
            this.groupBox2.Controls.Add(this.ButtonZoomIn);
            this.groupBox2.Location = new System.Drawing.Point(3, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 97);
            this.groupBox2.TabIndex = 10000;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Controls";
            // 
            // CameraFollowMission
            // 
            this.CameraFollowMission.Location = new System.Drawing.Point(86, 19);
            this.CameraFollowMission.Name = "CameraFollowMission";
            this.CameraFollowMission.Size = new System.Drawing.Size(75, 23);
            this.CameraFollowMission.TabIndex = 8;
            this.CameraFollowMission.Text = "Mission";
            this.CameraFollowMission.UseVisualStyleBackColor = true;
            this.CameraFollowMission.Click += new System.EventHandler(this.CameraFollowMission_Click);
            // 
            // CameraFollowCharacer
            // 
            this.CameraFollowCharacer.Location = new System.Drawing.Point(6, 19);
            this.CameraFollowCharacer.Name = "CameraFollowCharacer";
            this.CameraFollowCharacer.Size = new System.Drawing.Size(75, 23);
            this.CameraFollowCharacer.TabIndex = 7;
            this.CameraFollowCharacer.Text = "Character";
            this.CameraFollowCharacer.UseVisualStyleBackColor = true;
            this.CameraFollowCharacer.Click += new System.EventHandler(this.CameraFollowCharacer_Click);
            // 
            // ButtonZoomOut
            // 
            this.ButtonZoomOut.Location = new System.Drawing.Point(76, 65);
            this.ButtonZoomOut.Name = "ButtonZoomOut";
            this.ButtonZoomOut.Size = new System.Drawing.Size(62, 23);
            this.ButtonZoomOut.TabIndex = 6;
            this.ButtonZoomOut.Text = "Zoom Out";
            this.ButtonZoomOut.UseVisualStyleBackColor = true;
            this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.Location = new System.Drawing.Point(8, 65);
            this.ButtonZoomIn.Name = "ButtonZoomIn";
            this.ButtonZoomIn.Size = new System.Drawing.Size(62, 23);
            this.ButtonZoomIn.TabIndex = 5;
            this.ButtonZoomIn.Text = "Zoom In";
            this.ButtonZoomIn.UseVisualStyleBackColor = true;
            this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.followCharacter);
            this.groupBox1.Location = new System.Drawing.Point(3, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 106);
            this.groupBox1.TabIndex = 10000;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Follow Character";
            // 
            // followCharacter
            // 
            this.followCharacter.CheckOnClick = true;
            this.followCharacter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.followCharacter.Location = new System.Drawing.Point(3, 16);
            this.followCharacter.Name = "followCharacter";
            this.followCharacter.ScrollAlwaysVisible = true;
            this.followCharacter.Size = new System.Drawing.Size(162, 87);
            this.followCharacter.Sorted = true;
            this.followCharacter.TabIndex = 7;
            this.followCharacter.Click += new System.EventHandler(this.followCharacter_Click);
            // 
            // regionToolStripMenuItem
            // 
            this.regionToolStripMenuItem.Enabled = false;
            this.regionToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.regionToolStripMenuItem.Name = "regionToolStripMenuItem";
            this.regionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.regionToolStripMenuItem.Text = "-Region-";
            // 
            // rubikaToolStripMenuItem1
            // 
            this.rubikaToolStripMenuItem1.Name = "rubikaToolStripMenuItem1";
            this.rubikaToolStripMenuItem1.ShortcutKeyDisplayString = "R";
            this.rubikaToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.rubikaToolStripMenuItem1.Text = "&Rubi-ka";
            this.rubikaToolStripMenuItem1.Click += new System.EventHandler(this.rubikaToolStripMenuItem1_Click);
            // 
            // shadowlandsToolStripMenuItem1
            // 
            this.shadowlandsToolStripMenuItem1.Name = "shadowlandsToolStripMenuItem1";
            this.shadowlandsToolStripMenuItem1.ShortcutKeyDisplayString = "S";
            this.shadowlandsToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.shadowlandsToolStripMenuItem1.Text = "&Shadowlands";
            this.shadowlandsToolStripMenuItem1.Click += new System.EventHandler(this.shadowlandsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);
            // 
            // cameraControlsToolStripMenuItem
            // 
            this.cameraControlsToolStripMenuItem.Enabled = false;
            this.cameraControlsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraControlsToolStripMenuItem.Name = "cameraControlsToolStripMenuItem";
            this.cameraControlsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.cameraControlsToolStripMenuItem.Text = "-Camera Controls-";
            // 
            // followCharactersToolStripMenuItem1
            // 
            this.followCharactersToolStripMenuItem1.Name = "followCharactersToolStripMenuItem1";
            this.followCharactersToolStripMenuItem1.ShortcutKeyDisplayString = "C";
            this.followCharactersToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.followCharactersToolStripMenuItem1.Text = "Follow &Character(s)";
            this.followCharactersToolStripMenuItem1.Click += new System.EventHandler(this.followCharactersToolStripMenuItem1_Click);
            // 
            // OverlayTitleContextMenuStrip
            // 
            this.OverlayTitleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraControlsToolStripMenuItem,
            this.followCharactersToolStripMenuItem1,
            this.missionToolStripMenuItem,
            this.toolStripSeparator2,
            this.regionToolStripMenuItem,
            this.rubikaToolStripMenuItem1,
            this.shadowlandsToolStripMenuItem1,
            this.autoToolStripMenuItem,
            this.toolStripSeparator3,
            this.selectCharactersToolStripMenuItem,
            this.selectMapToolStripMenuItem,
            this.toolStripSeparator4,
            this.optionsToolStripMenuItem1,
            this.exitOverlayModeToolStripMenuItem});
            this.OverlayTitleContextMenuStrip.Name = "contextMenuStrip1";
            this.OverlayTitleContextMenuStrip.ShowCheckMargin = true;
            this.OverlayTitleContextMenuStrip.ShowImageMargin = false;
            this.OverlayTitleContextMenuStrip.Size = new System.Drawing.Size(192, 264);
            this.OverlayTitleContextMenuStrip.Text = "Overlay Mode";
            this.OverlayTitleContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OverlayTitleContextMenuStrip_Opening);
            // 
            // missionToolStripMenuItem
            // 
            this.missionToolStripMenuItem.Name = "missionToolStripMenuItem";
            this.missionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.missionToolStripMenuItem.Text = "Mission";
            this.missionToolStripMenuItem.Click += new System.EventHandler(this.missionToolStripMenuItem_Click);
            // 
            // autoToolStripMenuItem
            // 
            this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
            this.autoToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.autoToolStripMenuItem.Text = "&Auto";
            this.autoToolStripMenuItem.Click += new System.EventHandler(this.autoToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // selectCharactersToolStripMenuItem
            // 
            this.selectCharactersToolStripMenuItem.Name = "selectCharactersToolStripMenuItem";
            this.selectCharactersToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.selectCharactersToolStripMenuItem.Text = "Select Characters";
            // 
            // selectMapToolStripMenuItem
            // 
            this.selectMapToolStripMenuItem.Name = "selectMapToolStripMenuItem";
            this.selectMapToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.selectMapToolStripMenuItem.Text = "Select Map";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(188, 6);
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.optionsToolStripMenuItem1.Text = "Options...";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // exitOverlayModeToolStripMenuItem
            // 
            this.exitOverlayModeToolStripMenuItem.Name = "exitOverlayModeToolStripMenuItem";
            this.exitOverlayModeToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exitOverlayModeToolStripMenuItem.Text = "Exit Overlay Mode";
            this.exitOverlayModeToolStripMenuItem.Click += new System.EventHandler(this.exitOverlayModeToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 602);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "MainWindow";
            this.Text = "Demoder\'s Planet Map Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MagnificationSlider)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.OverlayTitleContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TileDisplay tileDisplay1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox followCharacter;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.HScrollBar tileDisplay1_hScrollBar;
        private System.Windows.Forms.VScrollBar tileDisplay1_vScrollBar;
        private System.Windows.Forms.ComboBox mapComboBox;
        private System.Windows.Forms.ToolStripMenuItem checkVersionToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.RadioButton RadioButtonMapSelectionRubika;
        internal System.Windows.Forms.RadioButton RadioButtonMapSelectionShadowlands;
        private System.Windows.Forms.Button ButtonZoomOut;
        private System.Windows.Forms.Button ButtonZoomIn;
        private System.Windows.Forms.ToolStripMenuItem readmeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem regionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rubikaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem shadowlandsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cameraControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem followCharactersToolStripMenuItem1;
        internal System.Windows.Forms.ContextMenuStrip OverlayTitleContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem selectMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitOverlayModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem OverlayModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem errorLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.RadioButton RadioButtonMapSelectionAuto;
        private System.Windows.Forms.ToolStripMenuItem autoToolStripMenuItem;
        private System.Windows.Forms.Button CameraFollowMission;
        private System.Windows.Forms.Button CameraFollowCharacer;
        private System.Windows.Forms.Label magnificationLabel;
        private System.Windows.Forms.ToolStripMenuItem missionToolStripMenuItem;
        internal System.Windows.Forms.TrackBar MagnificationSlider;
    }
}

