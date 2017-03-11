using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Provides an interface to PushBullet for sending notifications.
	/// </summary>
	public class PushBulletClient {

		// Thanks to xTheDeathlyx for the implementation of this class.

		/// <summary>
		/// Creates a new PushBullet interface using the given settings.
		/// </summary>
		public PushBulletClient(PushBulletDetails Details) {
			this.Details = Details;
		}

		/// <summary>
		/// Sends a push notification to all devices given the current API Key.
		/// </summary>
		public void SendPush(string Title, string Body) {
			var WebReq = (HttpWebRequest)WebRequest.Create("https://api.pushbullet.com/v2/pushes");
			WebReq.ReadWriteTimeout = 100000;
			WebReq.ContentType = "application/x-www-form-urlencoded";
			WebReq.Accept = "application/json";
			WebReq.Method = "POST";
			string AuthString = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Details.ApiKey + ":"));
			WebReq.Headers.Add("Authorization", "Basic " + AuthString);
			using(var Writer = new StreamWriter(WebReq.GetRequestStream())) {
				string Params = string.Format("type={0}&title={1}&body={2}", "note", Uri.EscapeDataString(Title), Uri.EscapeDataString(Body));
				Writer.Write(Params);
				Writer.Flush();
				Writer.Close();
			}
			try {
				HttpWebResponse Resp = (HttpWebResponse)WebReq.GetResponse();
			} catch (WebException ex) {
				if(ex.Status == WebExceptionStatus.ProtocolError) {
					using (var Reader = new StreamReader(ex.Response.GetResponseStream())) {
						string Response = Reader.ReadToEnd();
						throw new ApplicationException("Failed to send Pushbullet notification: " + Response);
					}
				} else {
					throw;
				}
			}
		}

		private PushBulletDetails Details;
	}
}