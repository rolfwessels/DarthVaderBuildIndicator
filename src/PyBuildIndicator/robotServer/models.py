from robotServer.helperClasses import MediaPlayer

__author__ = 'rolf'
PLAYSOUND = "playsound"

class Sequences(object):
    def __init__(self, typeName):
        self.__type = typeName

    def get_Type(self):
        return self.__type

    Type = property(fget=get_Type)


class SequencesPlaySound(Sequences):
    def __init__(self, fileName=""):
        super(SequencesPlaySound, self).__init__(PLAYSOUND)
        self.File = fileName


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


class Choreography(object):
    def __init__(self):
        self.__sequences = []

    def get_Sequences(self):
        return self.__sequences

    def set_Sequences(self, value):
        self.__sequences = value

    Sequences = property(fget=get_Sequences, fset=set_Sequences)

    def Execute(self):
        for sequence in self.Sequences:
            if sequence.Type == PLAYSOUND:
                MediaPlayer().Play(self.Sequences.File)


    @staticmethod
    def SimpleChoreographyPlaySound(fileName):
        choreography = Choreography()
        choreography.Sequences.append(SequencesPlaySound(fileName))
        return choreography