import os
from random import choice
import re
import time
from models import Choreography, SequencesText2Speech, SequencesGpIo
from twitter import *
from pins import *

CREDENTIALS_LOCATION = '~/.my_app_credentials'
CONSUMER_KEY = "cwuarzRNlG8hKCTYhhpaw"
CONSUMER_SECRET = "LRRnwTRdH4QCQNKDdtZcdoUzHBFOM0TS1TEIE3LVpkA"
oauth_token = "1407739633-ah36rx9iRPOymLDIo6eKsD4K1tPJotzfGKKbR5o"
oauth_secret = "ljbxI03SHee9zPKRsalHg2SxOxlR245cH0eArHomQZ8"
monitorName = "@gkdarth"


class TwitterCommunication(object):
    def __init__(self):
        self.twitter = None
        self.Quotes = ["I find your lack of faith disturbing.", "You don't know the power of the dark side!",
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

    def EnsureLogin(self):
        if self.twitter is None:
            self.twitter = Twitter(auth=OAuth(
                oauth_token, oauth_secret, CONSUMER_KEY, CONSUMER_SECRET))

    def SendTweet(self, message):
        self.EnsureLogin()
        self.twitter.statuses.update(status=message)

    def SendQuotes(self):
        self.SendTweet(choice(self.Quotes))

    def GetTimeLine(self):
        self.EnsureLogin()
        timeline = self.twitter.statuses.home_timeline()
        print timeline
        return timeline


    def GetTimeLineSteam(self, compositionRunner=None, block=True):
        while True:
            try:
                twitter_stream = TwitterStream(
                    domain="userstream.twitter.com",
                    api_version="1.1",
                    auth=OAuth(oauth_token, oauth_secret, CONSUMER_KEY, CONSUMER_SECRET),
                    block=block)
                iterator = twitter_stream.user()
                for tweet in iterator:
                    self.ProcessTweet(tweet, compositionRunner)
            except Exception:
                time.sleep(60)

    def ProcessTweet(self, tweet, compositionRunner):
        if (tweet is not None) and ('text' not in tweet): return
        textToRead = tweet['text']

        inter = self.RemoveText(monitorName, textToRead)
        if inter[1]:
            textToRead = inter[0]

        choreography = Choreography()
        if "#ls" in textToRead:
            inter = self.RemoveText('#lsred', textToRead)
            choreography.Sequences.append(SequencesGpIo(lsRedPin, inter[1]))
            textToRead = inter[0]

            inter = self.RemoveText('#lsGreen', textToRead)
            choreography.Sequences.append(SequencesGpIo(lsGreenPin, inter[1]))
            textToRead = inter[0]

            inter = self.RemoveText('#lsBlue', textToRead)
            choreography.Sequences.append(SequencesGpIo(lsBluePin, inter[1]))
            textToRead = inter[0]

        if "#b" in textToRead:
            inter = self.RemoveText('#bRed', textToRead)
            choreography.Sequences.append(SequencesGpIo(bRedPin, inter[1]))
            textToRead = inter[0]

            inter = self.RemoveText('#bGreen', textToRead)
            choreography.Sequences.append(SequencesGpIo(bGreenPin, inter[1]))
            textToRead = inter[0]

        choreography.Sequences.append(SequencesText2Speech(textToRead))
        compositionRunner.AddChoreography(choreography)

    def RemoveText(self, searchText=None, fromText=None, replacement=''):
        remove = re.compile(re.escape(searchText), re.IGNORECASE)
        newText = remove.sub(replacement, fromText).strip()
        return newText, (newText != fromText)

