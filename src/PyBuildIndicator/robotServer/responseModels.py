import os
import re
import urllib
import urllib2
import RPi.GPIO as GPIO

from flask.helpers import json
from robotServer.helperClasses import MyEncoder
from robotServer.models import Choreography, RESOURCES_SOUNDS_, RESOURCES_TEXT_SPEACH_


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
    def Call(self, runner):
        self.Path = RESOURCES_SOUNDS_ + self.FileName
        self.FileFound = os.path.exists(self.Path)
        self.callResponse = 0

        if not self.FileFound:
            self.ErrorMessage = "File could not be found"
        else:
            runner.AddChoreography(Choreography.SimpleChoreographyPlaySound(self.Path))
        if self.callResponse == 1:
            self.ErrorMessage = 'Could not call the process [' + self.Path + ']'
        return self.ReturnJson()


class TextToSpeechResponse(BaseResponse):
    def Call(self, runner):
        runner.AddChoreography(Choreography.SimpleSequencesText2Speech(self.Text))
        return self.ReturnJson()


class SetupGpIoResponse(BaseResponse):
    def Call(self,runnner):
        out = GPIO.OUT if self.direction.upper() == 'out'.upper() else GPIO.IN
        print 'settings pin ' + self.pin + ' to ' + str(self.direction)
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(int(self.pin), out)
        # runner.AddChoreography(Choreography.SimpleSequencesGpIo(pin,self.IsOn))
        return self.ReturnJson()


class GpIoOutputResponse(BaseResponse):
    def Call(self, runner):
        print 'push pin ', self.pin, ' to ', self.IsOn;
        pin = int(self.pin)
        runner.AddChoreography(Choreography.SimpleSequencesGpIo(pin,self.IsOn))
        return self.ReturnJson()


class PassiveResponse(BaseResponse):
    def __init__(self):
        BaseResponse.__init__(self)
        # super(BaseResponse, self).__init__()
        self.Passive = None
    def Call(self):
        return self.ReturnJson()


class EnqueueResponse(BaseResponse):
    def __init__(self):
        BaseResponse.__init__(self)
        self.Choreography = None

    def Call(self, runner):
        if self.Choreography is not None:
            runner.AddChoreography(self.Choreography)
        return self.ReturnJson()

class SetButtonChoreographyResponse(BaseResponse):
    def Call(self):

        return  self.ReturnJson()