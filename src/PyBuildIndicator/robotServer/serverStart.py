from __future__ import with_statement
from flask import Flask, redirect, url_for, render_template, jsonify, Response, request
from responseModels import PingResponse, PlayMp3FileResponse, TextToSpeechResponse, SetupGpIoResponse, GpIoOutputResponse, PassiveResponse
from robotServer.CompositionRunner import CompositionRunner
from robotServer.backgroundProcess import PassiveManager

# configuration
from robotServer.models import Passive

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
    response.text = text
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
    response.IsOn = bool(ison)
    return Response(response.Call(), mimetype='application/json')

@app.route('/passive')
def get_passive():
    response = PassiveResponse()
    response.Passive = GlobalCurrentProcess.CurrentPassive
    return Response(response.Call(), mimetype='application/json')

@app.route('/passive', methods=['POST'])
def set_passive():
    response = PassiveResponse()
    passive = Passive(**request.json)
    GlobalCurrentProcess.SetCurrentPassive(passive)
    response.Passive = passive
    return Response(response.Call(), mimetype='application/json')

if __name__ == '__main__':
    app.run(host='0.0.0.0')
