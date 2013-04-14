import os
import re
import urllib
import urllib2
import RPi.GPIO as GPIO

from flask.helpers import json
from robotServer.helperClasses import MyEncoder


class BaseResponse:
    def __init__(self):
        self.Success = True
        self.ErrorMessage = ""

    def setStatus(self, status, errorMessage=""):
        self.Success = status
        self.ErrorMessage = errorMessage

    def ReturnJson(self):
        self.Success = self.ErrorMessage == "";
        return json.dumps(self, cls=MyEncoder)


class PingResponse(BaseResponse):
    def Call(self):
        self.setStatus(True)
        return self.ReturnJson()


class PlayMp3FileResponse(BaseResponse):
    def GetMediaPlayer(self):
        if os.path.exists("/usr/bin/mpg321"):
            return "/usr/bin/mpg321"
        else:
            return 'D:\\Work\\Projects\\Home\\BuildIndicatron\\var\\cmdmp3\cmdmp3.exe'

    def Play(self, file):
        callSystem = self.GetMediaPlayer() + ' ' + file + ''
        return os.system(callSystem)

    def Call(self):
        self.Path = 'Resources/mp3/' + self.FileName

        self.FileFound = os.path.exists(self.Path)
        self.callResponse = 0
        if not self.FileFound:
            self.ErrorMessage = "File could not be found"
        else:
            self.callResponse = self.Play(self.Path)
        if self.callResponse == 1:
            self.ErrorMessage = 'Could not call the process [' + self.GetMediaPlayer() + ' ' + self.Path + ']'
        return self.ReturnJson()


class TextToSpeechResponse(BaseResponse):
    def ConvertToFileName(self, text):
        return re.sub('[^A-z0-9]', '_', text);

    def Download(self, url, toFile=None):
        request = urllib2.Request(url)
        request.add_header('User-Agent',
                           'Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.8.1.11) Gecko/20071127 Firefox/2.0.0.11')
        opener = urllib2.build_opener()
        f = open(toFile, 'w')
        f.write(opener.open(request).read())
        f.flush()
        f.close()

    def Call(self):
        url = 'http://translate.google.com/translate_tts?tl=en&q=' + urllib.quote_plus(self.text)
        self.SaveTo = 'Resources/mp3/' + self.ConvertToFileName(self.text) + '.mp3'
        if not os.path.exists(self.SaveTo):
            print "downloading file " + self.SaveTo
            self.Download(url, self.SaveTo)

        player = PlayMp3FileResponse()
        player.Play(self.SaveTo)

        return self.ReturnJson()


class SetupGpIoResponse(BaseResponse):
    def Call(self):
        out = GPIO.OUT if self.direction.upper() == 'out'.upper() else GPIO.IN
        print 'settings pin ' + self.pin + ' to ' + str(self.direction)
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(int(self.pin), out)
        return self.ReturnJson()


class GpIoOutputResponse(BaseResponse):
    def Call(self):
        print 'push pin ', self.pin, ' to ' , self.IsOn;
        GPIO.output(int(self.pin), self.IsOn)
        return self.ReturnJson()

class PassiveResponse(BaseResponse):
    def __init__(self):
        BaseResponse.__init__(self)
        # super(BaseResponse, self).__init__()
        self.Passive = None
    def Call(self):
        return self.ReturnJson()

