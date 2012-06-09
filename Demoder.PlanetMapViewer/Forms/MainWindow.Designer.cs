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
            this.pluginManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tileDisplay1_vScrollBar = new System.Windows.Forms.VScrollBar();
            this.tileDisplay1_hScrollBar = new System.Windows.Forms.HScrollBar();
            this.tileDisplay1 = new Demoder.PlanetMapViewer.Xna.TileDisplay();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pluginsPanel = new System.Windows.Forms.GroupBox();
            this.pluginListControl1 = new Demoder.PlanetMapViewer.Forms.PluginListControl();
            this.followCharacterGroupBox = new System.Windows.Forms.GroupBox();
            this.characterTrackerControl1 = new Demoder.PlanetMapViewer.Forms.CharacterTrackerControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.magnificationLabel = new System.Windows.Forms.Label();
            this.MagnificationSlider = new System.Windows.Forms.TrackBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mapSelectionControl1 = new Demoder.PlanetMapViewer.Forms.MapSelectionControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cameraUserControl1 = new Demoder.PlanetMapViewer.Forms.CameraUserControl();
            this.regionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rubikaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shadowlandsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cameraControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followCharactersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.OverlayTitleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginManagerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitOverlayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.pluginsPanel.SuspendLayout();
            this.followCharacterGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MagnificationSlider)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(725, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.pluginManagerToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // pluginManagerToolStripMenuItem
            // 
            this.pluginManagerToolStripMenuItem.Name = "pluginManagerToolStripMenuItem";
            this.pluginManagerToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.pluginManagerToolStripMenuItem.Text = "Plugin Manager";
            this.pluginManagerToolStripMenuItem.Click += new System.EventHandler(this.pluginManagerToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
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
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // checkVersionToolStripMenuItem
            // 
            this.checkVersionToolStripMenuItem.Name = "checkVersionToolStripMenuItem";
            this.checkVersionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.checkVersionToolStripMenuItem.Text = "Update...";
            this.checkVersionToolStripMenuItem.Click += new System.EventHandler(this.checkVersionToolStripMenuItem_Click);
            // 
            // readmeToolStripMenuItem
            // 
            this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
            this.readmeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.readmeToolStripMenuItem.Text = "Readme";
            this.readmeToolStripMenuItem.Click += new System.EventHandler(this.readmeToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 591);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(725, 22);
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(725, 567);
            this.splitContainer1.SplitterDistance = 553;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // tileDisplay1_vScrollBar
            // 
            this.tileDisplay1_vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_vScrollBar.Location = new System.Drawing.Point(539, 0);
            this.tileDisplay1_vScrollBar.Name = "tileDisplay1_vScrollBar";
            this.tileDisplay1_vScrollBar.Size = new System.Drawing.Size(15, 552);
            this.tileDisplay1_vScrollBar.TabIndex = 3;
            this.tileDisplay1_vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1_hScrollBar
            // 
            this.tileDisplay1_hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_hScrollBar.Location = new System.Drawing.Point(0, 552);
            this.tileDisplay1_hScrollBar.Name = "tileDisplay1_hScrollBar";
            this.tileDisplay1_hScrollBar.Size = new System.Drawing.Size(552, 15);
            this.tileDisplay1_hScrollBar.TabIndex = 2;
            this.tileDisplay1_hScrollBar.Value = 1;
            this.tileDisplay1_hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileDisplay1.Location = new System.Drawing.Point(0, 0);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(553, 567);
            this.tileDisplay1.TabIndex = 1;
            this.tileDisplay1.Text = "PlanetMap";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(5, 303);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pluginsPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.followCharacterGroupBox);
            this.splitContainer2.Size = new System.Drawing.Size(166, 259);
            this.splitContainer2.SplitterDistance = 129;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 10005;
            // 
            // pluginsPanel
            // 
            this.pluginsPanel.Controls.Add(this.pluginListControl1);
            this.pluginsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginsPanel.Location = new System.Drawing.Point(0, 0);
            this.pluginsPanel.Name = "pluginsPanel";
            this.pluginsPanel.Size = new System.Drawing.Size(166, 129);
            this.pluginsPanel.TabIndex = 10004;
            this.pluginsPanel.TabStop = false;
            this.pluginsPanel.Text = "Plugins";
            // 
            // pluginListControl1
            // 
            this.pluginListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginListControl1.Location = new System.Drawing.Point(3, 16);
            this.pluginListControl1.Name = "pluginListControl1";
            this.pluginListControl1.Size = new System.Drawing.Size(160, 110);
            this.pluginListControl1.TabIndex = 0;
            // 
            // followCharacterGroupBox
            // 
            this.followCharacterGroupBox.Controls.Add(this.characterTrackerControl1);
            this.followCharacterGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.followCharacterGroupBox.Location = new System.Drawing.Point(0, 0);
            this.followCharacterGroupBox.Name = "followCharacterGroupBox";
            this.followCharacterGroupBox.Size = new System.Drawing.Size(166, 129);
            this.followCharacterGroupBox.TabIndex = 10000;
            this.followCharacterGroupBox.TabStop = false;
            this.followCharacterGroupBox.Text = "Follow Character";
            // 
            // characterTrackerControl1
            // 
            this.characterTrackerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.characterTrackerControl1.Location = new System.Drawing.Point(3, 16);
            this.characterTrackerControl1.Name = "characterTrackerControl1";
            this.characterTrackerControl1.Size = new System.Drawing.Size(160, 110);
            this.characterTrackerControl1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.magnificationLabel);
            this.groupBox3.Controls.Add(this.MagnificationSlider);
            this.groupBox3.Location = new System.Drawing.Point(3, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(168, 67);
            this.groupBox3.TabIndex = 10003;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Magnification";
            // 
            // magnificationLabel
            // 
            this.magnificationLabel.AutoSize = true;
            this.magnificationLabel.Location = new System.Drawing.Point(72, 48);
            this.magnificationLabel.Name = "magnificationLabel";
            this.magnificationLabel.Size = new System.Drawing.Size(33, 13);
            this.magnificationLabel.TabIndex = 10002;
            this.magnificationLabel.Text = "100%";
            // 
            // MagnificationSlider
            // 
            this.MagnificationSlider.LargeChange = 1;
            this.MagnificationSlider.Location = new System.Drawing.Point(7, 16);
            this.MagnificationSlider.Maximum = 2;
            this.MagnificationSlider.Minimum = -3;
            this.MagnificationSlider.Name = "MagnificationSlider";
            this.MagnificationSlider.Size = new System.Drawing.Size(155, 45);
            this.MagnificationSlider.TabIndex = 10001;
            this.MagnificationSlider.TickFrequency = 25;
            this.MagnificationSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.MagnificationSlider.ValueChanged += new System.EventHandler(this.MagnificationSlider_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mapSelectionControl1);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(168, 118);
            this.groupBox4.TabIndex = 10000;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Map Selection";
            // 
            // mapSelectionControl1
            // 
            this.mapSelectionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapSelectionControl1.Location = new System.Drawing.Point(3, 16);
            this.mapSelectionControl1.Name = "mapSelectionControl1";
            this.mapSelectionControl1.Size = new System.Drawing.Size(162, 99);
            this.mapSelectionControl1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cameraUserControl1);
            this.groupBox2.Location = new System.Drawing.Point(3, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 97);
            this.groupBox2.TabIndex = 10000;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Controls";
            // 
            // cameraUserControl1
            // 
            this.cameraUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraUserControl1.Location = new System.Drawing.Point(3, 16);
            this.cameraUserControl1.Name = "cameraUserControl1";
            this.cameraUserControl1.Size = new System.Drawing.Size(159, 78);
            this.cameraUserControl1.TabIndex = 0;
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
            this.toolStripSeparator2,
            this.regionToolStripMenuItem,
            this.rubikaToolStripMenuItem1,
            this.shadowlandsToolStripMenuItem1,
            this.autoToolStripMenuItem,
            this.toolStripSeparator3,
            this.selectCharactersToolStripMenuItem,
            this.selectMapToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.toolStripSeparator4,
            this.optionsToolStripMenuItem1,
            this.pluginManagerToolStripMenuItem1,
            this.exitOverlayModeToolStripMenuItem});
            this.OverlayTitleContextMenuStrip.Name = "contextMenuStrip1";
            this.OverlayTitleContextMenuStrip.ShowCheckMargin = true;
            this.OverlayTitleContextMenuStrip.ShowImageMargin = false;
            this.OverlayTitleContextMenuStrip.Size = new System.Drawing.Size(192, 286);
            this.OverlayTitleContextMenuStrip.Text = "Overlay Mode";
            this.OverlayTitleContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OverlayTitleContextMenuStrip_Opening);
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
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.pluginsToolStripMenuItem.Text = "Plugins";
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
            // pluginManagerToolStripMenuItem1
            // 
            this.pluginManagerToolStripMenuItem1.Name = "pluginManagerToolStripMenuItem1";
            this.pluginManagerToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.pluginManagerToolStripMenuItem1.Text = "Plugin Manager...";
            this.pluginManagerToolStripMenuItem1.Click += new System.EventHandler(this.pluginManagerToolStripMenuItem1_Click);
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
            this.ClientSize = new System.Drawing.Size(725, 613);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "MainWindow";
            this.Text = "Demoder\'s Planet Map Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowFormClosing);
            this.Load += new System.EventHandler(this.MainWindowFormLoad);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.pluginsPanel.ResumeLayout(false);
            this.followCharacterGroupBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MagnificationSlider)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox followCharacterGroupBox;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.HScrollBar tileDisplay1_hScrollBar;
        private System.Windows.Forms.VScrollBar tileDisplay1_vScrollBar;
        private System.Windows.Forms.ToolStripMenuItem checkVersionToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
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
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoToolStripMenuItem;
        private System.Windows.Forms.Label magnificationLabel;
        internal System.Windows.Forms.TrackBar MagnificationSlider;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStripMenuItem pluginManagerToolStripMenuItem;
        private System.Windows.Forms.GroupBox pluginsPanel;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private PluginListControl pluginListControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MapSelectionControl mapSelectionControl1;
        private CameraUserControl cameraUserControl1;
        private CharacterTrackerControl characterTrackerControl1;
        private System.Windows.Forms.ToolStripMenuItem pluginManagerToolStripMenuItem1;
    }
}

