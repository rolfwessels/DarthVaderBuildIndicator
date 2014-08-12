from RPi import GPIO
from flask import json
import hashlib
import os
from random import choice
import re
import urllib
import urllib2
from helperClasses import MediaPlayer
from pins import GPIOoutput
from twitterComs import TwitterCommunication


TEXT_SPEECH = "Text2Speech"
TEXT_ONELINER = "oneliner"
PLAYSOUND = "playsound"
GP_IO = 'GpIO'
TWEETING = "Tweet"
QUOTES = "Quotes"
RESOURCES_TEXT_SPEACH_ = 'resources/text2speach/'
RESOURCES_SOUNDS_ = 'resources/sounds/'
INSULT = "Insult"


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
        GPIOoutput(self.Pin,self.IsOn)
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

        if self.SendTweet and len(self.Text) > 0:
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
                self.EnsureBackgroundSoundIsThere()
                os.system(
                    "play -q -m resources/text2speach/background.mp3  -v 5 " + self.SaveTo + " speed 0.78 echo 0.8 0.88 6.0 0.4")
            else:
                os.system("play -q -v 5 " + self.SaveTo)


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

    def EnsureBackgroundSoundIsThere(self):
        path = "resources/text2speach/"
        tmpB = path + "background.mp3"
        if not os.path.exists(tmpB):
            background = path + "Star-Wars-1391.mp3"
            os.system("sox -v 0.2 " + background + " -r 16000 " + tmpB)
        pass


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
        Quotes = ["SendQuotesI find your lack of faith disturbing.", "You don't know the power of the dark side!",
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


class SequencesInsult(SequencesText2Speech):
    def __init__(self):
        super(SequencesInsult, self).__init__("")
        self.__type = INSULT
        self.SendTweet = True

    def ExecuteFirstInstance(self):
        Insult = ["Your birth certificate is an apology letter from the condom factory.",
                  "You must have been born on a highway because that's where most accidents happen.",
                  "I don't exactly hate you, but if you were on fire and I had water, I'd drink it.",
                  "You are proof that God has a sense of humor.",
                  "Shut up, you'll never be the man your mother is.",
                  "Hey, you have somthing on your chin... no, the 3rd one down",
                  "Do you wanna lose ten pounds of ugly fat? Cut off your head",
                  "Am I getting smart with you? How would you know?",
                  "Your family tree must be a cactus because everybody on it is a prick.",
                  "I'd like to see things from your point of view but I can't seem to get my head that far up my ass.",
                  "It's better to let someone think you are an Idiot than to open your mouth and prove it.",
                  "You are so stupid, you'd trip over a cordless phone.",
                  "You are so old, your birth-certificate expired.",
                  "Well I could agree with you, but then we'd both be wrong.",
                  "So, a thought crossed your mind? Must have been a long and lonely journey.",
                  "Why don't you slip into something more comfortable -- like a coma.",
                  "If assholes could fly, this place would be an airport!",
                  "Are you always an idiot, or just when I'm around?",
                  "You stare at frozen juice cans because they say, concentrate.",
                  "Come again when you can't stay quite so long.",
                  "You may not be the best looking girl here, but beauty is only a light switch away!",
                  "It looks like your face caught on fire and someone tried to put it out with a hammer.",
                  "I love what you've done with your hair. How do you get it to come out of the nostrils like that?",
                  "You do realize makeup isn't going to fix your stupidity?",
                  "Ordinarily people live and learn. You just live.",
                  "Looks like you traded in your neck for an extra chin!",
                  "Don't you need a license to be that ugly?",
                  "You occasionally stumble over the truth, but you quickly pick yourself up and carry on as if nothing happened.",
                  "I've seen people like you, but I had to pay admission!",
                  "Jesus loves you, everyone else thinks you're an asshole!",
                  "Shock me, say something intelligent.",
                  "Don't feel sad, don't feel blue, Frankenstein was ugly too.",
                  "Aww, it's so cute when you try to talk about things you don't understand.",
                  "You are proof that evolution CAN go in reverse.",
                  "You are so old, you fart dust.",
                  "I may be fat, but you're ugly, and I can lose weight.",
                  "I heard you took an IQ test and they said you're results were negative.",
                  "If I want your opinion, I'll give it to you.",
                  "I wish you no harm, but it would have been much better if you had never lived.",
                  "When was the last time you could see your whole body in the mirror?",
                  "If a crackhead saw you, he'd think he needs to go on a diet.",
                  "Being around you is like having a cancer of the soul.",
                  "Even if you were twice as smart, you'd still be stupid!",
                  "We all sprang from apes, but you didn't spring far enough.",
                  "Learn from your parents' mistakes - use birth control!",
                  "If what you don't know can't hurt you, you're invulnerable.",
                  "Do you still love nature, despite what it did to you?",
                  "Your parents hated you so much you bath toys were an iron and a toaster",
                  "You're as useless as a screen door on a submarine.",
                  "Maybe if you ate some of that makeup you could be pretty on the inside.",
                  "I'd like to help you out. Which way did you come in?",
                  "If you had another brain, it would be lonely.",
                  "I don't know what makes you so stupid, but it really works!",
                  "You must think you're strong, but you only smell strong.",
                  "The best part of you is still running down your old mans leg.",
                  "100,000 sperm, you were the fastest?",
                  "Yeah you're pretty, pretty stupid",
                  "Nice tan, orange is my favorite color.",
                  "Is your name Maple Syrup? It should be, you sap.",
                  "Brains aren't everything. In your case they're nothing.",
                  "I look into your eyes and get the feeling someone else is driving.",
                  "If you spoke your mind, you'd be speechless.",
                  "Ever since I saw you in your family tree, I've wanted to cut it down.",
                  "Just reminding u there is a very fine line between hobby and mental illness.",
                  "Your mom must have a really loud bark!",
                  "Are your parents siblings?",
                  "You're as useful as an ashtray on a motorcycle.",
                  "You act like your arrogance is a virtue.",
                  "Beauty is skin deep, but ugly is to the bone.",
                  "You're the reason why women earn 75 cents to the dollar.",
                  "When anorexics see you, they think they need to go on a diet.",
                  "I hear the only place you're ever invited is outside.",
                  "Please tell me you don't home-school your kids.",
                  "You'll make a great first wife some day.",
                  "You are so old, even your memory is in black and white.",
                  "For those who never forget a face, you are an exception.",
                  "People like you are the reason I work out.",
                  "You're stupid because you're blonde."]
        self.Text = choice(Insult)
        super(SequencesInsult, self).ExecuteFirstInstance()


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
                    sequences.SendTweet = v['SendTweet']
                elif v['Type'] == TWEETING:
                    sequences = SequencesTweet(v['Text'])
                elif v['Type'] == QUOTES:
                    sequences = SequencesQuotes()
                    sequences.SendTweet = v['SendTweet']
                elif v['Type'] == INSULT:
                    sequences = SequencesInsult()
                    sequences.SendTweet = v['SendTweet']
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
