
sox translate_tts.mp3 -r 32000 foreground.mp3  speed 0.75
sox -v 0.2 Star-Wars-1391.mp3 -r 32000 background.mp3
sox -m background.mp3 foreground.mp3 test.mp3

start test.mp3

rem sox Star-Wars-1391.mp3  -r 32000 background.mp3
rem sox translate_tts.mp3  -r 32000 foreground.mp3
rem remsox -v 1.880 background.mp3 background.mp3
rem sox -v 2.880 foreground.mp3 foreground.mp3
rem sox -m background.mp3 foreground.mp3 test.mp3

