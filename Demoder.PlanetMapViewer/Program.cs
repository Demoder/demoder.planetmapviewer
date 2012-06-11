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
using System.Linq;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.Forms;
using System.IO;

namespace Demoder.PlanetMapViewer
{
    static class Program
    {
        static bool debug = false;
        static StreamWriter writer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && args[0]=="--debug")
            {
                debug=true;
            }
            if (debug)
            {
                var stream = File.Open("stdout.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                Console.SetOut(writer);
            }

            WriteLog("");
            WriteLog("Starting application!");
            WriteLog("");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            try
            {
                Application.Run(new MainWindow());
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }

            if (writer != null)
            {
                writer.Flush();
                writer.Dispose();
                writer = null;
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Dispose();
                writer = null;
            }
        }


        internal static void WriteLog(string format, params object[] parameters)
        {
            if (!debug) { return; }

            if (writer == null) { return; }
            var log = String.Format(format, parameters);
            lock (writer)
            {
                writer.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToShortTimeString(), log));
            }
        }

        internal static void WriteLog(Exception ex)
        {
            WriteLog("");
            WriteLog(ex.ToString());
            WriteLog("");
        }

    }
}
