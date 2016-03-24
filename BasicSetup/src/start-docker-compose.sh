#!/bin/bash

# get ip address from this docker host
HOSTIP=$(ip -f inet -o addr show eth0|cut -d\  -f 7 | cut -d/ -f 1)
# run docker-compose with patched docker-compose file  containing the docker host ip address instead of the placeholder
sed -e "s/REPLACE_WITH_DOCKERHOSTIP/$HOSTIP/g" docker-compose.yml | docker-compose --file - up -d