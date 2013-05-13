import re
import time
from models import Choreography, SequencesGpIo, SequencesText2Speech, SequencesPlaySound, SequencesQuotes, SequencesTweet, SequencesJokeOrOneLiner
from pins import lsRedPin, lsGreenPin, lsBluePin, bRedPin, bGreenPin
from twitter import TwitterStream, OAuth
from twitterComs import TwitterCommunication, oauth_token, oauth_secret, CONSUMER_KEY, CONSUMER_SECRET, monitorName, screen_name
from wolframalphaLookup import WolframalphaLookup

__author__ = 'rolf'


class TwitterListener(TwitterCommunication):
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

    def processText(self, compositionRunner, textToRead):
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

        if textToRead.startswith('say something'):
            if "funny" in textToRead.lower():
                choreography.Sequences.append(SequencesPlaySound("Funny"))
            elif "fail" in textToRead.lower() or "bad" in textToRead.lower():
                choreography.Sequences.append(SequencesPlaySound("Fail"))
            elif "start" in textToRead.lower() or "good" in textToRead.lower():
                choreography.Sequences.append(SequencesPlaySound("Start"))
            elif "success" in textToRead.lower():
                choreography.Sequences.append(SequencesPlaySound("Success"))
            else:
                choreography.Sequences.append(SequencesPlaySound("WTF"))
        elif "tell" in textToRead.lower() and "quote" in textToRead.lower():
            choreography.Sequences.append(SequencesQuotes())
        elif "tell" in textToRead.lower() and "joke" in textToRead.lower():
            choreography.Sequences.append(SequencesJokeOrOneLiner())
        elif textToRead.startswith('tweet'):
            choreography.Sequences.append(SequencesTweet(textToRead[6:]))
        elif textToRead.startswith('say'):
            choreography.Sequences.append(SequencesText2Speech(textToRead[3:]))
        elif "what" in textToRead.lower() or "where" in textToRead.lower() or "who" in textToRead.lower() or "why" in textToRead.lower() or "when" in textToRead.lower() or "how" in textToRead.lower() or "define" in textToRead.lower():
            loopkupQuestion = textToRead.replace("you","darth vader").replace("your","darth vader")
            lookup = WolframalphaLookup()
            result = lookup.LookupResult(loopkupQuestion)
            t2s = SequencesText2Speech(result)
            t2s.DisableTransform = True
            choreography.Sequences.append(t2s)
        if len(choreography.Sequences) > 0:
            compositionRunner.AddChoreography(choreography)
        else:
            print "No Sequences so not adding to compositionRunner"

    def ProcessTweet(self, tweet, compositionRunner):
        if (tweet is None) or ('text' not in tweet): return
        if (tweet['user']['screen_name'] == screen_name): return
        textToRead = tweet['text']

        self.processText(compositionRunner, textToRead)

    def RemoveText(self, searchText=None, fromText=None, replacement=''):
        remove = re.compile(re.escape(searchText), re.IGNORECASE)
        newText = remove.sub(replacement, fromText).strip()
        return newText, (newText != fromText)