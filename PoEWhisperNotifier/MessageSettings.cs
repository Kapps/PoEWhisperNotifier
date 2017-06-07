using Newtonsoft.Json;
using PoEWhisperNotifier.Converters;
using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Stores setting information about a specific type of message, such as a whisper or chat message.
	/// </summary>
	public class MessageSettings {
		
		/// <summary>
		/// Indicates the type of message these settings apply to.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public LogMessageType MessageType { get; private set; }

		/// <summary>
		/// Gets or sets messages to exclude if these phrases are in them.
		/// </summary>
		[TypeConverter(typeof(StringPipeConverter))]
		[Description("A pipe-separated set of strings to exclude a message if one or more of these matches.")]
		public string[] ExcludeFilter { get; set; }

		/// <summary>
		/// Gets or sets a filter to include messages only if they match one of these filters.
		/// </summary>
		[TypeConverter(typeof(StringPipeConverter))]
		[Description("A pipe-separated set of strings to include messages only if one or more of these matches.")]
		public string[] IncludeFilter { get; set; }

		/// <summary>
		/// The color that this type of message should be displayed as.
		/// </summary>
		[Description("The color to display these messages as in the logs.")]
		public Color ForegroundColor { get; set; }

		/// <summary>
		/// Indicates whether to play a notification for this message (using the default notification settings).
		/// </summary>
		[Description("Whether to play a notification (using default settings) for these messages.")]
		public bool PlayNotification { get; set; }

		/// <summary>
		/// Indicates whether this message should be logged.
		/// </summary>
		[Description("Whether to log these messages to the main window.")]
		public bool LogMessage { get; set; }
		
		/// <summary>
		/// Creates a new instance of this class with the default settings.
		/// </summary>
		[JsonConstructor]
		public MessageSettings(LogMessageType MessageType) {
			this.MessageType = MessageType;
			this.ExcludeFilter = new string[0];
			this.IncludeFilter = new string[0];
			this.ForegroundColor = Color.White;
			this.PlayNotification = true;
			this.LogMessage = true;
		}

		/// <summary>
		/// Loads the MessageSettings that are stored in the settings file.
		/// </summary>
		public static MessageSettings[] LoadFromConfig() {
			try {
				string Json = Settings.Default.MessageConfig;
				if (String.IsNullOrWhiteSpace(Json))
					return CreateDefaults();
				var Result = JsonConvert.DeserializeObject<MessageSettings[]>(Json);
				// Automatically add in new MessageTypes with default settings.
				var MissingMessageTypes = Enum.GetValues(typeof(LogMessageType)).OfType<LogMessageType>().Where(c => !Result.Any(d => d.MessageType == c));
				var Defaults = CreateDefaults();
				foreach(var MissingType in MissingMessageTypes) {
					var Default = Defaults.Single(c => c.MessageType == MissingType);
					Result = Result.Concat(new[] { Default }).ToArray();
				}
				return Result;
			} catch (Exception ex) {
				Debug.WriteLine($"Failed to load message settings: {ex}.");
				return CreateDefaults();
			}
		}

		private static MessageSettings[] CreateDefaults() {
			return new[] {
				new MessageSettings(LogMessageType.Disconnect) {
					PlayNotification = true,
					ForegroundColor = Color.White,
				},
				new MessageSettings(LogMessageType.Global) {
					PlayNotification = false,
					ForegroundColor = Color.FromArgb(224, 0, 0),
					LogMessage = false
				},
				new MessageSettings(LogMessageType.Guild) {
					PlayNotification = false,
					ForegroundColor = Color.FromArgb(144, 144, 144)
				},
				new MessageSettings(LogMessageType.Party) {
					PlayNotification = false,
					ForegroundColor = Color.FromArgb(13, 142, 205)
				},
				new MessageSettings(LogMessageType.Trade) {
					PlayNotification = false,
					ForegroundColor = Color.FromArgb(254, 128, 0),
					LogMessage = false
				},
				new MessageSettings(LogMessageType.Whisper) {
					PlayNotification = true,
					ForegroundColor = Color.FromArgb(156, 98, 214)
				},
				new MessageSettings(LogMessageType.Unknown) {
					PlayNotification = false,
					ForegroundColor = Color.White
				},
				new MessageSettings(LogMessageType.Status) {
					PlayNotification = false,
					ForegroundColor = Color.White
				}
			};
		}

		/// <summary>
		/// Saves the given settings back to the settings file.
		/// </summary>
		public static void SaveToConfig(MessageSettings[] Config) {
			string Json = JsonConvert.SerializeObject(Config);
			Settings.Default.MessageConfig = Json;
			Settings.Default.Save();
		}

		public override string ToString() {
			return this.MessageType.ToString();
		}
	}
}
