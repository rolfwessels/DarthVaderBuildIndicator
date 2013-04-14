
class PassiveManager():
    def __init__(self):
        self.CurrentPassive = Passive()

    def SetCurrentPassive(self, passive):
        print "Setting new passive",passive
        self.CurrentPassive = passive
        pass


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
        self.Interval = 0
        self.StartTime = "06:00"
        self.SleepTime = "19:00"
        self.Compositions = []

    def __init__(self, ** params):
        self.__dict__.update(params)


# class SequencesPlaySound(Sequences):
#     def __init__(self):
#         super(SequencesPlaySound, self).__init__(typeName)
#         self.File = ""
