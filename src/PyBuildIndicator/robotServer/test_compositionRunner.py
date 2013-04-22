from unittest import TestCase
from robotServer.CompositionRunner import CompositionRunner
from robotServer.models import Choreography


class TestCompositionRunner(TestCase):
    def test_RunCorr_darthvader_dontmakeme(self):
        runner = CompositionRunner()
        runner.RunCorr(Choreography.SimpleChoreographyPlaySound("Funny/darthvader_dontmakeme.wav"))

    def test_RunCorr(self):
        runner = CompositionRunner()
        runner.RunCorr(Choreography.SimpleChoreographyPlaySound("Funny"))
