import datetime
from flask import json
import os
import threading
import time


class MyEncoder(json.JSONEncoder):
    def default(self, obj):
    # if not isinstance(obj, BaseResponse):
    #     return super(MyEncoder, self).default(obj)
    #     if isinstance(obj, datetime.datetime):
    #         return "Dasdasdasdasd".__dict__
        return obj.__dict__


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


class MediaPlayer():

    _instance = None

    def __new__(cls, *args, **kwargs):
        if not cls._instance:
            cls._instance = super(MediaPlayer, cls).__new__(
                cls, *args, **kwargs)
        return cls._instance

    def __init__(self):

        self.MediaPlayer = self.DetermineTheMediaPlayer()
        print "Media player is ", self.MediaPlayer

    def DetermineTheMediaPlayer(self):
        if os.path.exists("/usr/bin/mpg321"):
            return "/usr/bin/mpg321"
        else:
            return 'D:\\Work\\Projects\\Home\\BuildIndicatron\\var\\cmdmp3\cmdmp3.exe'

    def Play(self, file):
        print "Playing file"
        callSystem = self.MediaPlayer + ' ' + file + ''
        return os.system(callSystem)
