from unittest import TestCase
from CompositionRunner import CompositionRunner
from voiceRecognition import VoiceRecognition


__author__ = 'rolf'


class TestVoiceRecognition(TestCase):
    def test_recordClip(self):
        recognition = VoiceRecognition()
        for clips in recognition.recordClip():
            print "Say exit to continue"
            if "exit" in clips:
                break

    def test_processClips(self):
        recognition = VoiceRecognition()
        runner = CompositionRunner()

        for clips in recognition.processClips(runner):
            print "Say exit to continue"
            if "exit" in clips:
                runner.Stop()
                break
