# -*- coding: utf-8 -*-
import os
import serverStart
import unittest


class serverStart_Tests(unittest.TestCase):
    def setUp(self):
        """Before each test, set up a blank database"""
        print "Starting"
        self.app = serverStart.app.test_client()

    def tearDown(self):
        """Get rid of the database again after each test."""

    def test_ping(self):
        """Test that messages work"""
        rv = self.app.get('/ping')
        assert '"Success": true' in rv.data
        assert '"ErrorMessage": ""' in rv.data

    def test_playMp3File(self):
        """Test it plays a mp3 file"""
        rv = self.app.get('/playmp3file/As_You_Wish_Sound_Effect.mp3')
        print rv.data
        assert '"Success": true' in rv.data
        assert '"ErrorMessage": ""' in rv.data
        assert '"FileFound": true' in rv.data
        assert '"callResponse": 0' in rv.data

    def test_playMp3File_fail(self):
        """Test it does not find a mp3 file"""
        rv = self.app.get('/playmp3file/As_You_Wish_Sound_Effect_not_exists.mp3')
        print rv.data
        assert '"Success": false' in rv.data
        assert '"ErrorMessage": "File could not be found"' in rv.data
        assert '"FileFound": false' in rv.data
        assert '"callResponse": 0' in rv.data

    def test_get_passive(self):
        """Test it does not find a mp3 file"""
        rv = self.app.get('/passive')
        print rv.data
        assert '"Success": true' in rv.data
        assert '"ErrorMessage": ""' in rv.data

        #
        # def login(self, username, password):
        #     return self.app.post('/login', data=dict(
        #         username=username,
        #         password=password
        #     ), follow_redirects=True)
        #
        # def logout(self):
        #     return self.app.get('/logout', follow_redirects=True)
        #
        # # testing functions
        #
        # def test_empty_db(self):
        #     """Start with a blank database."""
        #     rv = self.app.get('/')
        #     assert 'No entries here so far' in rv.data
        #
        # def test_login_logout(self):
        #     """Make sure login and logout works"""
        #     rv = self.login(flaskr.app.config['USERNAME'],
        #                     flaskr.app.config['PASSWORD'])
        #     assert 'You were logged in' in rv.data
        #     rv = self.logout()
        #     assert 'You were logged out' in rv.data
        #     rv = self.login(flaskr.app.config['USERNAME'] + 'x',
        #                     flaskr.app.config['PASSWORD'])
        #     assert 'Invalid username' in rv.data
        #     rv = self.login(flaskr.app.config['USERNAME'],
        #                     flaskr.app.config['PASSWORD'] + 'x')
        #     assert 'Invalid password' in rv.data
        #
        # def test_messages(self):
        #     """Test that messages work"""
        #     self.login(flaskr.app.config['USERNAME'],
        #                flaskr.app.config['PASSWORD'])
        #     rv = self.app.post('/add', data=dict(
        #         title='<Hello>',
        #         text='<strong>HTML</strong> allowed here'
        #     ), follow_redirects=True)
        #     assert 'No entries here so far' not in rv.data
        #     assert '&lt;Hello&gt;' in rv.data
        #     assert '<strong>HTML</strong> allowed here' in rv.data


if __name__ == '__main__':
    unittest.main()
