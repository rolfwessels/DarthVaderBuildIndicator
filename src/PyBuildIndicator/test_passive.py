from unittest import TestCase
import unittest
from models import Passive, Choreography, Sequences, SequencesPlaySound

__author__ = 'rolf'



class TestPassive(TestCase):

    def test_Passive(self):
        dic = {u'Interval': 20, u'Compositions': [{u'Sequences': [{u'Type': u'playsound', u'File': u'darthvader_lackoffaith.wav'}]}], u'StartTime': u'07:00', u'SleepTime': u'20:00'}
        assert "'Interval': 20" in str(dic)
        cc = Passive(dic)
        assert 20 == cc.Interval
    pass

    def test_Passive_Compositions(self):
        dic = {u'Interval': 20, u'Compositions': [{u'Sequences': [{u'Type': u'playsound', u'File': u'darthvader_lackoffaith.wav'}]}], u'StartTime': u'07:00', u'SleepTime': u'20:00'}
        cc = Passive(dic)
        assert len(cc.Compositions) == 1
        assert isinstance(cc.Compositions[0],Choreography)
    pass

    def test_Passive_Sequences(self):
        dic = {u'Interval': 20, u'Compositions': [{u'Sequences': [{u'Type': u'playsound', u'File': u'darthvader_lackoffaith.wav'}]}], u'StartTime': u'07:00', u'SleepTime': u'20:00'}
        cc = Passive(dic)
        cor = cc.Compositions[0]
        assert len(cor.Sequences) == 1
        sequences_ = cor.Sequences[0]
        assert isinstance(sequences_,SequencesPlaySound)
        assert sequences_.Type == "playsound"
        assert sequences_.File == "darthvader_lackoffaith.wav"
    pass

if __name__ == '__main__':
    unittest.main()
