from unittest import TestCase
from robotServer.models import SequencesJokeOrOneLiner

__author__ = 'rolf'


class TestSequencesJokeOrOneLiner(TestCase):
    def testGetRandom(self):
        sec = SequencesJokeOrOneLiner()
        var = sec.GetOneLiner()
        print var
        assert var is not None


    def testExecuteFirstInstance(self):
        sec = SequencesJokeOrOneLiner()
        var = sec.ExecuteFirstInstance()
