from RPi import GPIO
from pygame.tests.test_utils import unittest
from time import sleep
from unittest import TestCase


class TestPassive(TestCase):

    def test_Passive(self):
        GPIO.setmode(GPIO.BCM)

        red = 25
        green = 23

        GPIO.setup(green, GPIO.OUT)
        GPIO.setup(red, GPIO.OUT)

        switch = False
        for count in range(0,11):
            print (count)

            GPIO.output(green, switch)

            GPIO.output(red, not switch)
            switch = not switch
            sleep(0.5)


        assert False
        pass




if __name__ == '__main__':
    unittest.main()
