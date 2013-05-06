import datetime
from flask.helpers import json
import os
from random import choice
from helperClasses import SleepingThread, MyEncoder
from models import Passive

FileLocation = "Resources/passive.json"

class PassiveManager():
    def __init__(self, composition_runner):
        self.CurrentPassive = Passive()
        self.Timer = SleepingThread(self.ThreadTimerTick, 5)
        self.Timer.start()
        self.runner = composition_runner
        self.LastRun = datetime.datetime.now()
        self.LoadFile()

    def LoadFile(self):
        print "Loading passive "+FileLocation
        if os.path.isfile(FileLocation):
            filePointer = open(FileLocation,"r")
            json_load = json.load(filePointer)
            self.CurrentPassive = Passive(json_load)
            print "Passive loaded" + str(json_load)
            filePointer.close()

    def SaveToFile(self, passive):

        try:
            print "Save passive"
            filePointer = open(FileLocation,"w")
            json_dumps = json.dumps(passive, cls=MyEncoder)
            print json_dumps
            filePointer.write(json_dumps)
            filePointer.flush()
            filePointer.close()
            print "done saving passive"
        except ValueError:
            print "Oops!  That was no valid number.  Try again..."


    def SetCurrentPassive(self, passive):
        print "Setting new passive", passive
        if isinstance(passive, Passive):
            self.CurrentPassive = passive
            self.SaveToFile(passive)

    def ThreadTimerTick(self):
        datetime_now = datetime.datetime.now()
        if datetime_now.hour >= self.CurrentPassive.SleepTime:
            # print "skip because of the hour is less than sleep time"
            return

        if datetime_now.hour < self.CurrentPassive.StartTime:
            # print "skip because of the time is less than start time"
            return

        if  (datetime_now - self.LastRun).total_seconds() > (self.CurrentPassive.Interval*60):
            self.CurrentPassive.LastRun = str(datetime_now)
            self.LastRun = datetime_now
            self.RunRandomSequence()


    def RunRandomSequence(self):
        print "Running a Composition"
        if len(self.CurrentPassive.Compositions) > 0:
            self.runner.AddChoreography(choice(self.CurrentPassive.Compositions))
        else:
            print "no items to choose from"


