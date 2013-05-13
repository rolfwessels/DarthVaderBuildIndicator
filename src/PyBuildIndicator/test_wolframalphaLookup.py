from unittest import TestCase
from wolframalphaLookup import WolframalphaLookup

__author__ = 'rolf'


class TestWolframalphaLookup(TestCase):
    def test_LookupResult(self):
        lookup = WolframalphaLookup()
        result = lookup.LookupResult("Who is the president of south africa")
        print "test:"+ result
        assert "jacob" in result.lower()

    def test_LookupResult_Darth(self):
        lookup = WolframalphaLookup()
        result = lookup.LookupResult("Who is darth vader mother")
        print "test:"+ result

    def test_LookupResult_Father(self):
        lookup = WolframalphaLookup()
        result = lookup.LookupResult("Who is darth vader father")
        print "test:"+ result

    def test_LookupResult_NotFound(self):
        lookup = WolframalphaLookup()
        result = lookup.LookupResult("Who is my mother")
        print "test:"+ result