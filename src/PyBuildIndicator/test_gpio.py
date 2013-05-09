import select

from RPi import GPIO
from time import sleep
from unittest import TestCase
import unittest


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

            GPIO.output(bGreen, switch)

            GPIO.output(bRed, not switch)
            switch = not switch
            sleep(0.5)

        GPIO.output(bGreen, False)
        GPIO.output(bRed, False)

        assert True
        pass

    def test_RGB(self):
        GPIO.setmode(GPIO.BCM)

        lsRed = 27 # Works
        lsGreen = 9 # nothing
        lsBlue = 11 # nothing

        GPIO.setup(lsBlue, GPIO.OUT)
        GPIO.setup(lsGreen, GPIO.OUT)
        GPIO.setup(lsRed, GPIO.OUT)

        GPIO.output(lsBlue, True)

        sleep(1.5)
        GPIO.output(lsBlue, False)
        GPIO.output(lsGreen, True)
        sleep(1.5)
        GPIO.output(lsGreen, False)
        GPIO.output(lsRed, True)
        sleep(1.5)

        GPIO.output(lsRed, False)
        GPIO.output(lsGreen, False)
        GPIO.output(lsBlue, False)

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
