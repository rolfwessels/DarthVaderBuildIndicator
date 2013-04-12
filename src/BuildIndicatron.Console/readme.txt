Smaller linux dists
http://www.linuxsystems.it/2012/06/debian-wheezy-raspberry-pi-minimal-image/


ssh-keygen -t rsa
scp /c/Users/rolf/.ssh/id_rsa.pub pi@192.168.1.14:/home/pi/.ssh/authorized_keys


http://learn.adafruit.com/playing-sounds-and-using-buttons-with-raspberry-pi/install-audio
sudo apt-get install alsa-utils
sudo apt-get install mpg321
sudo modprobe snd_bcm2835
sudo amixer cset numid=3 1

http://www.techerator.com/2011/09/how-to-play-ogg-vorbis-files-from-the-linux-command-line/
sudo apt-get install vorbis-tools

ogg123 sample.ogg
