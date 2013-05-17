from RPi import GPIO
import datetime
import re
import socket
from helperClasses import SleepingThread
from models import Passive, Choreography


class buttonClickRunner():
    def __init__(self, pin, composition_runner):
        self.CurrentPassive = Passive()
        self.Pin = pin
        GPIO.setup(self.Pin, GPIO.IN)
        self.PinValue = GPIO.input(self.Pin)
        self.Runner = composition_runner
        self.Choreography = []
        self.Choreography.append(Choreography.SimpleSequencesText2Speech(self.GetInitialString()))
        self.CurrentCount = 0
        self.LastCall = datetime.datetime.now()
        self.DelayBeforeReset = datetime.timedelta(0, 60)
        self.Timer = SleepingThread(self.ThreadTimerTick, 0.5)
        self.Timer.start()

    def SetLastComposition(self, lastComposition):
        self.LastComposition = lastComposition
        self.CurrentCount = -1
        self.LastCall = datetime.datetime.now()

    def SetChoreography(self, choreography):
        print "Setting new Choreography for button play", choreography
        self.Choreography = choreography

    def ThreadTimerTick(self):
        if self.PinValue != GPIO.input(self.Pin):
            self.PinValue = GPIO.input(self.Pin)
            self.RunNextSequence()

    def GetNextSequence(self):
        count = len(self.Choreography)
        if datetime.datetime.now() - self.LastCall > self.DelayBeforeReset or self.CurrentCount > (count - 1):
            print "reset"
            self.CurrentCount = 0
        if self.CurrentCount == -1:
            nextSequence = self.LastComposition
        else:
            nextSequence = self.Choreography[self.CurrentCount]
        print "count ", self.CurrentCount
        self.CurrentCount = min(count - 1, self.CurrentCount + 1)
        self.LastCall = datetime.datetime.now()
        return nextSequence

    def RunNextSequence(self):
        sequence = self.GetNextSequence()
        sequence.AllowRepeat = False
        self.Runner.AddChoreography(sequence)

    def GetInitialString(self):
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(("telkom.co.za", 80))
        myIp = s.getsockname()[0]
        s.close()
        myIp = myIp.replace(".", "dot ")
        myIp = re.sub(r'([1-9])', r'\1 ', myIp)
        return "What is thy bidding, my master? " + myIp
        pass

    def Stop(self):
        self.Timer.Stop()
        pass


