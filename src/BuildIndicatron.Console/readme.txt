Smaller linux dists
http://www.linuxsystems.it/2012/06/debian-wheezy-raspberry-pi-minimal-image/


rm c:/Users/rolf/.ssh/known_hosts
ssh-keygen -t rsa
rem on the server run mkdir ~/.ssh
scp /c/Users/rolf/.ssh/id_rsa.pub pi@192.168.1.14:/home/pi/.ssh/authorized_keys
ssh pi@192.168.1.14

http://learn.adafruit.com/playing-sounds-and-using-buttons-with-raspberry-pi/install-audio
sudo apt-get update
sudo apt-get -y install alsa-utils
sudo apt-get -y install mpg321
sudo modprobe snd_bcm2835
sudo amixer cset numid=3 1

http://www.techerator.com/2011/09/how-to-play-ogg-vorbis-files-from-the-linux-command-line/
sudo apt-get install vorbis-tools


Add python

sudo apt-get -y install python-dev
sudo apt-get -y install python-rpi.gpio
sudo apt-get -y install python-virtualenv
sudo pip install Flask

python serverStart.py

add mono
sudo apt-get install mono-complete