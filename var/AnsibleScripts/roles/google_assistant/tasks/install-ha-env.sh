sudo apt-get update
sudo apt-get install python3-dev python3-venv # Use python3.4-venv if the package cannot be found.
python3 -m venv env
env/bin/python -m pip install --upgrade pip setuptools
env/bin/python -m pip install --upgrade google-assistant-library
env/bin/python -m pip install --upgrade google-auth-oauthlib[tool]
#source env/bin/activate