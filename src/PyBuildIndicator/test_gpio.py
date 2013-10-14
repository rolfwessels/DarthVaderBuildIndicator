import select

from RPi import GPIO
from time import sleep
from unittest import TestCase
import unittest
from pins import *


class TestPassive(TestCase):
    def test_Passive(self):
        GPIO.setmode(GPIO.BCM)

        bRed = 17
        bGreen = 24

        GPIO.setup(bGreen, GPIO.OUT)
        GPIO.setup(bRed, GPIO.OUT)

        switch = False
        for count in range(0, 11):
            print (count)

            GPIOoutput(bGreen, switch)

            GPIOoutput(bRed, not switch)
            switch = not switch
            sleep(0.5)

        GPIOoutput(bGreen, False)
        GPIOoutput(bRed, False)

        assert True
        pass

    def test_RGB(self):

        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)

        GPIOoutput(lsBluePin, True)

        sleep(1.5)
        GPIOoutput(lsBluePin, False)
        GPIOoutput(lsGreenPin, True)
        sleep(1.5)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsRedPin, True)
        sleep(1.5)

        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)

        assert True
        pass


    def testButton(self):
        GPIO.setmode(GPIO.BCM)
        pin = 10
        GPIO.setup(pin, GPIO.IN)

        input_value = GPIO.input(pin)
        print 'Value:', input_value
        assert input_value
        pass


if __name__ == '__main__':
    unittest.main()
