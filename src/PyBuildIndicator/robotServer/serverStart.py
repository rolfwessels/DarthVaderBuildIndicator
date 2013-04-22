from __future__ import with_statement
from RPi import GPIO
from flask import Flask, redirect, url_for, render_template, jsonify, Response, request
from multiprocessing import Lock
from responseModels import PingResponse, PlayMp3FileResponse, TextToSpeechResponse, SetupGpIoResponse, GpIoOutputResponse, PassiveResponse, EnqueueResponse, SetButtonChoreographyResponse
from robotServer.CompositionRunner import CompositionRunner
from robotServer.backgroundProcess import PassiveManager

# configuration
from robotServer.buttonClickRunner import buttonClickRunner
from robotServer.models import Passive, Choreography

DEBUG = True
MP3PATH = "Resources/mp3"
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

# globals
GlobalCompositionRunner = CompositionRunner()
GlobalCurrentProcess = PassiveManager(GlobalCompositionRunner)
GlobalButtonClickRunner = buttonClickRunner(buttonPin,GlobalCompositionRunner)

# create our little application :)
app = Flask(__name__)
app.config.from_object(__name__)
app.config.from_envvar('FLASKR_SETTINGS', silent=True)

@app.route('/')
def show_index():
    error = "allgood"
    return render_template('index.html', error=error)

@app.route('/ping')
def ping():
    return Response(PingResponse().Call(), mimetype='application/json')

@app.route('/playmp3file/<filename>')
def playMp3File(filename):
    player = PlayMp3FileResponse()
    player.FileName = filename
    return Response(player.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/TextToSpeech/<text>')
def  TextToSpeech(text):
    response = TextToSpeechResponse()
    response.Text = text
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/setupgpio/<pin>/<direction>')
def setup_gpio(pin,direction):
    response = SetupGpIoResponse()
    response.pin = pin
    response.direction = direction
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/ouputgpio/<pin>/<ison>')
def output_gpio(pin,ison):
    response = GpIoOutputResponse()
    response.pin = int(pin)
    print "ke > ",ison
    response.IsOn = ison in ['true', '1', 't', 'y', 'yes', 'yeah', 'TRUE', 'certainly', 'True']
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/passive')
def get_passive():
    response = PassiveResponse()
    response.Passive = GlobalCurrentProcess.CurrentPassive
    return Response(response.Call(), mimetype='application/json')

@app.route('/passive', methods=['POST'])
def set_passive():
    response = PassiveResponse()
    print request.json
    passive = Passive(request.json)
    GlobalCurrentProcess.SetCurrentPassive(passive)
    response.Passive = passive
    return Response(response.Call(), mimetype='application/json')

@app.route('/enqueue', methods=['POST'])
def enqueue():
    response = EnqueueResponse()
    print request.json
    response.Choreography = Choreography(request.json)
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/setButtonChoreography', methods=['POST'])
def setButtonChoreography():
    response = SetButtonChoreographyResponse()
    response.Choreography = Choreography(request.json)
    GlobalButtonClickRunner.SetChoreography(response.Choreography)
    return Response(response.Call(), mimetype='application/json')


if __name__ == '__main__':

    app.run(host='0.0.0.0')


