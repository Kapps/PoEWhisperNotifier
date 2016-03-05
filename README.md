PoEWhisperNotifier
==================
PoEWhisperNotifier is a free and open-source tool that runs in the background to notify you of whispers you get in PoE (optionally) while your game is minimized. You can currently be notified through Windows tray notifications, playing a sound, sending an email, or notifying your phone through PushBullet. The program will also record a history of all missed whispers in case you can't immediately get back to PoE. 

The program works by parsing client.txt as PoE writes to it in realtime. It should be fairly light on resources (does not load all of client.txt and uses very little CPU) and can remain open even while PoE is closed if desired.

Please note that this program never actually interacts with the client and therefore will not get you banned for using it.

Getting Started
==================
To get started, all you need to do is download and run the [compiled version](https://github.com/Kapps/PoEWhisperNotifier/releases/latest). In most cases, your Client.txt path should be automatically detected and you will immediately start receiving notifications.

If your Client.txt path is not detected automatically (has a red background), either run the program after PoE is already running and it should auto-detect it, or click the Log Path textbox, manually locate your Client.txt, then press Start.

The default settings will play a sound and show a Windows notification if you receive a whisper / disconnect while PoE is minimized.
For sending notifications to your phone, using PushBullet is highly recommended. If you do not have a mobile device that supports PushBullet, you may use SMTP to send emails / texts.

Phone Notifications (PushBullet)
==================
1. Go to http://pushbullet.com and sign in with your Google or Facebook account.
2. Go to https://www.pushbullet.com/#settings/account and click Create Access Token
3. Go into the Configure PushBullet settings item in the program and paste in the access token.
4. Make sure that Enable PushBullet Notifications is checked.

Download
==================
You can download the latest version of the program from the [release page](https://github.com/Kapps/PoEWhisperNotifier/releases/latest).
