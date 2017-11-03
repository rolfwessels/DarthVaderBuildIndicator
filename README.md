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
sudo apt-get -y install python-pip python-dev libffi-dev libssl-dev
pip install ansible --user
echo 'PATH=$HOME/.local/bin:$PATH' >> ~/.bashrc
```

What to install on the raspberry pi
```
curl -sSL https://get.docker.com | sh

sudo docker run -d --name=portainer --privileged --restart=unless-stopped -p 9000:9000 -v /data/portainer:/data -v /var/run/docker.sock:/var/run/docker.sock portainer/portainer:linux-arm
docker run --rm richlander/dotnetapp-prod-arm32 Hello .NET Core from Docker