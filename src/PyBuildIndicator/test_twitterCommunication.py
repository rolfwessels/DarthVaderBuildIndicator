from unittest import TestCase
from CompositionRunner import CompositionRunner
from twitterComs import TwitterCommunication

__author__ = 'rolf'


class TestTwitterCommunication(TestCase):
    def __init__(self, methodName='runTest'):
        super(TestTwitterCommunication, self).__init__(methodName)
        self.Runner = CompRunnerNow()
        self.Runner.Stop()

    def testSendQuotes(self):
        tc = TwitterCommunication()
        tc.SendQuotes()
        assert True is True

    def testGetTimeLine(self):
        tc = TwitterCommunication()
        tc.GetTimeLine()
        pass

    def testGetTimeLineSteam(self):
        tc = TwitterCommunication()
        tc.GetTimeLineSteam(self.Runner)
        pass

    def test_ProcessTweet(self):
        tweet = {u'favorited': False, u'contributors': None, u'truncated': False, u'text': u'@gkdarth another test',
                 u'in_reply_to_status_id': None,
                 u'user': {u'follow_request_sent': None, u'profile_use_background_image': True,
                           u'default_profile_image': True, u'id': 525321924, u'verified': False,
                           u'profile_image_url_https': u'https://si0.twimg.com/sticky/default_profile_images/default_profile_1_normal.png',
                           u'profile_sidebar_fill_color': u'DDEEF6', u'profile_text_color': u'333333',
                           u'followers_count': 1, u'profile_sidebar_border_color': u'C0DEED', u'id_str': u'525321924',
                           u'profile_background_color': u'C0DEED', u'listed_count': 0,
                           u'profile_background_image_url_https': u'https://si0.twimg.com/images/themes/theme1/bg.png',
                           u'utc_offset': None, u'statuses_count': 3, u'description': None, u'friends_count': 4,
                           u'location': u'', u'profile_link_color': u'0084B4',
                           u'profile_image_url': u'http://a0.twimg.com/sticky/default_profile_images/default_profile_1_normal.png',
                           u'following': None, u'geo_enabled': False,
                           u'profile_background_image_url': u'http://a0.twimg.com/images/themes/theme1/bg.png',
                           u'name': u'Bal Sak', u'lang': u'en', u'profile_background_tile': False,
                           u'favourites_count': 0, u'screen_name': u'SnrBalSak', u'notifications': None, u'url': None,
                           u'created_at': u'Thu Mar 15 12:23:03 +0000 2012', u'contributors_enabled': False,
                           u'time_zone': None, u'protected': False, u'default_profile': True, u'is_translator': False},
                 u'filter_level': u'medium', u'geo': None, u'id': 331433415196938240L, u'favorite_count': 0,
                 u'lang': u'en', u'entities': {u'symbols': [], u'user_mentions': [
            {u'id': 1407739633, u'indices': [0, 8], u'id_str': u'1407739633', u'screen_name': u'GkDarth',
             u'name': u'Darth Vader'}], u'hashtags': [], u'urls': []}, u'created_at': u'Mon May 06 15:40:58 +0000 2013',
                 u'retweeted': False, u'coordinates': None, u'in_reply_to_user_id_str': u'1407739633',
                 u'source': u'web', u'in_reply_to_status_id_str': None, u'in_reply_to_screen_name': u'GkDarth',
                 u'id_str': u'331433415196938240', u'place': None, u'retweet_count': 0,
                 u'in_reply_to_user_id': 1407739633}
        tc = TwitterCommunication()
        tc.ProcessTweet(tweet, self.Runner)

    def test_ProcessTweet(self):
        tweet = {u'favorited': False, u'contributors': None, u'truncated': False, u'text': u'@gkdarth another test #lsgreen'}
        tc = TwitterCommunication()
        tc.ProcessTweet(tweet, self.Runner)


class CompRunnerNow(CompositionRunner):
    def AddChoreography(self, choreography):
        self.RunChoreography(choreography)


