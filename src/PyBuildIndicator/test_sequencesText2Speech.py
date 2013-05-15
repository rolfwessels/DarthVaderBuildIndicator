from unittest import TestCase
from models import SequencesText2Speech
from twitterComs import TwitterCommunication

__author__ = 'rolf'


class TestSequencesText2Speech(TestCase):
    def testSplitString(self):
        string = "Oh no, there are currently 2 build on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels and Coreen"
        strings = []

        sentences = string.split('.')
        for sentence in sentences:
            if len(sentence) < 80:
                strings.append(sentence)
            else:
                st = ""
                for i, word in enumerate(sentence.split(), 1):
                    if len(st) < 80:
                        st += word+" "
                    else:
                        strings.append(st)
                        st = ""
                strings.append(st)
        print strings

    def testingSample(self):
        string = "luke    i'm your father"
        sec = SequencesText2Speech(string)
        sec.PlayPartText(string)
        print string

    def testingSample1(self):
        string = "Oh no, there are currently 2 build on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels and Coreen"
        sec = SequencesText2Speech(string)
        sec.ExecuteFirstInstance()
        print string


    def testingSample_single(self):
        string = "Dirk listen to this"
        sec = SequencesText2Speech(string)
        sec.PlayPartText(string)
        print string
