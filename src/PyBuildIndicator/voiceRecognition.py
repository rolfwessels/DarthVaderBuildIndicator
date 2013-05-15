import json
import os
import subprocess
import urllib2
from helperClasses import MediaPlayer
from twitterListner import TwitterListener

GOOGLE_SR_URL = 'http://www.google.com/speech-api/v1/recognize?lang=en-us&client=chromium'

class VoiceRecognition(object):
    def __init__(self):
        self.ThreshHold = 3.0
        pass

    def AudioFileToText(self):
        result = ""
        try:
            req = urllib2.Request(GOOGLE_SR_URL, data=open("/tmp/noise.flac", 'r').read())
            req.add_header('User-Agent', 'Mozilla/5.0')
            req.add_header('Content-Type', 'audio/x-flac; rate=16000')
            res = urllib2.urlopen(req)
            result = json.load(res)

            # Google returns a JSON object that has a list of hypotheses about what
            # the transcription should be. I just take the first one. This code will
            # break if something goes wrong in opening the URL etc.
            result = result['hypotheses'][0]['utterance']
        except :
            result = "No match"
        finally:
            return result


    def recordClip(self):
        recordTime = 2
        while True:
            os.system("arecord -D plughw:1,0 -f cd -t wav -d "+str(recordTime)+" -r 16000 /tmp/noise.wav 1>/dev/null 2>/dev/null")
            maxVolume = subprocess.check_output(
                "sox /tmp/noise.wav -n stats -s 16 2>&1 | awk '/^Max\\ level/ {print $3}'",
                shell=True)

            if float(maxVolume) > self.ThreshHold:
                #convert to mp3
                print "Convert to mp3 [" + str(maxVolume) + "] [" + str(self.ThreshHold) + "]"
                os.system("flac /tmp/noise.wav -f --best --sample-rate 16000 -o /tmp/noise.flac 1>/dev/null 2>/dev/null")

                text = self.AudioFileToText()
                if "darth vader" in text.lower() or "talk later" in text.lower():
                    player = MediaPlayer()
                    player.Play("resources/sounds/Start/darthvader_yesmaster.wav")
                    recordTime = 5
                else:
                    print "Voice text detected text [" + text + "]"
                    yield text
                    recordTime = 2
            pass

    def processClips(self, compo=None):
        for cl in self.recordClip():
            listener = TwitterListener()
            listener.processText(compo,cl)
            yield cl
