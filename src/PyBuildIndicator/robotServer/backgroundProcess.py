import datetime
from random import choice
from robotServer.helperClasses import SleepingThread
from robotServer.models import Passive


class PassiveManager():
    def __init__(self, composition_runner):
        self.CurrentPassive = Passive()
        self.Timer = SleepingThread(self.ThreadTimerTick, 5)
        self.Timer.start()
        self.runner = composition_runner
        self.LastRun = datetime.datetime.now()

    def SetCurrentPassive(self, passive):
        print "Setting new passive", passive
        if isinstance(passive, Passive):
            self.CurrentPassive = passive

    def ThreadTimerTick(self):
        pass
        if  (datetime.datetime.now() - self.LastRun).total_seconds() > (self.CurrentPassive.Interval*60):
            self.CurrentPassive.LastRun =  str(datetime.datetime.now())
            self.LastRun = datetime.datetime.now()
            self.RunRandomSequence()
        # else:
        #     seconds = (datetime.datetime.now() - self.LastRun).total_seconds()
            # print "The passive is ticking", seconds

    def RunRandomSequence(self):
        print "Running a Composition"
        if len(self.CurrentPassive.Compositions) > 0:
            self.runner.AddChoreography(choice(self.CurrentPassive.Compositions))
        else:
            print "no items to choose from"


