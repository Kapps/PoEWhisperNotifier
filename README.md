PoEWhisperNotifier
==================
A simple C# application to notify you of whispers you get in PoE (optionally) while your game is minimized.
Other features include storing a history of the whispers received and Windows tray notifications.

The program works by parsing client.txt as PoE writes to it in realtime. It should also be fairly light on resources (does not load all of client.txt and uses little CPU) and can remain open even while PoE is closed if desire.

For sending notifications to your phone, using PushBullet is highly recommended over SMTP. Simple go to http://pushbullet.com, sign in with your Google account, go to http://pushbullet.com/account and copy that access token into the Configure PushBullet setting in the program (and enable PushBullet notifications in settings).

Direct download link for the compiled version: https://github.com/Kapps/PoEWhisperNotifier/releases/download/v1.3/PoEWhisperNotifier.zip
