PoEWhisperNotifier
==================
A simple C# application to notify you of whispers you get in PoE (optionally) while your game is minimized through either Windows tray notifications, playing a sound, sending an email, or notifying your phone through PushBullet. It will also record a history of all missed whispers in case you can't immediately get back to it PoE.

The program works by parsing client.txt as PoE writes to it in realtime. It should also be fairly light on resources (does not load all of client.txt and uses very little CPU) and can remain open even while PoE is closed if desired.

Getting Started
==================
1. Download the compiled version using the link at the bottom and launch the program.
2. Make sure the Log Path selected is valid (the default path is fine if you did not change where PoE is installed to).
3. Press Start (you may wish to enable Auto-Start in settings to skip this step in the future).

The default settings will play a sound and show a Windows notification if you receive a whisper while PoE is minimized.
For sending notifications to your phone, using PushBullet is highly recommended over SMTP.

Phone Notifications (PushBullet)
==================
1. Go to http://pushbullet.com and sign in with your Google account.
2. Go to http://pushbullet.com/account and copy that access token.
3. Go into the Configure PushBullet settings item in the program and paste in the access token.
4. Make sure Enable PushBullet Notifications is checked.

Download
==================
Direct download link for the latest compiled version: https://github.com/Kapps/PoEWhisperNotifier/releases/download/v1.4/v1.4b.PoeWhisperNotifier.zip
