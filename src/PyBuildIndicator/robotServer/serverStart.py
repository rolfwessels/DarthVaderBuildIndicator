from __future__ import with_statement
from RPi import GPIO
from flask import Flask, redirect, url_for, render_template, jsonify, Response, request
from responseModels import PingResponse, PlayMp3FileResponse, TextToSpeechResponse, SetupGpIoResponse, GpIoOutputResponse, PassiveResponse, EnqueueResponse
from robotServer.CompositionRunner import CompositionRunner
from robotServer.backgroundProcess import PassiveManager

# configuration
from robotServer.models import Passive, Choreography

DEBUG = True
MP3PATH = "Resources/mp3"

# globals
GlobalCompositionRunner = CompositionRunner()
GlobalCurrentProcess = PassiveManager(GlobalCompositionRunner)


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
    return Response(response.Call(), mimetype='application/json')

@app.route('/ouputgpio/<pin>/<ison>')
def output_gpio(pin,ison):
    response = GpIoOutputResponse()
    response.pin = int(pin)
    print "ke > ",ison
    response.IsOn = ison in ['true', '1', 't', 'y', 'yes', 'yeah', 'TRUE', 'certainly', 'True']
    return Response(response.Call(), mimetype='application/json')

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

if __name__ == '__main__':
    GPIO.setmode(GPIO.BCM)
    GPIO.setup(25, GPIO.OUT)
    GPIO.setup(23, GPIO.OUT)
    app.run(host='0.0.0.0')


