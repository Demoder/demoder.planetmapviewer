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

namespace Demoder.PlanetMapViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tileDisplay1_vScrollBar = new System.Windows.Forms.VScrollBar();
            this.tileDisplay1_hScrollBar = new System.Windows.Forms.HScrollBar();
            this.tileDisplay1 = new Demoder.PlanetMapViewer.TileDisplay();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.RadioButtonMapSelectionShadowlands = new System.Windows.Forms.RadioButton();
            this.RadioButtonMapSelectionRubika = new System.Windows.Forms.RadioButton();
            this.mapComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonZoomOut = new System.Windows.Forms.Button();
            this.ButtonZoomIn = new System.Windows.Forms.Button();
            this.RadioButtonCameraManual = new System.Windows.Forms.RadioButton();
            this.RadioButtonCameraFollowCharacters = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.followCharacter = new System.Windows.Forms.CheckedListBox();
            this.regionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rubikaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shadowlandsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cameraControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followCharactersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.OverlayTitleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitOverlayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.OverlayTitleContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.overlayModeToolStripMenuItem});
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
            // overlayModeToolStripMenuItem
            // 
            this.overlayModeToolStripMenuItem.CheckOnClick = true;
            this.overlayModeToolStripMenuItem.Name = "overlayModeToolStripMenuItem";
            this.overlayModeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.overlayModeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.overlayModeToolStripMenuItem.Text = "Overlay Mode";
            this.overlayModeToolStripMenuItem.Click += new System.EventHandler(this.hideSidebarToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveToolStripMenuItem.Text = "&Screenshot";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkVersionToolStripMenuItem,
            this.readmeToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // checkVersionToolStripMenuItem
            // 
            this.checkVersionToolStripMenuItem.Name = "checkVersionToolStripMenuItem";
            this.checkVersionToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.checkVersionToolStripMenuItem.Text = "Update...";
            this.checkVersionToolStripMenuItem.Click += new System.EventHandler(this.checkVersionToolStripMenuItem_Click);
            // 
            // readmeToolStripMenuItem
            // 
            this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
            this.readmeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.readmeToolStripMenuItem.Text = "Readme";
            this.readmeToolStripMenuItem.Click += new System.EventHandler(this.readmeToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 470);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(684, 22);
            this.statusStrip1.TabIndex = 10000;
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(121, 17);
            this.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1";
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
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(684, 446);
            this.splitContainer1.SplitterDistance = 515;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // tileDisplay1_vScrollBar
            // 
            this.tileDisplay1_vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_vScrollBar.Location = new System.Drawing.Point(501, 0);
            this.tileDisplay1_vScrollBar.Name = "tileDisplay1_vScrollBar";
            this.tileDisplay1_vScrollBar.Size = new System.Drawing.Size(15, 431);
            this.tileDisplay1_vScrollBar.TabIndex = 3;
            this.tileDisplay1_vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1_hScrollBar
            // 
            this.tileDisplay1_hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1_hScrollBar.Location = new System.Drawing.Point(0, 431);
            this.tileDisplay1_hScrollBar.Name = "tileDisplay1_hScrollBar";
            this.tileDisplay1_hScrollBar.Size = new System.Drawing.Size(515, 15);
            this.tileDisplay1_hScrollBar.TabIndex = 2;
            this.tileDisplay1_hScrollBar.Value = 1;
            this.tileDisplay1_hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileDisplay1_ScrollBar_Scroll);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileDisplay1.Location = new System.Drawing.Point(0, 0);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(515, 446);
            this.tileDisplay1.TabIndex = 1;
            this.tileDisplay1.Text = "PlanetMap";
            this.tileDisplay1.OnDraw += new System.EventHandler(this.tileDisplay1_OnDraw);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.Controls.Add(this.RadioButtonMapSelectionShadowlands);
            this.groupBox4.Controls.Add(this.RadioButtonMapSelectionRubika);
            this.groupBox4.Controls.Add(this.mapComboBox);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(174, 71);
            this.groupBox4.TabIndex = 10000;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Map Selection";
            // 
            // RadioButtonMapSelectionShadowlands
            // 
            this.RadioButtonMapSelectionShadowlands.AutoSize = true;
            this.RadioButtonMapSelectionShadowlands.Location = new System.Drawing.Point(73, 19);
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
            this.RadioButtonMapSelectionRubika.Checked = true;
            this.RadioButtonMapSelectionRubika.Location = new System.Drawing.Point(8, 19);
            this.RadioButtonMapSelectionRubika.Name = "RadioButtonMapSelectionRubika";
            this.RadioButtonMapSelectionRubika.Size = new System.Drawing.Size(63, 17);
            this.RadioButtonMapSelectionRubika.TabIndex = 2;
            this.RadioButtonMapSelectionRubika.TabStop = true;
            this.RadioButtonMapSelectionRubika.Text = "Rubi-Ka";
            this.RadioButtonMapSelectionRubika.UseVisualStyleBackColor = true;
            this.RadioButtonMapSelectionRubika.CheckedChanged += new System.EventHandler(this.RadioMapTypeCheckedChanged);
            // 
            // mapComboBox
            // 
            this.mapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapComboBox.FormattingEnabled = true;
            this.mapComboBox.Location = new System.Drawing.Point(3, 42);
            this.mapComboBox.Name = "mapComboBox";
            this.mapComboBox.Size = new System.Drawing.Size(159, 21);
            this.mapComboBox.Sorted = true;
            this.mapComboBox.TabIndex = 3;
            this.mapComboBox.SelectedIndexChanged += new System.EventHandler(this.mapComboBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ButtonZoomOut);
            this.groupBox2.Controls.Add(this.ButtonZoomIn);
            this.groupBox2.Controls.Add(this.RadioButtonCameraManual);
            this.groupBox2.Controls.Add(this.RadioButtonCameraFollowCharacters);
            this.groupBox2.Location = new System.Drawing.Point(3, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 97);
            this.groupBox2.TabIndex = 10000;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Controls";
            // 
            // ButtonZoomOut
            // 
            this.ButtonZoomOut.Location = new System.Drawing.Point(76, 65);
            this.ButtonZoomOut.Name = "ButtonZoomOut";
            this.ButtonZoomOut.Size = new System.Drawing.Size(62, 23);
            this.ButtonZoomOut.TabIndex = 6;
            this.ButtonZoomOut.Text = "Zoom out";
            this.ButtonZoomOut.UseVisualStyleBackColor = true;
            this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.Location = new System.Drawing.Point(8, 65);
            this.ButtonZoomIn.Name = "ButtonZoomIn";
            this.ButtonZoomIn.Size = new System.Drawing.Size(62, 23);
            this.ButtonZoomIn.TabIndex = 5;
            this.ButtonZoomIn.Text = "Zoom in";
            this.ButtonZoomIn.UseVisualStyleBackColor = true;
            this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // RadioButtonCameraManual
            // 
            this.RadioButtonCameraManual.AutoSize = true;
            this.RadioButtonCameraManual.Location = new System.Drawing.Point(6, 42);
            this.RadioButtonCameraManual.Name = "RadioButtonCameraManual";
            this.RadioButtonCameraManual.Size = new System.Drawing.Size(60, 17);
            this.RadioButtonCameraManual.TabIndex = 6;
            this.RadioButtonCameraManual.Text = "Manual";
            this.RadioButtonCameraManual.UseVisualStyleBackColor = true;
            this.RadioButtonCameraManual.CheckedChanged += new System.EventHandler(this.RadioButtonCameraControlCheckChanged);
            // 
            // RadioButtonCameraFollowCharacters
            // 
            this.RadioButtonCameraFollowCharacters.AutoSize = true;
            this.RadioButtonCameraFollowCharacters.Checked = true;
            this.RadioButtonCameraFollowCharacters.Location = new System.Drawing.Point(6, 19);
            this.RadioButtonCameraFollowCharacters.Name = "RadioButtonCameraFollowCharacters";
            this.RadioButtonCameraFollowCharacters.Size = new System.Drawing.Size(115, 17);
            this.RadioButtonCameraFollowCharacters.TabIndex = 4;
            this.RadioButtonCameraFollowCharacters.TabStop = true;
            this.RadioButtonCameraFollowCharacters.Text = "Follow Character(s)";
            this.RadioButtonCameraFollowCharacters.UseVisualStyleBackColor = true;
            this.RadioButtonCameraFollowCharacters.CheckedChanged += new System.EventHandler(this.RadioButtonCameraControlCheckChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.followCharacter);
            this.groupBox1.Location = new System.Drawing.Point(3, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 106);
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
            this.followCharacter.Size = new System.Drawing.Size(168, 87);
            this.followCharacter.Sorted = true;
            this.followCharacter.TabIndex = 7;
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
            // manualToolStripMenuItem1
            // 
            this.manualToolStripMenuItem1.Name = "manualToolStripMenuItem1";
            this.manualToolStripMenuItem1.ShortcutKeyDisplayString = "M";
            this.manualToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.manualToolStripMenuItem1.Text = "&Manual";
            this.manualToolStripMenuItem1.Click += new System.EventHandler(this.manualToolStripMenuItem1_Click);
            // 
            // OverlayTitleContextMenuStrip
            // 
            this.OverlayTitleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraControlsToolStripMenuItem,
            this.followCharactersToolStripMenuItem1,
            this.manualToolStripMenuItem1,
            this.toolStripSeparator2,
            this.regionToolStripMenuItem,
            this.rubikaToolStripMenuItem1,
            this.shadowlandsToolStripMenuItem1,
            this.toolStripSeparator3,
            this.selectCharactersToolStripMenuItem,
            this.selectMapToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitOverlayModeToolStripMenuItem});
            this.OverlayTitleContextMenuStrip.Name = "contextMenuStrip1";
            this.OverlayTitleContextMenuStrip.ShowCheckMargin = true;
            this.OverlayTitleContextMenuStrip.ShowImageMargin = false;
            this.OverlayTitleContextMenuStrip.Size = new System.Drawing.Size(192, 220);
            this.OverlayTitleContextMenuStrip.Text = "Overlay Mode";
            this.OverlayTitleContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OverlayTitleContextMenuStrip_Opening);
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
            // exitOverlayModeToolStripMenuItem
            // 
            this.exitOverlayModeToolStripMenuItem.Name = "exitOverlayModeToolStripMenuItem";
            this.exitOverlayModeToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exitOverlayModeToolStripMenuItem.Text = "Exit Overlay Mode";
            this.exitOverlayModeToolStripMenuItem.Click += new System.EventHandler(this.exitOverlayModeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 492);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "Form1";
            this.Text = "Demoder\'s Planet Map Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.OverlayTitleContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TileDisplay tileDisplay1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox followCharacter;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.HScrollBar tileDisplay1_hScrollBar;
        private System.Windows.Forms.VScrollBar tileDisplay1_vScrollBar;
        private System.Windows.Forms.ComboBox mapComboBox;
        private System.Windows.Forms.ToolStripMenuItem checkVersionToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.RadioButton RadioButtonMapSelectionRubika;
        internal System.Windows.Forms.RadioButton RadioButtonMapSelectionShadowlands;
        internal System.Windows.Forms.RadioButton RadioButtonCameraFollowCharacters;
        internal System.Windows.Forms.RadioButton RadioButtonCameraManual;
        private System.Windows.Forms.Button ButtonZoomOut;
        private System.Windows.Forms.Button ButtonZoomIn;
        private System.Windows.Forms.ToolStripMenuItem readmeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem overlayModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rubikaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem shadowlandsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cameraControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem followCharactersToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem1;
        internal System.Windows.Forms.ContextMenuStrip OverlayTitleContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem selectMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitOverlayModeToolStripMenuItem;
    }
}

