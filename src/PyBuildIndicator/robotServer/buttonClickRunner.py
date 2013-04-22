from RPi import GPIO
from robotServer.helperClasses import SleepingThread
from robotServer.models import Passive, Choreography


class buttonClickRunner():
    def __init__(self, pin, composition_runner):
        self.CurrentPassive = Passive()
        self.Pin = pin
        GPIO.setup(self.Pin,GPIO.IN)
        self.PinValue = GPIO.input(self.Pin)
        self.Runner = composition_runner
        self.Choreography = Choreography.SimpleSequencesText2Speech("please add a button Choreography")
        self.Timer = SleepingThread(self.ThreadTimerTick, 0.5)
        self.Timer.start()


    def SetChoreography(self, choreography):
        print "Setting new Choreography for button play", choreography
        if isinstance(choreography, Choreography):
            self.Choreography = choreography

    def ThreadTimerTick(self):
        if self.PinValue != GPIO.input(self.Pin):
            self.PinValue = GPIO.input(self.Pin)
            self.RunRandomSequence()

    def RunRandomSequence(self):
        self.Runner.AddChoreography(self.Choreography)


