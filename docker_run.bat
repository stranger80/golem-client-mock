REM Make sure your Virtual host (eg. VirtualBox) has a port forwarding rule from 127.0.0.1:5001 to guest port 5001
docker run -d -p 5001:5001 --name testbed golem-client-mock 