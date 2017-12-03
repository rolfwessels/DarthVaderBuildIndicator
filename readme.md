[![Build Status](https://travis-ci.org/rolfwessels/DarthVaderBuildIndicator.svg?branch=master)](https://travis-ci.org/rolfwessels/DarthVaderBuildIndicator)

[![Docker Pulls](https://img.shields.io/docker/pulls/rolfwessels/buildindicator.svg)](https://hub.docker.com/r/rolfwessels/buildindicator/)

[![Microbadger](https://images.microbadger.com/badges/rolfwessels/buildindicator.svg)](http://microbadger.com/images/rolfwessels/buildindicator "Image size")

# DarthVaderBuildIndicator

Hacked darth vader led light used with RaspberryPi for build indicator https://www.youtube.com/watch?v=BnXh99us4-A

If you need more information. Please feel free to contact me.

# new ansible install

```
ssh-copy-id pi@192.168.1.249
#add to host files
./init.sh
```

# new install.
```
# suggest to change default password - Change `sudo raspi-config`

sudo apt-get -y update
sudo apt-get -y upgrade

#setup some audio

sudo apt-get -y install alsa-utils mpg321 libsox-fmt-mp3 sox
sudo modprobe snd_bcm2835
sudo amixer cset numid=3 1
amixer cset numid=1 -- 95%

# mono

sudo apt-get install -y mono-complete

```

# helping with deployment

Add ansible (windows bash)
https://www.jeffgeerling.com/blog/2017/using-ansible-through-windows-10s-subsystem-linux
```
sudo apt-get update -y
sudo apt-get upgrade -y
sudo apt-get -y install python-pip python-dev libffi-dev libssl-dev
sudo pip install ansible --user
echo 'PATH=$HOME/.local/bin:$PATH' >> ~/.bashrc
```

Adding a new host SSH access
```
export newhost=pi@192.168.1.249
ssh-keygen -t rsa
ssh $newhost mkdir -p .ssh
cat ~/.ssh/id_rsa.pub | ssh $newhost 'cat >> .ssh/authorized_keys'
ssh $newhost 

```

```
ssh-copy-id chip@192.168.1.248 .. to copy the id to the service
```

# Run beta images
```
sudo docker run --rm  -p 5000:5000 rolfwessels/buildindicator:beta


```


# enable google assisant raspberry pi

```
sudo apt-get install -y fswebcam python-pip git
cat /proc/asound/cards
arecord -l
lsusb

scp client_secret_1007341124689-lia9ap7qrkqme2iqkuisgv6k13jtnakr.apps.googleusercontent.com.json pi@192.168.1.102:/home/pi/
git clone https://github.com/googlesamples/assistant-sdk-python
cp -r assistant-sdk-python/google-assistant-sdk/googlesamples/assistant/grpc raome
python -m pip install --upgrade google-auth-oauthlib[tool]
google-oauthlib-tool --client-secrets /home/pi/client_secret_1007341124689-lia9ap7qrkqme2iqkuisgv6k13jtnakr.apps.googleusercontent.com.json --scope https://www.googleapis.com/auth/assistant-sdk-prototype --save --headless 
```

# adding bluetooth to chip

https://bbs.nextthing.co/t/guide-to-connecting-to-a-bluetooth-speaker/4684

```
sudo apt-get update && sudo apt-get install -y bluez-tools pulseaudio-module-bluetooth unzip wget
bt-adapter -d
bt-device --set 12:00:00:00:06:EA Trusted 1
{ sleep 1; echo "connect 12:00:00:00:06:EA"; sleep 10; } | bluetoothctl
pactl load-module module-bluetooth-discover && pactl load-module module-bluetooth-policy && pactl load-module module-switch-on-connect
pactl list short sinks
```