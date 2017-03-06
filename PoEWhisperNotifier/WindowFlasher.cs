using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Helper class used to flash taskbar icons for specific windows.
	/// </summary>
	public static class WindowFlasher {

		// Constants from http://stackoverflow.com/a/11310131.

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);


		/// <summary>
		/// Flashes the window with the given handle in the specified style.
		/// </summary>
		public static void FlashWindow(IntPtr Handle, FlashStyle Style) {
			var fi = new FLASHWINFO() {
				cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
				hwnd = Handle,
				dwFlags = (uint)Style,
				dwTimeout = 0,
				uCount = 1
			};
			FlashWindowEx(ref fi); // Ignore the error codes, as it might just be active.
			//if(!FlashWindowEx(ref fi))
			//throw new InvalidOperationException("Failed to flash PoE window: Error code " + Marshal.GetLastWin32Error());
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO {
			/// <summary>
			/// The size of the structure in bytes.
			/// </summary>
			public uint cbSize;
			/// <summary>
			/// A Handle to the Window to be Flashed. The window can be either opened or minimized.
			/// </summary>
			public IntPtr hwnd;
			/// <summary>
			/// The Flash Status.
			/// </summary>
			public uint dwFlags;
			/// <summary>
			/// The number of times to Flash the window.
			/// </summary>
			public uint uCount;
			/// <summary>
			/// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
			/// </summary>
			public uint dwTimeout;
		}
	}

	/// <summary>
	/// Indicates how to flash a specific window.
	/// </summary>
	[Flags]
	public enum FlashStyle {
		/// <summary>
		/// Stop flashing. The system restores the window to its original stae.
		/// </summary>
		Stop = 0,
		/// <summary>
		/// Flash the window caption.
		/// </summary>
		Caption = 1,
		/// <summary>
		/// Flash the taskbar button.
		/// </summary>
		Tray = 2,
		/// <summary>
		/// Flash both the window caption and taskbar button.
		/// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
		/// </summary>
		All = 3,
		/// <summary>
		/// Flash continuously, until the FLASHW_STOP flag is set.
		/// </summary>
		Constant = 4
	}
}
