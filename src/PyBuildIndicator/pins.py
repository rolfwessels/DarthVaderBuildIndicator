from RPi import GPIO

__author__ = 'rolf'

GPIO.setmode(GPIO.BOARD)

bRedPin = 7
bGreenPin = 8
lsRedPin = 11
lsGreenPin = 25
lsBluePin = 9
buttonPin = 24
pinSpin = 22
pinFire = 10

isButtonSwitch = False

GPIO.setmode(GPIO.BCM)
GPIO.setup(bRedPin, GPIO.OUT)
GPIO.setup(bGreenPin, GPIO.OUT)
GPIO.setup(lsBluePin, GPIO.OUT)
GPIO.setup(lsGreenPin, GPIO.OUT)
GPIO.setup(lsRedPin, GPIO.OUT)
GPIO.setup(pinSpin, GPIO.OUT)
GPIO.setup(pinFire, GPIO.OUT)

def GPIOoutput(Pin,ins):
    print 'set pin ', Pin ,' ' ,ins
    InversePins = {lsBluePin,lsGreenPin, lsRedPin, bGreenPin, bRedPin}
    if Pin in InversePins :  GPIO.output(Pin,ins == 0 or ins == False)
    else : GPIO.output(Pin,ins)

GPIOoutput(lsBluePin,False)
GPIOoutput(lsGreenPin,False)
GPIOoutput(lsRedPin, False)
GPIOoutput(bRedPin, False)
GPIOoutput(bGreenPin, False)
GPIOoutput(pinSpin, False)
GPIOoutput(pinFire, False)
GPIO.setup(buttonPin, GPIO.IN)


