import os
import urllib
import urllib2
from flask import jsonify
from flask.helpers import json

class BaseResponse:
    def __init__(self, ):
        self.Success = True
        self.ErrorMessage = ""

    def setStatus(self, status, errorMessage=""):
        self.Success = status
        self.ErrorMessage = errorMessage

    def ReturnJson(self):
        self.Success = self.ErrorMessage == "";
        return json.dumps(self, cls=MyEncoder)


class MyEncoder(json.JSONEncoder):
    def default(self, obj):
        if not isinstance(obj, BaseResponse):
            return super(MyEncoder, self).default(obj)
        return obj.__dict__


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

    def Play(self,file):
        callSystem = self.GetMediaPlayer() + ' ' + file + ''
        return os.system(callSystem)

    def Call(self):
        self.Path = 'Resources/mp3/'+self.FileName

        self.FileFound = os.path.exists(self.Path)
        self.callResponse = 0
        if not self.FileFound:
            self.ErrorMessage = "File could not be found"
        else:
            self.callResponse = self.Play(self.Path)
        if self.callResponse == 1:
            self.ErrorMessage = 'Could not call the process [' + self.GetMediaPlayer() + ' ' + self.Path   + ']'
        return self.ReturnJson()


class TextToSpeechResponse(BaseResponse):
    def Call(self):
        url = 'http://translate.google.com/translate_tts?tl=en&q='+urllib.quote_plus(self.text)
        print url
        request = urllib2.Request(url)
        request.add_header('User-Agent', 'Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.8.1.11) Gecko/20071127 Firefox/2.0.0.11')
        opener = urllib2.build_opener()
        self.SaveTo = 'Resources/mp3/textsaved.mp3'
        f = open( self.SaveTo, 'w')
        f.write(opener.open(request).read())
        f.flush()
        f.close()
        player = PlayMp3FileResponse()
        player.Play( self.SaveTo)

        return  self.ReturnJson()