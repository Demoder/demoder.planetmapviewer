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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Demoder.PmvInstaller.Tester
{
    public partial class Form1 : Form
    {
        private Process[] blockingProcesses;

        public Form1()
        {
            InitializeComponent();

            Task.Factory.StartNew(this.CheckIncompatibleProcesses, TaskCreationOptions.LongRunning);
        }

        private void CheckIncompatibleProcesses()
        {
            while (true)
            {
                var lst = new List<Process>();
                foreach (var p in Process.GetProcesses())
                {
                    if (p.ProcessName.Equals("client", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lst.Add(p);
                        continue;
                    }

                    if (p.ProcessName.Equals("AnarchyOnline", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lst.Add(p);
                        continue;
                    }

                    if (p.ProcessName.Equals("Demoders PlanetMap Viewer"))
                    {
                        lst.Add(p);
                    }
                }

                if (lst.Count == 0)
                {
                    Program.ReturnCode = ReturnCode.OK;
                    Application.Exit();
                    return;
                }

                this.blockingProcesses = lst.ToArray();
                this.UpdateList();
                Thread.Sleep(250);
            }
        }

        private void UpdateList()
        {
            if (this.listView1.InvokeRequired)
            {
                this.listView1.Invoke((Action)this.UpdateList);
                return;
            }

            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            foreach (var process in this.blockingProcesses)
            {
                this.listView1.Items.Add(new ListViewItem(process.MainWindowTitle));
            }
            this.listView1.EndUpdate();
        }
    }
}
