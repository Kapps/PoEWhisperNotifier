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
		/// This value may be null for some message types.
		/// </summary>
		public readonly string Sender;
		/// <summary>
		/// The contents of the message, as entered by the sender.
		/// </summary>
		public readonly string Message;
		/// <summary>
		/// The type of the message that was received (such as a whisper or disconnect).
		/// </summary>
		public readonly LogMessageType MessageType;

		public MessageData(DateTime Date, string Sender, string Message, LogMessageType MessageType) {
			this.Date = Date;
			this.Sender = Sender;
			this.Message = Message;
			this.MessageType = MessageType;
		}
	}

	/// <summary>
	/// Indicates the type of message contained by a MessageData instance.
	/// </summary>
	public enum LogMessageType {
		/// <summary>
		/// An unexpected message was received.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// A whisper was sent or received.
		/// </summary>
		Whisper = 1,
		/// <summary>
		/// An unexpected disconnect was received.
		/// </summary>
		Disconnect = 2,
		/// <summary>
		/// A party message was sent or received.
		/// </summary>
		Party = 4,
        /// <summary>
        /// A guild message was sent or received.
        /// </summary>
        Guild = 5

    }

	/// <summary>
	/// Monitors a log file for changes in the form of appended lines.
	/// </summary>
	public class LogMonitor {

		private const string DEFAULT_LOG_PATH = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\Client.txt";

		/// <summary>
		/// Called when a message (such as a whisper or disconnect) is received.
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
		/// Attempts to location a default client.txt location, using either the standard installation path or the directory PoE is running frmo.
		/// Returns whether the client.txt was successfully located.
		/// </summary>
		public static bool TryGetDefaultLogPath(out string LogPath) {
			try {
				var PoeProc = Main.GetPoeProcess();
				string ExpectedPath;
				if (PoeProc != null)
					ExpectedPath = Path.Combine(Path.GetDirectoryName(PoeProc.MainModule.FileName), "logs", "Client.txt");
				else
					ExpectedPath = DEFAULT_LOG_PATH;
				if (IsValidLogPath(ExpectedPath)) {
					LogPath = ExpectedPath;
					return true;
				}
			} catch {
				// Ignore any failures as this is purely a convenience function and should not cause issues on failure.
			}
			LogPath = "";
			return false;
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
			_LogThread = new Thread(RunReadLoop) { IsBackground = true };
			_LogThread.Start();
		}

		/// <summary>
		/// Stops monitoring the log file for changes.
		/// If any messages are currently being processed, they may still be dispatched.
		/// </summary>
		public void StopMonitoring() {
			if(!IsMonitoring)
				throw new InvalidOperationException("Not currently monitoring.");
			IsMonitoring = false;
			_LogThread.Join();
			_LogStream.Dispose();
		}

		/// <summary>
		/// Returns the chat symbol that corresponds to messages of the given message type, or an empty string if none does.
		/// </summary>=
		public static string ChatSymbolForMessageType(LogMessageType Type) {
			var KVP = SymbolToMessageType.FirstOrDefault(c => c.Value == Type);
			return KVP.Key == null ? "" : KVP.Key;
		}

		/// <summary>
		/// Returns the message type that corresponds to the given chat symbol, or Unknown if none does.
		/// </summary>
		public static LogMessageType MessageTypeForChatSymbol(string ChatSymbol) {
			LogMessageType Res;
			if (!SymbolToMessageType.TryGetValue(ChatSymbol, out Res))
				Res = LogMessageType.Unknown;
			return Res;
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
				Action<MessageData> Ev = null;
				if(TryParseChat(Line, out Data) || TryParseDisconnect(Line, out Data)) {
					Ev = MessageReceived;
					if (Ev != null)
						Ev(Data);
				}
			}
		}

		private bool TryParseDisconnect(string Line, out MessageData Data) {
			Data = default(MessageData);
			try {
				var Match = DisconnectRegex.Match(Line);
				if (!Match.Success || Match.Groups.Count != 2)
					return false;
				string Reason = Match.Groups[1].Value.Trim();
				Data = new MessageData(DateTime.Now, null, "Abnormal disconnection: " + Reason, LogMessageType.Disconnect);
				return true;
			} catch {
				return false;
			}
		}

		private bool TryParseChat(string Line, out MessageData Data) {
			Data = default(MessageData);
			try {
				var Match = ChatRegex.Match(Line);
				if(!Match.Success || Match.Groups.Count != 4)
					return false;
				string ChatSymbol = Match.Groups[1].Value.FirstOrDefault().ToString();
				string Username = Match.Groups[2].Value;
				string Contents = Match.Groups[3].Value;
				if(String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Contents))
					return false;
				Username = Username.Trim();
				var MessageType = MessageTypeForChatSymbol(ChatSymbol);
				Data = new MessageData(DateTime.Now, Username, Contents, MessageType);
				return true;
			} catch {
				return false;
			}
		}

		// Group 1 = Chat Type, Group 2 = Username, Group 3 = Contents
		private static readonly Regex ChatRegex = new Regex(@"^.+\ .+\ .+\ .+\ \[.+\]\ (%|@From|@От кого|@De|&)(.+):\ (.+)$");
		private static readonly Regex DisconnectRegex = new Regex(@"^.+\ .+\ .+\ .+\ \[.+\]\ Abnormal disconnect:(.+)");
		private static readonly Dictionary<string, LogMessageType> SymbolToMessageType = new Dictionary<string, LogMessageType>() { { "%", LogMessageType.Party }, { "@", LogMessageType.Whisper }, { "&", LogMessageType.Guild } };

		private FileStream _LogStream;
		private Thread _LogThread;
	}
}
