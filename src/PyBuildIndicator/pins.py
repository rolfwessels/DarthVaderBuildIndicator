from RPi import GPIO

__author__ = 'rolf'

GPIO.setmode(GPIO.BOARD)

bRedPin = 7
bGreenPin = 8
lsRedPin = 11
lsGreenPin = 25
lsBluePin = 9
buttonPin = 24

GPIO.setmode(GPIO.BCM)
GPIO.setup(bRedPin, GPIO.OUT)
GPIO.setup(bGreenPin, GPIO.OUT)
GPIO.setup(lsBluePin, GPIO.OUT)
GPIO.setup(lsGreenPin, GPIO.OUT)
GPIO.setup(lsRedPin, GPIO.OUT)


def GPIOoutput(Pin,ins):
    InversePins = {lsBluePin, lsGreenPin , lsRedPin, bGreenPin, bRedPin}
    if Pin in InversePins :  GPIO.output(Pin,ins == 0 or ins == False)
    else : GPIO.output(Pin,ins)

GPIOoutput(lsBluePin,False)
GPIOoutput(lsGreenPin,False)
GPIOoutput(lsRedPin, False)


GPIO.setup(buttonPin, GPIO.IN)


