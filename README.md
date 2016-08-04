# DarthVaderBuildIndicator

Hacked darth vader led light used with RaspberryPi for build indicator https://www.youtube.com/watch?v=BnXh99us4-A

If you need more information. Please feel free to contact me.


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