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

namespace Demoder.PlanetMapViewer.Forms
{
    partial class MapSelectionControl
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
            this.RadioRK = new System.Windows.Forms.RadioButton();
            this.RadioSL = new System.Windows.Forms.RadioButton();
            this.RadioAuto = new System.Windows.Forms.RadioButton();
            this.MapComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // RadioRK
            // 
            this.RadioRK.AutoSize = true;
            this.RadioRK.Location = new System.Drawing.Point(3, 3);
            this.RadioRK.Name = "RadioRK";
            this.RadioRK.Size = new System.Drawing.Size(62, 17);
            this.RadioRK.TabIndex = 0;
            this.RadioRK.TabStop = true;
            this.RadioRK.Text = "Rubi-ka";
            this.RadioRK.UseVisualStyleBackColor = true;
            this.RadioRK.CheckedChanged += new System.EventHandler(this.RadioMapTypeCheckedChanged);
            // 
            // RadioSL
            // 
            this.RadioSL.AutoSize = true;
            this.RadioSL.Location = new System.Drawing.Point(3, 26);
            this.RadioSL.Name = "RadioSL";
            this.RadioSL.Size = new System.Drawing.Size(89, 17);
            this.RadioSL.TabIndex = 1;
            this.RadioSL.TabStop = true;
            this.RadioSL.Text = "Shadowlands";
            this.RadioSL.UseVisualStyleBackColor = true;
            // 
            // RadioAuto
            // 
            this.RadioAuto.AutoSize = true;
            this.RadioAuto.Location = new System.Drawing.Point(3, 49);
            this.RadioAuto.Name = "RadioAuto";
            this.RadioAuto.Size = new System.Drawing.Size(47, 17);
            this.RadioAuto.TabIndex = 2;
            this.RadioAuto.TabStop = true;
            this.RadioAuto.Text = "Auto";
            this.RadioAuto.UseVisualStyleBackColor = true;
            // 
            // MapComboBox
            // 
            this.MapComboBox.FormattingEnabled = true;
            this.MapComboBox.Location = new System.Drawing.Point(3, 72);
            this.MapComboBox.Name = "MapComboBox";
            this.MapComboBox.Size = new System.Drawing.Size(143, 21);
            this.MapComboBox.TabIndex = 3;
            this.MapComboBox.SelectedIndexChanged += new System.EventHandler(this.MapComboBoxSelectedIndexChanged);
            // 
            // MapSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MapComboBox);
            this.Controls.Add(this.RadioAuto);
            this.Controls.Add(this.RadioSL);
            this.Controls.Add(this.RadioRK);
            this.Name = "MapSelectionControl";
            this.Size = new System.Drawing.Size(153, 100);
            this.Load += new System.EventHandler(this.MapSelectionControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RadioButton RadioRK;
        internal System.Windows.Forms.RadioButton RadioSL;
        internal System.Windows.Forms.RadioButton RadioAuto;
        internal System.Windows.Forms.ComboBox MapComboBox;
    }
}
