/*
* Demoder.PlanetMapViewer
* Copyright (C) 2012, 2013 Demoder (demoder@demoder.me)
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

namespace Demoder.PlanetMapViewer.Forms
{
    partial class OptionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AoPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ButtonOkay = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FPS = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.overlayModeWorkaroundTopmost = new System.Windows.Forms.CheckBox();
            this.overlayModeShowExitButton = new System.Windows.Forms.CheckBox();
            this.overlayModeShowScrollbars = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.disableTutorials = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.fontPage = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.selectedFont = new System.Windows.Forms.ComboBox();
            this.textTypes = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FPS)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.fontPage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.AoPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Anarchy Online";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AoPath
            // 
            this.AoPath.Location = new System.Drawing.Point(76, 16);
            this.AoPath.Name = "AoPath";
            this.AoPath.Size = new System.Drawing.Size(223, 20);
            this.AoPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AODirectory";
            // 
            // ButtonOkay
            // 
            this.ButtonOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonOkay.Location = new System.Drawing.Point(123, 0);
            this.ButtonOkay.Name = "ButtonOkay";
            this.ButtonOkay.Size = new System.Drawing.Size(63, 22);
            this.ButtonOkay.TabIndex = 1;
            this.ButtonOkay.Text = "Okay";
            this.ButtonOkay.UseVisualStyleBackColor = true;
            this.ButtonOkay.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(214, 0);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(63, 22);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.FPS);
            this.groupBox2.Location = new System.Drawing.Point(6, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(119, 79);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Performance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "FPS";
            // 
            // FPS
            // 
            this.FPS.Location = new System.Drawing.Point(68, 13);
            this.FPS.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.FPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(44, 20);
            this.FPS.TabIndex = 0;
            this.FPS.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.overlayModeWorkaroundTopmost);
            this.groupBox3.Controls.Add(this.overlayModeShowExitButton);
            this.groupBox3.Controls.Add(this.overlayModeShowScrollbars);
            this.groupBox3.Location = new System.Drawing.Point(127, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(133, 79);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Overlay Mode";
            // 
            // overlayModeWorkaroundTopmost
            // 
            this.overlayModeWorkaroundTopmost.AutoSize = true;
            this.overlayModeWorkaroundTopmost.Location = new System.Drawing.Point(6, 59);
            this.overlayModeWorkaroundTopmost.Name = "overlayModeWorkaroundTopmost";
            this.overlayModeWorkaroundTopmost.Size = new System.Drawing.Size(123, 17);
            this.overlayModeWorkaroundTopmost.TabIndex = 2;
            this.overlayModeWorkaroundTopmost.Text = "Force on top (bugfix)";
            this.overlayModeWorkaroundTopmost.UseVisualStyleBackColor = true;
            // 
            // overlayModeShowExitButton
            // 
            this.overlayModeShowExitButton.AutoSize = true;
            this.overlayModeShowExitButton.Location = new System.Drawing.Point(6, 37);
            this.overlayModeShowExitButton.Name = "overlayModeShowExitButton";
            this.overlayModeShowExitButton.Size = new System.Drawing.Size(107, 17);
            this.overlayModeShowExitButton.TabIndex = 1;
            this.overlayModeShowExitButton.Text = "Show Exit Button";
            this.overlayModeShowExitButton.UseVisualStyleBackColor = true;
            // 
            // overlayModeShowScrollbars
            // 
            this.overlayModeShowScrollbars.AutoSize = true;
            this.overlayModeShowScrollbars.Location = new System.Drawing.Point(6, 15);
            this.overlayModeShowScrollbars.Name = "overlayModeShowScrollbars";
            this.overlayModeShowScrollbars.Size = new System.Drawing.Size(102, 17);
            this.overlayModeShowScrollbars.TabIndex = 0;
            this.overlayModeShowScrollbars.Text = "Show Scrollbars";
            this.overlayModeShowScrollbars.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.disableTutorials);
            this.groupBox4.Location = new System.Drawing.Point(266, 58);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(119, 79);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General";
            // 
            // disableTutorials
            // 
            this.disableTutorials.AutoSize = true;
            this.disableTutorials.Location = new System.Drawing.Point(6, 16);
            this.disableTutorials.Name = "disableTutorials";
            this.disableTutorials.Size = new System.Drawing.Size(104, 17);
            this.disableTutorials.TabIndex = 0;
            this.disableTutorials.Text = "Disable Tutorials";
            this.disableTutorials.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalPage);
            this.tabControl1.Controls.Add(this.fontPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(401, 168);
            this.tabControl1.TabIndex = 6;
            // 
            // generalPage
            // 
            this.generalPage.AutoScroll = true;
            this.generalPage.Controls.Add(this.groupBox2);
            this.generalPage.Controls.Add(this.groupBox3);
            this.generalPage.Controls.Add(this.groupBox4);
            this.generalPage.Controls.Add(this.groupBox1);
            this.generalPage.Location = new System.Drawing.Point(4, 22);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(393, 142);
            this.generalPage.TabIndex = 0;
            this.generalPage.Text = "General";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // fontPage
            // 
            this.fontPage.AutoScroll = true;
            this.fontPage.Controls.Add(this.groupBox5);
            this.fontPage.Location = new System.Drawing.Point(4, 22);
            this.fontPage.Name = "fontPage";
            this.fontPage.Padding = new System.Windows.Forms.Padding(3);
            this.fontPage.Size = new System.Drawing.Size(393, 142);
            this.fontPage.TabIndex = 1;
            this.fontPage.Text = "Fonts";
            this.fontPage.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.selectedFont);
            this.groupBox5.Controls.Add(this.textTypes);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(176, 130);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Text Type";
            // 
            // selectedFont
            // 
            this.selectedFont.FormattingEnabled = true;
            this.selectedFont.Location = new System.Drawing.Point(6, 103);
            this.selectedFont.Name = "selectedFont";
            this.selectedFont.Size = new System.Drawing.Size(155, 21);
            this.selectedFont.TabIndex = 2;
            this.selectedFont.SelectedValueChanged += new System.EventHandler(this.selectedFont_SelectedValueChanged);
            // 
            // textTypes
            // 
            this.textTypes.Location = new System.Drawing.Point(6, 19);
            this.textTypes.Name = "textTypes";
            this.textTypes.Size = new System.Drawing.Size(155, 82);
            this.textTypes.Sorted = true;
            this.textTypes.TabIndex = 0;
            this.textTypes.SelectedValueChanged += new System.EventHandler(this.textTypes_SelectedValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ButtonOkay);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonCancel);
            this.splitContainer1.Size = new System.Drawing.Size(401, 197);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 7;
            // 
            // OptionWindow
            // 
            this.AcceptButton = this.ButtonOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(401, 197);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(417, 231);
            this.Name = "OptionWindow";
            this.Text = "Options";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FPS)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.fontPage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        internal System.Windows.Forms.TextBox AoPath;
        private System.Windows.Forms.Button ButtonOkay;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown FPS;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox overlayModeShowScrollbars;
        private System.Windows.Forms.CheckBox overlayModeShowExitButton;
        private System.Windows.Forms.CheckBox overlayModeWorkaroundTopmost;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox disableTutorials;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TabPage fontPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox textTypes;
        private System.Windows.Forms.ComboBox selectedFont;
    }
}