from Queue import Queue
from RPi import GPIO
from datetime import datetime
import threading
import time
from robotServer.helperClasses import SleepingThread, MediaPlayer
from robotServer.models import Choreography, PLAYSOUND, RESOURCES_SOUNDS_, GP_IO, SequencesGpIo, Sequences


class CompositionRunner():
    def __init__(self):
        self.Initialize()
        self.StartTheThread()

    def Initialize(self):
        self.sleepTime = 1
        self.queue = Queue()

    def StartTheThread(self):
        self.Timer = SleepingThread(self.WorkingFunction, 1)
        self.Timer.start()
        print "Starting the running thread"

    def WorkingFunction(self):
        item = self.queue.get()
        print  "Looking at ", item
        try:
            self.RunCorr(item)
        finally:
            self.queue.task_done()

    def AddChoreography(self, choreography):
        self.queue.put(choreography)

    def RunCorr(self, item):
        startTime = datetime.now()
        allDone = False
        while not allDone:
            timedDifference = datetime.now() - startTime
            allDone = True
            for sequence in item.Sequences:
                if isinstance(sequence,Sequences) :
                    sequence.Execute(timedDifference.total_seconds()*1000)
                    allDone = allDone and sequence.Done
                else :
                    print "Huh this is not a Sequences", sequence
            time.sleep(0.01)
        # reset to allow another playback
        for sequence in item.Sequences:
            sequence.Done = False


