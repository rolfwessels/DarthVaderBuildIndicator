Smaller linux dists
http://www.linuxsystems.it/2012/06/debian-wheezy-raspberry-pi-minimal-image/


rm c:/Users/rolf/.ssh/known_hosts
ssh-keygen -t rsa
rem on the server run mkdir ~/.ssh
scp /c/Users/rolf/.ssh/id_rsa.pub pi@192.168.1.14:/home/pi/.ssh/authorized_keys
ssh pi@192.168.1.14

http://learn.adafruit.com/playing-sounds-and-using-buttons-with-raspberry-pi/install-audio
sudo apt-get -y remove --auto-remove --purge libx11-.*
sudo apt-get -y update
sudo apt-get -y upgrade


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


wget http://downloads.sourceforge.net/project/sox/sox/14.4.1/sox-14.4.1.tar.bz2
tar jxf sox-14.4.1.tar.bz2
rm sox-14.4.1.tar.bz2
cd sox-14.4.1
./configure
make -s && make install

sudo apt-get -y install libsox-fmt-mp3 sox


sudo apt-get -y install git-core
sudo wget http://goo.gl/1BOfJ -O /usr/bin/rpi-update && sudo chmod +x /usr/bin/rpi-update
sudo rpi-update


sudo apt-get remove desktop-base lightdm lxappearance lxde-common lxde-icon-theme lxinput lxpanel lxpolkit lxrandr lxsession-edit lxshortcut lxtask lxterminal obconf openbox raspberrypi-artwork xarchiver xinit xserver-xorg xserver-xorg-video-fbdev