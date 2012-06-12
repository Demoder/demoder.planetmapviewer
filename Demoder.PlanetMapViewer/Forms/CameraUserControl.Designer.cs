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
    partial class CameraUserControl
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
            this.zoomInButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.characterButton = new System.Windows.Forms.Button();
            this.activeCharacterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zoomInButton
            // 
            this.zoomInButton.Location = new System.Drawing.Point(109, 3);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(66, 23);
            this.zoomInButton.TabIndex = 1;
            this.zoomInButton.Text = "Zoom In";
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Location = new System.Drawing.Point(109, 32);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(66, 23);
            this.zoomOutButton.TabIndex = 2;
            this.zoomOutButton.Text = "Zoom Out";
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // characterButton
            // 
            this.characterButton.Location = new System.Drawing.Point(3, 3);
            this.characterButton.Name = "characterButton";
            this.characterButton.Size = new System.Drawing.Size(94, 23);
            this.characterButton.TabIndex = 3;
            this.characterButton.Text = "Characters";
            this.characterButton.UseVisualStyleBackColor = true;
            this.characterButton.Click += new System.EventHandler(this.characterButton_Click);
            // 
            // activeCharacterButton
            // 
            this.activeCharacterButton.Location = new System.Drawing.Point(3, 32);
            this.activeCharacterButton.Name = "activeCharacterButton";
            this.activeCharacterButton.Size = new System.Drawing.Size(94, 23);
            this.activeCharacterButton.TabIndex = 4;
            this.activeCharacterButton.Text = "Active Window";
            this.activeCharacterButton.UseVisualStyleBackColor = true;
            this.activeCharacterButton.Click += new System.EventHandler(this.activeCharacterButton_Click);
            // 
            // CameraUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.activeCharacterButton);
            this.Controls.Add(this.characterButton);
            this.Controls.Add(this.zoomOutButton);
            this.Controls.Add(this.zoomInButton);
            this.Name = "CameraUserControl";
            this.Size = new System.Drawing.Size(181, 63);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button zoomInButton;
        private System.Windows.Forms.Button zoomOutButton;
        private System.Windows.Forms.Button characterButton;
        private System.Windows.Forms.Button activeCharacterButton;
    }
}
