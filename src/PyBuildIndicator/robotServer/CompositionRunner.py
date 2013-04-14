from Queue import Queue
import threading
import time
from robotServer.helperClasses import SleepingThread
from robotServer.models import Choreography


class CompositionRunner():
    def __init__(self):
        self.Initialize()
        self.StartTheThread()

    def Initialize(self):
        self.sleepTime = 5
        self.queue = Queue()

    def StartTheThread(self):
        self.Timer = SleepingThread(self.WorkingFunction,2)
        self.Timer.start()
        print "Starting the running thread"

    def WorkingFunction(self):
        item = self.queue.get()
        print  "Looking at ",item
        try:
            item.Execute()
        finally:
            self.queue.task_done()

    def AddChoreography(self, choreography):
        if isinstance(choreography, Choreography):
            print "Enqueue new choreography"
            self.queue.put(choreography)
