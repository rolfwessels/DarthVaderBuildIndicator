from unittest import TestCase
from CompositionRunner import CompositionRunner
from models import Choreography


class TestCompositionRunner(TestCase):
    def test_RunCorr_darthvader_dontmakeme(self):
        runner = CompositionRunner()
        runner.RunChoreography(Choreography.SimpleChoreographyPlaySound("Funny/darthvader_dontmakeme.wav"))

    def test_RunCorr(self):
        runner = CompositionRunner()
        runner.RunChoreography(Choreography.SimpleChoreographyPlaySound("Funny"))
