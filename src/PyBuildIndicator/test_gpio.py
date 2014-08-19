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


        switch = False
        for count in range(0, 11):
            print (count)

            GPIOoutput(bGreenPin, switch)
            GPIOoutput(bRedPin, not switch)
            switch = not switch
            sleep(0.5)

        GPIOoutput(bGreenPin, False)
        GPIOoutput(bRedPin, False)

        assert True
        pass


    def test_SinglePin(self):

        pin = bGreenPin

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


        delay = 3.5
        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)
        print 'off'
        GPIOoutput(lsBluePin, True)
        print 'blue'

        sleep(delay)
        GPIOoutput(lsBluePin, False)
        GPIOoutput(lsGreenPin, True)
        print 'green'
        sleep(delay)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsRedPin, True)
        print 'red'
        sleep(delay)

        GPIOoutput(lsRedPin, False)
        GPIOoutput(lsGreenPin, False)
        GPIOoutput(lsBluePin, False)


        GPIOoutput(bGreenPin, True)
        print 'bGreenPin'
        sleep(delay)
        GPIOoutput(bGreenPin, False)

        GPIOoutput(bRedPin, True)
        print 'bRedPin'
        sleep(delay)
        GPIOoutput(bRedPin, False)

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
