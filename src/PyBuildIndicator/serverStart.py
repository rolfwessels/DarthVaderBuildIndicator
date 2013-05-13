from __future__ import with_statement
from flask import Flask, render_template, Response, request, make_response
from functools import wraps
import thread
from responseModels import PingResponse, PlayMp3FileResponse, TextToSpeechResponse, SetupGpIoResponse, GpIoOutputResponse, PassiveResponse, EnqueueResponse, SetButtonChoreographyResponse, GetClipsResponse
from CompositionRunner import CompositionRunner
from backgroundProcess import PassiveManager

# configuration
from buttonClickRunner import buttonClickRunner
from models import Passive, Choreography
from pins import *
from twitterListner import TwitterListener

DEBUG = True
MP3PATH = "resources/mp3"

# globals

GlobalTwitterCommunication = TwitterListener()
GlobalCompositionRunner = CompositionRunner()
GlobalCurrentProcess = PassiveManager(GlobalCompositionRunner)
GlobalButtonClickRunner = buttonClickRunner(buttonPin,GlobalCompositionRunner)

GlobalCompositionRunner.SetRepeatComposition(GlobalButtonClickRunner.SetLastComposition)
thread.start_new_thread(GlobalTwitterCommunication.GetTimeLineSteam, (GlobalCompositionRunner,))

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
        return f(*args, **kwargs)
    return decorated_function
def noCache(f):
    @wraps(f)
    @add_response_headers({'Expires': 'Mon, 26 Jul 1997 05:00:00 GMT'})
    def decorated_function(*args, **kwargs):
        return f(*args, **kwargs)
    return decorated_function

@app.route('/')
@noCache
def show_index():
    error = "allgood"
    return render_template('index.html', error=error)

@app.route('/api/json')
def show_json():
    f = open('static/jsonsample', 'r')
    return Response(f.read(), mimetype='application/json')

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
    choreography = []
    for cor in request.json:
        choreography.append(Choreography(cor))
    GlobalButtonClickRunner.SetChoreography(choreography)
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
