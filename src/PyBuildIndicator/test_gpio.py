import select

from RPi import GPIO
from time import sleep
from unittest import TestCase
import unittest
from pins import *


class TestPassive(TestCase):

    def test_shoot(self):


        sleep(0.5)
        print 'on'
        GPIOoutput(pinSpin, True)
        sleep(3)
        print 'fire'
        GPIOoutput(pinFire, True)

        sleep(1)
        print 'off'
        GPIOoutput(pinFire, False)
        sleep(0.5)
        GPIOoutput(pinSpin, False)




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


    def test_SinglePin(self):

        pin = lsGreenPin

        #
        # GPIOoutput(lsRedPin, False)
        # GPIOoutput(lsGreenPin, False)
        # GPIOoutput(lsBluePin, False)
        print 'off'

        GPIOoutput(pin, True)
        sleep(3)
        GPIOoutput(pin, False)
        pass

    def test_RGB(self):

        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)
        print 'off'
        GPIOoutput(lsBluePin, True)
        print 'blue'
        sleep(1.5)
        GPIOoutput(lsBluePin, False)
        GPIOoutput(lsGreenPin, True)
        print 'green'
        sleep(1.5)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsRedPin, True)
        print 'red'
        sleep(1.5)

        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)
        print 'off'
        assert True
        pass


    def testButton(self):
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(buttonPin, GPIO.IN)

        input_value = GPIO.input(buttonPin)
        print 'Value:', input_value
        assert input_value
        pass


if __name__ == '__main__':
    unittest.main()
