from flask import json
import urllib2

URL_PARAMETERS = "api/json?depth=2&tree=jobs[name,color,healthReport[score],builds[duration,result],lastFailedBuild[number,timestamp,changeSet[items[author[fullName]]]]]"


class JenkinsBuildServer(object):
    def __init__(self, host="http://localhost:5000/"):
        self.Host = host
        pass

    def GetJenkinsData(self):
        url = self.Host + URL_PARAMETERS
        req = urllib2.Request(url)
        res = urllib2.urlopen(req)
        result = json.load(res)
        return result

    def GetStatus(self):
        result = self.GetJenkinsData()
        valid = 0
        invalid = 0
        count = 0
        for jobs in result['jobs']:
            if jobs['color'] == 'blue':
                valid += 1
                count += 1
            if jobs['color'] == 'red':
                invalid += 1
                count += 1
        if invalid == 0:
            return "Good work, there are currently " + str(count) + " builds on jenkins and they are all passing"
        if valid == 0:
            return "There are currently " + str(
                count) + " builds on jenkins and they are all broken. Maybe development is not for you"

        return "You have failed me, there are currently " + str(count) + " builds on jenkins with " + str(invalid) + " builds failing"

    def GetFailingBuilds(self):
        result = self.GetJenkinsData()

        buildFailed = ""
        for jobs in result['jobs']:
            if jobs['color'] == 'red':
                if buildFailed != "":
                    buildFailed += " and "
                buildFailed += jobs['name'] + " failed"
        if buildFailed != "":
            return buildFailed
        return "All builds are passing"

    def GetWhoBrokeTheBuilds(self):
        result = self.GetJenkinsData()
        authors = []
        for jobs in result['jobs']:
            if jobs['color'] == 'red':
                for items in jobs['lastFailedBuild']['changeSet']['items']:
                    authors.append(items['author']['fullName'])

        if len(authors) > 0:
            return " and ".join(authors)
        return "All builds are passing"
