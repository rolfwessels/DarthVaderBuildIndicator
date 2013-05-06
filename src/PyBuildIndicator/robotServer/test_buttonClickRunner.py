from RPi import GPIO
import datetime
from pprint import pprint
from unittest import TestCase
from buttonClickRunner import buttonClickRunner
from models import Choreography

__author__ = 'rolf'


class TestButtonClickRunner(TestCase):
    def testGetInitialString(self):
        GPIO.setmode(GPIO.BCM)
        runner = buttonClickRunner(22, None)
        string = runner.GetInitialString()

        print string
        runner.Stop()
        assert '1 9 2 dot 1 6 8' in string
    pass

    def testGettingTheNextCor(self):
        GPIO.setmode(GPIO.BCM)

        runner = buttonClickRunner(22, None)
        cc = [
            Choreography.SimpleSequencesText2Speech("one"),
            Choreography.SimpleSequencesText2Speech("two"),
            Choreography.SimpleSequencesText2Speech("three"),
        ]

        runner.SetChoreography(cc)
        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "one"

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "two"

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "three"

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "three"

        runner.Stop()
        pass

    def testGettingTheNextCorWithDelay(self):
        GPIO.setmode(GPIO.BCM)

        runner = buttonClickRunner(22, None)
        cc = [
            Choreography.SimpleSequencesText2Speech("one"),
            Choreography.SimpleSequencesText2Speech("two"),
            Choreography.SimpleSequencesText2Speech("three"),
            ]

        runner.SetChoreography(cc)
        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "one"

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "two"

        print runner.LastCall
        runner.LastCall = datetime.datetime.now() - runner.DelayBeforeReset - runner.DelayBeforeReset
        print runner.LastCall

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "one"

        sequence = runner.GetNextSequence()
        pprint(sequence.Sequences[0].Text)
        assert sequence.Sequences[0].Text is "two"

        runner.Stop()
        pass
