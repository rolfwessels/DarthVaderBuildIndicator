from RPi import GPIO

__author__ = 'rolf'

GPIO.setmode(GPIO.BOARD)

bRedPin = 17
bGreenPin = 24
lsRedPin = 27
lsGreenPin = 9
lsBluePin = 11
buttonPin = 10

GPIO.setmode(GPIO.BCM)
GPIO.setup(bRedPin, GPIO.OUT)
GPIO.setup(bGreenPin, GPIO.OUT)
GPIO.setup(lsBluePin, GPIO.OUT)
GPIO.setup(lsGreenPin, GPIO.OUT)
GPIO.setup(lsRedPin, GPIO.OUT)
GPIO.setup(buttonPin, GPIO.IN)