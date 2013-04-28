from __future__ import with_statement
from RPi import GPIO
from flask import Flask, redirect, url_for, render_template, jsonify, Response, request, make_response
from functools import wraps
from multiprocessing import Lock
from responseModels import PingResponse, PlayMp3FileResponse, TextToSpeechResponse, SetupGpIoResponse, GpIoOutputResponse, PassiveResponse, EnqueueResponse, SetButtonChoreographyResponse, GetClipsResponse
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

app = Flask(__name__)

def add_response_headers(headers={}):
    """This decorator adds the headers passed in to the response"""
    def decorator(f):
        @wraps(f)
        def decorated_function(*args, **kwargs):
            resp = make_response(f(*args, **kwargs))
            h = resp.headers
            for header, value in headers.items():
                h[header] = value
            return resp
        return decorated_function
    return decorator


def noCache(f):
    @wraps(f)
    @add_response_headers({'Expires': 'Mon, 26 Jul 1997 05:00:00 GMT'})
    def decorated_function(*args, **kwargs):
        print "header"
        return f(*args, **kwargs)
    return decorated_function

@app.route('/')
@noCache
def show_index():
    error = "allgood"
    return render_template('index.html', error=error)

@app.route('/ping')
@noCache
def ping():
    return Response(PingResponse().Call(), mimetype='application/json')

@app.route('/playmp3file/<filename>')
@noCache
def playMp3File(filename):
    player = PlayMp3FileResponse()
    player.FileName = filename
    return Response(player.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/texttospeech/<text>')
@noCache
def  TextToSpeech(text):
    response = TextToSpeechResponse()
    response.Text = text
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/setupgpio/<pin>/<direction>')
@noCache
def setup_gpio(pin,direction):
    response = SetupGpIoResponse()
    response.pin = pin
    response.direction = direction
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/ouputgpio/<pin>/<ison>')
@noCache
def output_gpio(pin,ison):
    response = GpIoOutputResponse()
    response.pin = int(pin)
    print "ke > ",ison
    response.IsOn = ison in ['true', '1', 't', 'y', 'yes', 'yeah', 'TRUE', 'certainly', 'True']
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/passive')
@noCache
def get_passive():
    response = PassiveResponse()
    response.Passive = GlobalCurrentProcess.CurrentPassive
    return Response(response.Call(), mimetype='application/json')

@app.route('/passive', methods=['POST'])
@noCache
def set_passive():
    response = PassiveResponse()
    print request.json
    passive = Passive(request.json)
    GlobalCurrentProcess.SetCurrentPassive(passive)
    response.Passive = passive
    return Response(response.Call(), mimetype='application/json')

@app.route('/enqueue', methods=['POST'])
@noCache
def enqueue():
    response = EnqueueResponse()
    print request.json
    response.Choreography = Choreography(request.json)
    return Response(response.Call(GlobalCompositionRunner), mimetype='application/json')

@app.route('/setButtonChoreography', methods=['POST'])
@noCache
def setButtonChoreography():
    response = SetButtonChoreographyResponse()
    response.Choreography = []
    for cor in request.json:
        response.Choreography.append(Choreography(cor))

    GlobalButtonClickRunner.SetChoreography(response.Choreography)
    return Response(response.Call(), mimetype='application/json')

@app.route('/getClips')
@noCache
def getClips():
    response = GetClipsResponse()
    return Response(response.Call(), mimetype='application/json')

if __name__ == '__main__':
    sound = Choreography.SimpleChoreographyPlaySound("Start")
    GlobalCompositionRunner.AddChoreography(sound)
    app.run(host='0.0.0.0')
