using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	static class Program {
		[DllImport("User32.dll")]
		private static extern void SetProcessDPIAware();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			SetProcessDPIAware();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
			using(var LogFile = new StreamWriter("err-" + Guid.NewGuid().ToString() + ".txt")) {
				var Ex = e.ExceptionObject as Exception;
				LogFile.WriteLine(DateTime.Now.ToString() + ": " + (Ex?.ToString() ?? "Unknown Exception!"));
			}
		}
	}
}