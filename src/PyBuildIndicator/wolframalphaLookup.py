import HTMLParser
import json
from random import choice
import re
import urllib
import urllib2

LOOKUP_URL = 'https://www.wolframalpha.com/input/?i='


class WolframalphaLookup(object):
    def __init__(self):
        self.ThreshHold = 3.0
        pass

    def LookupResult(self, request):
        result = ""
        try:
            requestUrl = LOOKUP_URL + urllib.quote_plus(request)
            print "Calling " + requestUrl
            req = urllib2.Request(requestUrl)
            req.add_header('User-Agent', 'Mozilla/5.0')
            result = urllib2.urlopen(req)
            result = result.read()
            res = "0200\\.push\\( \\{\"stringified\": \"(.*?)\",\""
            m = re.search(res, result)
            result = m.group(1)
        except:
            result = ""

        if len(result) == 0:
            result = choice(["Let me check on that.",
                             "That's a really good question.",
                             "So this is really preliminary data, but we felt that it was important that we get this out there first...",
                             "Try that again",
                             "Look at the time! Gotta go!",
                             "Who said that",
                             "Here's the NMR, Professor Sames!",
                             "Did rolf put you up to this?",
                             "I think that's in my lab notebook.",
                             "I haven't looked at that yet.",
                             "The literature doesn't really speak to that.",
                             "But, it's obviously the thermodynamic product!",
                             "hmmmm",
                             "You know, that's something we're going to look at in our next round of experiments.",
                             "Try any other question?",
                             "IDK my BFF Jill?",
                             "Huh.",
                             "You know, there are some questions that we haven't been able to answer.",
                             "I don't know."])
        h = HTMLParser.HTMLParser()
        result = h.unescape(result).replace("\\n", " ")
        return result

