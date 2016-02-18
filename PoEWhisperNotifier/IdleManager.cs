using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Manages actions and calculations for a user's idle status.
	/// A user is considered idle if they have not entered any input (in any application) for over a certain period of time.
	/// </summary>
	public static class IdleManager {

		[StructLayout(LayoutKind.Sequential)]
		private struct LASTINPUTINFO {
			public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

			[MarshalAs(UnmanagedType.U4)]
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.U4)]
			public UInt32 dwTime;
		}

		[DllImport("user32.dll")]
		private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		private static readonly TimeSpan MinIdleDelay = TimeSpan.FromMinutes(2.0);
		//private static readonly TimeSpan MinIdleDelay = TimeSpan.FromSeconds(5.0);

		// TODO: Can merge the below two with LogMonitor.

		/// <summary>
		/// Begins monitoring for any idle activity.
		/// </summary>
		public static void BeginMonitoring() {
			if (IsMonitoring)
				throw new InvalidOperationException("The IdleManager is already monitoring.");
			IsMonitoring = true;
			IdleThread = new Thread(RunIdleLoop);
			IdleThread.IsBackground = true;
			IdleThread.Start();
		}

		/// <summary>
		/// Stops monitoring for idle activity.
		/// </summary>
		public static void StopMonitoring() {
			if (!IsMonitoring)
				throw new InvalidOperationException("The IdleManager is not monitoring.");
			IsMonitoring = false;
			IdleThread.Join();
		}

		/// <summary>
		/// Indicates the time that the user last entered any input on.
		/// This method may not be completely accurate, but is usually accurate to a 1 millisecond interval.
		/// The result may also change between calls by a slight amount (1 MS usually) due to the estimation required.
		/// </summary>
		public static DateTime LastInputTime {
			get {
				// TODO: Consider making more accurate, may not matter though.
				var LastInput = new LASTINPUTINFO() {
					cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)),
					dwTime = 0
				};
				GetLastInputInfo(ref LastInput);
				// Have to make sure dwTime is same type as TickCount - otherwise signed vs unsigned, which causes issues when TickCount wraps around at 25 days.
				var IdleTime = unchecked(Environment.TickCount - (int)LastInput.dwTime);
				return DateTime.Now - TimeSpan.FromMilliseconds(IdleTime);
			}
		}

		/// <summary>
		/// Indicates if the user is currently considered idle.
		/// </summary>
		public static bool IsUserIdle {
			get {
				var IdleTime = DateTime.Now - LastInputTime;
				// Less than 0 to account for rollover.
				return IdleTime < TimeSpan.Zero || IdleTime > MinIdleDelay;
			}
		}

		/// <summary>
		/// Calls the given action if the user does not enter any input (hence acknowledging the action)
		/// before being considered idle. For example, a user may wish to only be notified of PushBullet
		/// notifications when idle, however after leaving the computer they may receive a message prior 
		/// to being considered idle, which would cause them to miss it. This method would invoke that 
		/// notification if they go idle before acknowledging it (by providing any user input anywhere).
		/// If the user is currently idle, the action is invoked immediately.
		/// </summary>
		public static void AddIdleAction(Action Act) {
			lock (IdleMutex) {
				// No need to be in the lock in this case, but not being would make race conditions harder to find.
				if (IsUserIdle)
					Act();
				else
					IdleActions.Add(Act);
			}
		}

		private static void RunIdleLoop() {
			DateTime PrevInputTime = LastInputTime;
			while (IsMonitoring) {
				var CurrInputTime = LastInputTime;
				// Allow a little bit of leeway, our time function isn't 100% accurate since based off current tick which changes as it runs.
				// Only should make a 1MS difference at most, but just to be safe...
				bool HasNewInput = Math.Abs((CurrInputTime - PrevInputTime).TotalMilliseconds) > 100;
				lock (IdleMutex) {
					if (HasNewInput)
						IdleActions.Clear();
					else if (IsUserIdle) {
						IdleActions.ForEach(c => c());
						IdleActions.Clear();
					}
				}
				PrevInputTime = CurrInputTime;
				Thread.Sleep(250);
			}
		}

		private static Mutex IdleMutex = new Mutex();
		private static List<Action> IdleActions = new List<Action>();
		private static bool IsMonitoring = false;
		private static Thread IdleThread;
	}
}
