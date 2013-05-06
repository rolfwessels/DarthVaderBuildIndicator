from RPi import GPIO
from flask import json
import hashlib
import md5
import os
from pprint import pprint
from random import choice
import re
import urllib
import urllib2
from helperClasses import MediaPlayer

TEXT_SPEECH = "Text2Speech"
TEXT_ONELINER = "oneliner"
PLAYSOUND = "playsound"
GP_IO = 'GpIO'

RESOURCES_TEXT_SPEACH_ = 'Resources/text2speach/'
RESOURCES_SOUNDS_ = 'Resources/sounds/'


class Sequences(object):
    def __init__(self, typeName):
        self.__type = typeName
        self.Done = False
        self.BeginTime = 0

    def get_Type(self):
        return self.__type

    Type = property(fget=get_Type)

    def Execute(self, timedDifference):
        if timedDifference > self.BeginTime and not self.Done:
            self.ExecuteFirstInstance()
            self.Done = True
        pass

    def ExecuteFirstInstance(self):
        pass


class SequencesPlaySound(Sequences):
    def __init__(self, fileName=""):
        super(SequencesPlaySound, self).__init__(PLAYSOUND)
        self.File = fileName

    def ExecuteFirstInstance(self):
        print "play the file ", self.File
        soundFile = RESOURCES_SOUNDS_ + self.File
        if os.path.isdir(soundFile):
            soundFile = soundFile + "/" + choice(os.listdir(soundFile))
            print soundFile
            MediaPlayer().Play(soundFile)
        elif os.path.exists(soundFile):
            MediaPlayer().Play(soundFile)
        super(SequencesPlaySound, self).ExecuteFirstInstance()


class SequencesGpIo(Sequences):
    def __init__(self, pin, isOn):
        super(SequencesGpIo, self).__init__(GP_IO)
        self.Pin = pin
        self.IsOn = isOn

    def ExecuteFirstInstance(self):
        print "set pin", self.Pin, " to ", self.IsOn
        GPIO.output(self.Pin, self.IsOn)
        super(SequencesGpIo, self).ExecuteFirstInstance()


class SequencesText2Speech(Sequences):
    def __init__(self, text="", disableTransform=False):
        super(SequencesText2Speech, self).__init__(TEXT_SPEECH)
        self.Text = text
        self.DisableTransform = disableTransform

    def ConvertToFileName(self, text):
        hash = hashlib.md5(text).hexdigest()
        print hash
        print re.sub('[^A-z0-9]', '_', text)[:30]
        code = re.sub('[^A-z0-9]', '_', text)[:30] + '_' + str(hash)[:5] + '_' + str(self.DisableTransform)

        return code

    def Download(self, url, toFile=None):
        request = urllib2.Request(url)
        request.add_header('User-Agent',
                           'Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.8.1.11) Gecko/20071127 Firefox/2.0.0.11')
        opener = urllib2.build_opener()
        f = open(toFile, 'w')
        f.write(opener.open(request).read())
        f.flush()
        f.close()

    def ExecuteFirstInstance(self):
        strings = self.SplitString(self.Text)
        print "reading"
        print strings
        for s in strings:
            self.PlayPartText(s)
        super(SequencesText2Speech, self).ExecuteFirstInstance()

    def PlayPartText(self, input):
        self.SaveTo = RESOURCES_TEXT_SPEACH_ + self.ConvertToFileName(input) + '.mp3'
        if not os.path.exists(self.SaveTo):
            url = 'http://translate.google.com/translate_tts?tl=en&q=' + urllib.quote_plus(input)
            print "downloading file " + url + " " + self.SaveTo
            self.Download(url, self.SaveTo)
            if not self.DisableTransform:
                self.Transform(self.SaveTo)
        os.system("play " + self.SaveTo + " echo 0.8 0.88 6.0 0.4")


    def SplitString(self, string):
        strings = []
        sentences = string.split('.')
        for sentence in sentences:
            if len(sentence) < 80:
                strings.append(sentence)
            else:
                st = ""
                for i, word in enumerate(sentence.split(), 1):
                    if len(st) < 80:
                        st += word + " "
                    else:
                        strings.append(st)
                        st = ""
                strings.append(st)
        return strings

    def Transform(self, SaveTo):
        path = "Resources/text2speach/"
        tmpF = path + "foreground.mp3"
        tmpB = path + "background.mp3"
        background = path + "Star-Wars-1391.mp3"

        os.system("sox -v 7 " + SaveTo + " -r 32000 " + tmpF + "  speed 0.75")
        os.system("sox -v 0.2 " + background + " -r 32000 " + tmpB)
        os.system("sox -m " + tmpB + "  " + tmpF + " " + SaveTo)


class SequencesJokeOrOneLiner(SequencesText2Speech):
    def __init__(self):
        super(SequencesJokeOrOneLiner, self).__init__("")
        self.__type = TEXT_ONELINER
        json_data = open('Resources/jokes.json')
        self.Jokes = json.load(json_data)
        json_data.close()
        pass

    def ExecuteFirstInstance(self):
        self.Text = self.GetOneLiner()
        super(SequencesJokeOrOneLiner, self).ExecuteFirstInstance()

    def GetOneLiner(self):
        return choice(self.Jokes)
        pass


class Passive(object):
    def __init__(self, params=None):
        self.Initialize()
        if params is not None:
            self.__dict__.update(params)
            self.Compositions = []
            for v in params["Compositions"]:
                self.Compositions.append(Choreography(v))

    def Initialize(self):
        self.Interval = 5000
        self.StartTime = "06:00"
        self.SleepTime = "19:00"
        self.LastRun = None
        self.Compositions = []


class Choreography(object):
    def __init__(self, params=None):
        self.__sequences = []
        if params is not None:
            self.__dict__.update(params)
            self.__sequences = []
            for v in params["Sequences"]:
                sequences = Sequences(v['Type'])
                if v['Type'] == PLAYSOUND:
                    sequences = SequencesPlaySound(v['File'])
                elif v['Type'] == GP_IO:
                    sequences = SequencesGpIo(v['Pin'], v['IsOn'])
                elif v['Type'] == TEXT_SPEECH:
                    sequences = SequencesText2Speech(v['Text'], v['DisableTransform'])
                elif v['Type'] == TEXT_ONELINER:
                    sequences = SequencesJokeOrOneLiner()

                sequences.BeginTime = v['BeginTime']
                self.__sequences.append(sequences)

    def get_Sequences(self):
        return self.__sequences

    def set_Sequences(self, value):
        self.__sequences = value

    Sequences = property(fget=get_Sequences, fset=set_Sequences)

    @staticmethod
    def SimpleChoreographyPlaySound(fileName):
        choreography = Choreography()
        choreography.Sequences.append(SequencesPlaySound(fileName))
        return choreography

    @staticmethod
    def SimpleSequencesText2Speech(text):
        choreography = Choreography()
        choreography.Sequences.append(SequencesText2Speech(text))
        return choreography

    @staticmethod
    def SimpleSequencesGpIo(pin, state):
        choreography = Choreography()
        choreography.Sequences.append(SequencesGpIo(pin, state))
        return choreography
