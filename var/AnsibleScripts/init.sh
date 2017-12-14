ssh-copy-id pi@192.168.1.102 && \
ansible-playbook init-add-python.yml -i hosts --limit 192.168.1.102 --ask-sudo-pass && \
ansible-playbook init-wheel-user.yml -i hosts --limit 192.168.1.102 --ask-sudo-pass