# RegisterAPI
Register Users


sudo docker ps
 -> check if cassandra is running

if not run docker-compose.yml
 -> docker-compose -f docker-compose.yml up

Setting up Keyspace and Table in Cassandra:
sudo docker exec -it cassandra-node-2 bash
 bash>  cqlsh
 cqlsh> use dev;
 
CREATE TABLE UserProfile(
UserId uuid PRIMARY KEY,
GCMClientId text,
ProfileName text,
ImageUrl text,
DeviceId text,
CountryCode text,
MobileNumber text,
CreatedOn timestamp,
IsDeleted boolean
);


Setting up API Code and build
run the build-registerapi.sh script




Setup Nginx:
sudo apt-get install nginx
sudo nano /etc/nginx/sites-enabled/default
paste the contents of nginx-conf file
sudo systemctl restart nginx



Invoking api/registration/

wget http://localhost:5000/register --no-check-certificate

wget -S --header="Accept-Encoding: gzip, deflate" --header='Accept-Charset: UTF-8' --header='Content-Type: application/json' -O response.json --post-data '{"GCMClientId":"D144F6A3-2970-4EF8-8DCD-C57BC70B21FC","ProfileName":"user9","ImageUrl":"NULL","IsDeleted":false,"CountryCode":"+1","MobileNumber":"484925958002","CreatedOn":"2020-01-06T17:16:40.000"}' http://localhost:5000/register --no-check-certificate
