from RPi import GPIO
from flask import json
import hashlib
import os
from random import choice
import re
import urllib
import urllib2
from helperClasses import MediaPlayer
from twitterComs import TwitterCommunication


TEXT_SPEECH = "Text2Speech"
TEXT_ONELINER = "oneliner"
PLAYSOUND = "playsound"
GP_IO = 'GpIO'
TWEETING = "Tweet"
QUOTES = "Quotes"
RESOURCES_TEXT_SPEACH_ = 'resources/text2speach/'
RESOURCES_SOUNDS_ = 'resources/sounds/'


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
        self.SendTweet = False
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

        if  self.SendTweet and len(self.Text) > 0:
            twitter = TwitterCommunication()
            twitter.SendTweet(self.Text)

        super(SequencesText2Speech, self).ExecuteFirstInstance()

    def PlayPartText(self, input):
        if len(input) > 0:
            self.SaveTo = RESOURCES_TEXT_SPEACH_ + self.ConvertToFileName(input) + '.mp3'
            if not os.path.exists(self.SaveTo):
                url = 'http://translate.google.com/translate_tts?tl=en&q=' + urllib.quote_plus(input)
                print "downloading file " + url + " " + self.SaveTo
                self.Download(url, self.SaveTo)

            if not self.DisableTransform:
                os.system("play -m resources/text2speach/background.mp3  -v 5 " + self.SaveTo + " speed 0.78 echo 0.8 0.88 6.0 0.4")
            else:
                os.system("play -v 5 " + self.SaveTo )



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
        path = "resources/text2speach/"
        tmpF = path + "foreground.mp3"
        tmpB = path + "background.mp3"
        background = path + "Star-Wars-1391.mp3"

        os.system("sox -v 7 " + SaveTo + " -r 32000 " + tmpF + "  speed 0.75")
        os.system("sox -v 0.2 " + background + " -r 32000 " + tmpB)
        os.system("sox -m " + tmpB + "  " + tmpF + " " + SaveTo)

class SequencesTweet(Sequences):
    def __init__(self, text=""):
        super(SequencesTweet, self).__init__(TWEETING)
        self.Text = text
        pass

    def ExecuteFirstInstance(self):
        if len(self.Text) > 0:
            twitter = TwitterCommunication()
            twitter.SendTweet(self.Text)
        super(SequencesTweet, self).ExecuteFirstInstance()

class SequencesJokeOrOneLiner(SequencesText2Speech):
    def __init__(self):
        super(SequencesJokeOrOneLiner, self).__init__("")
        self.SendTweet = True
        self.__type = TEXT_ONELINER

    def ExecuteFirstInstance(self):
        self.Text = self.GetOneLiner()
        super(SequencesJokeOrOneLiner, self).ExecuteFirstInstance()

    def GetOneLiner(self):
        json_data = open('resources/jokes.json')
        Jokes = json.load(json_data)
        json_data.close()
        return choice(Jokes)
        pass

class SequencesQuotes(SequencesText2Speech):
    def __init__(self):
        super(SequencesQuotes, self).__init__("")
        self.__type = QUOTES
        self.SendTweet = True

    def ExecuteFirstInstance(self):
        Quotes = ["I find your lack of faith disturbing.", "You don't know the power of the dark side!",
                  "Luke, I am your father!.",
                  "Today will be a day long remembered. It has seen the death of Kenobi, and soon the fall of the rebellion.",
                  "The force is strong with this one.", "I sense something, a presence I've not felt since.......",
                  "You should not have come back!",
                  "The ability to destroy a planet is insignificant next to the power of the force.",
                  "Just for once, let me look at your face with my own eyes.",
                  "I've been waiting for you, Obi-wan. We meet again at last. The circle is now complete. When I left you I was but the learner. Now I am the master.",
                  "Perhaps I can find new ways to motivate them.", "Obi-Wan has taught you well.",
                  "Obi-Wan once thought as you do. You don't know the power of the Dark Side, I must obey my master.",
                  "It is too late for me, son. The Emperor will show you the true nature of the Force. He is your master now.",
                  "You are unwise to lower your defenses!", " As you wish.",
                  "No. Leave them to me. I will deal with them myself.", "My son is with them.",
                  "You cannot hide forever, Luke.", "Don't fail me again, Admiral.",
                  "Asteroids do not concern me, Admiral. I want that ship, not excuses.",
                  "He will join us or die, my master.",
                  "Alert all commands. Calculate every possible destination along their last known trajectory.",
                  "Impressive. Most impressive. Obi-Wan has taught you well. You have controlled your fear. Now, release your anger. Only your hatred can destroy me.",
                  "The force is with you, young Skywalker, but you are not a Jedi yet.",
                  "What is thy bidding, my master?", "When I left you I was but the learner. Now I am the master."]
        self.Text = choice(Quotes)
        super(SequencesQuotes, self).ExecuteFirstInstance()

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
        self.AllowRepeat = True
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
                elif v['Type'] == TWEETING:
                    sequences = SequencesTweet(v['Text'])
                elif v['Type'] == QUOTES:
                    sequences = SequencesQuotes()
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
