using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PoEWhisperNotifier {
	/// <summary>
	/// An immutable struct that provides information about a single PoE message.
	/// </summary>
	public struct MessageData {
		/// <summary>
		/// The date the message was parsed (may not be exactly when it was received).
		/// </summary>
		public readonly DateTime Date;
		/// <summary>
		/// The name of the user who sent the message.
		/// </summary>
		public readonly string Sender;
		/// <summary>
		/// The contents of the message, as entered by the sender.
		/// </summary>
		public readonly string Message;

		public MessageData(DateTime Date, string Sender, string Message) {
			this.Date = Date;
			this.Sender = Sender;
			this.Message = Message;
		}
	}

	/// <summary>
	/// Monitors a log file for changes in the form of appended lines.
	/// </summary>
	public class LogMonitor {
		/// <summary>
		/// Called when a message is received.
		/// This may be invoked on a thread separate from the UI thread.
		/// </summary>
		public event Action<MessageData> MessageReceived;

		/// <summary>
		/// Gets the path to the log file being monitored.
		/// </summary>
		public string LogPath { get; private set; }

		/// <summary>
		/// Indicates if the LogMonitor is currently monitoring LogPath for changes.
		/// </summary>
		public bool IsMonitoring { get; private set; }

		/// <summary>
		/// Indicates whether the given file path points to a valid log file.
		/// </summary>
		public static bool IsValidLogPath(string LogPath) {
			return !String.IsNullOrWhiteSpace(LogPath) && File.Exists(LogPath) && Path.GetExtension(LogPath) == ".txt";
		}

		/// <summary>
		/// Creates a new LogMonitor without immediately starting to monitor changes.
		/// </summary>
		public LogMonitor(string LogPath) {
			if(!IsValidLogPath(LogPath))
				throw new ArgumentException("The log path specified was invalid.");
			this.LogPath = LogPath;
		}

		/// <summary>
		/// Begins monitoring the log file for changes.
		/// </summary>
		public void BeginMonitoring() {
			if(IsMonitoring)
				throw new InvalidOperationException("Already monitoring for changes.");
			this.IsMonitoring = true;
			this._LogStream = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			_LogStream.Seek(0, SeekOrigin.End);
			// Instead of proper async handling, take the lazy way and ReadLine in a new thread.
			new Thread(RunReadLoop) { IsBackground = true }.Start();
		}

		/// <summary>
		/// Stops monitoring the log file for changes.
		/// If any messages are currently being processed, they may still be dispatched.
		/// </summary>
		public void StopMonitoring() {
			if(!IsMonitoring)
				throw new InvalidOperationException("Not currently monitoring.");
			IsMonitoring = false;
			_LogStream.Dispose();
		}

		private void RunReadLoop() {
			var Reader = new StreamReader(_LogStream);
			while(IsMonitoring) {
				if(_LogStream.Length == _LogStream.Position) {
					Thread.Sleep(250);
					continue;
				}
				string Line = Reader.ReadLine();
				MessageData Data;
				if(TryParseLine(Line, out Data)) {
					var MR = MessageReceived;
					if(MR != null)
						MR(Data);
				}
			}
		}

		private bool TryParseLine(string Line, out MessageData Data) {
			Data = default(MessageData);
			try {
				var Match = WhisperRegex.Match(Line);
				if(!Match.Success || Match.Groups.Count != 3)
					return false;
				string Username = Match.Groups[1].Value;
				string Contents = Match.Groups[2].Value;
				if(String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Contents))
					return false;
				Data = new MessageData(DateTime.Now, Username, Contents);
				return true;
			} catch {
				return false;
			}
		}

		// Group 1 = Username, Group 2 = Contents - 1ff = message code for chat.
		private static readonly Regex WhisperRegex = new Regex(@"^.+\ .+\ .+\ 1ff\ \[.+\]\ @(.+):\ (.+)$");
		private FileStream _LogStream;
	}
}
