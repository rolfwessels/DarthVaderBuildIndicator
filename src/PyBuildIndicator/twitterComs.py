from random import choice
from twitter import *

CREDENTIALS_LOCATION = '~/.my_app_credentials'
CONSUMER_KEY = "cwuarzRNlG8hKCTYhhpaw"
CONSUMER_SECRET = "LRRnwTRdH4QCQNKDdtZcdoUzHBFOM0TS1TEIE3LVpkA"
oauth_token = "1407739633-ah36rx9iRPOymLDIo6eKsD4K1tPJotzfGKKbR5o"
oauth_secret = "ljbxI03SHee9zPKRsalHg2SxOxlR245cH0eArHomQZ8"
monitorName = "@gkdarth"
screen_name = "GkDarth"


class TwitterCommunication(object):
    def __init__(self):
        self.twitter = None

    def EnsureLogin(self):
        if self.twitter is None:
            self.twitter = Twitter(auth=OAuth(
                oauth_token, oauth_secret, CONSUMER_KEY, CONSUMER_SECRET))

    def SendTweet(self, message):
        self.EnsureLogin()
        self.twitter.statuses.update(status=message)



