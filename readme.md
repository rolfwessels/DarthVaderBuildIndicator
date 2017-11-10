[![Build Status](https://travis-ci.org/rolfwessels/DarthVaderBuildIndicator.svg?branch=master)](https://travis-ci.org/rolfwessels/DarthVaderBuildIndicator)

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

