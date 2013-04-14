import datetime
from random import choice
import threading
import time


class SleepingThread(threading.Thread):
    def __init__(self, action, seconds=20000000):
        self.sleepTime = seconds
        self.action = action
        self.stopped = False
        threading.Thread.__init__(self)

    def run(self):
        while not self.stopped:
            time.sleep(self.sleepTime)
            self.action()

class PassiveManager():
    def __init__(self):
        self.CurrentPassive = Passive()
        self.Timer = SleepingThread(self.ThreadTimerTick, 5)
        self.Timer.start()

    def SetCurrentPassive(self, passive):
        print "Setting new passive", passive
        if isinstance(passive, Passive):
            self.CurrentPassive = passive

    def ThreadTimerTick(self):
        if self.CurrentPassive.LastRun is None \
            or (datetime.datetime.now() - self.CurrentPassive.LastRun).total_seconds() > self.CurrentPassive.Interval:
            self.CurrentPassive.LastRun = datetime.datetime.now()
            self.RunRandomSequence()
        else:
            seconds = (datetime.datetime.now() - self.CurrentPassive.LastRun).total_seconds()
            print "The passive is ticking", seconds

    def RunRandomSequence(self):
        print "Running a Composition"
        if len(self.CurrentPassive.Compositions) > 0:
            print choice(self.CurrentPassive.Compositions)
        else:
            print "no items to choose from"


class Sequences(object):
    def __init__(self, typeName):
        self.__type = typeName

    def get_Type(self):
        return self.__type

    Type = property(fget=get_Type)


class Choreography(object):
    def __init__(self):
        self.__sequences = []

    def get_Sequences(self):
        return self.__sequences

    def set_Sequences(self, value):
        self.__sequences = value

    Sequences = property(fget=get_Sequences, fset=set_Sequences)


class Passive(object):
    def __init__(self):
        self.Initialize()

    def __init__(self, **params):
        self.Initialize()
        self.__dict__.update(params)

    def Initialize(self):
        self.Interval = 5000
        self.StartTime = "06:00"
        self.SleepTime = "19:00"
        self.LastRun = None
        self.Compositions = []


# class SequencesPlaySound(Sequences):
#     def __init__(self):
#         super(SequencesPlaySound, self).__init__(typeName)
#         self.File = ""
