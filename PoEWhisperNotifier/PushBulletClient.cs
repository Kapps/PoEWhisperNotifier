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
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.pushbullet.com/v2/pushes");
			httpWebRequest.ReadWriteTimeout = 100000;
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.Accept = "application/json";
			httpWebRequest.Method = "POST";
			string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Details.ApiKey + ":"));
			httpWebRequest.Headers.Add("Authorization", "Basic " + auth);
			using(var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
				string parameters = string.Format("type={0}&title={1}&body={2}", "note", Uri.EscapeDataString(Title), Uri.EscapeDataString(Body));
				streamWriter.Write(parameters);
				streamWriter.Flush();
				streamWriter.Close();
			}
			HttpWebResponse resp = (HttpWebResponse)httpWebRequest.GetResponse();
			string respStr = new StreamReader(resp.GetResponseStream()).ReadToEnd();
			//System.Windows.Forms.MessageBox.Show("Response : " + respStr); // if you want see the output
		}

		private PushBulletDetails Details;
	}
}