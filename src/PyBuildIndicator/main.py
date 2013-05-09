__author__ = 'rolf'
from flask import Flask
app = Flask(__name__)

print "wtf"

@app.route('/')
def index():
    return 'Hello World!'


if __name__ == '__main__':
    app.run(host='0.0.0.0')
