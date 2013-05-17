from unittest import TestCase
from jenkinsBuildServer import JenkinsBuildServer

__author__ = 'rolf'


class TestJenkinsBuildServer(TestCase):
    def test_GetStatus(self):
        new = JenkinsBuildServer()
        status = new.GetStatus()
        print status
        assert "builds" in status

    def test_GetFailingBuilds(self):
        new = JenkinsBuildServer()
        status = new.GetFailingBuilds()
        print status
        assert "failed" in status

    def test_GetWhoBrokeTheBuilds(self):
        new = JenkinsBuildServer()
        status = new.GetWhoBrokeTheBuilds()
        print status
        assert "Sean" in status
