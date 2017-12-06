# FIT-Anmeldesystem-Backend

![htlleondinglogo](./images/htlleondinglogo.png)
<br>
![absedv_logo](./images/absedv_logo.png)

## Developer and Technical advice

### Branches and Developer:

- <b>Andrej Sakal:</b> Sakal-Dev
- <b>Felix Hofmann:</b> Hofmann-Dev

### API and Swagger:

- [http://localhost:8080/api/{controller}](http://localhost:8080/api/{controller})
- [http://localhost:8080/swagger](http://localhost:8080/swagger)

### Microsoft SQL - Database in Docker

#### How to?

1. Download & Install Docker from Official Page
2. Set up 4GB RAM for Docker <b><u>important!!</u></b>
3. Check with "docker --version"
4. Pull Image with "docker pull microsoft/mssql-server-linux"
5. To run the container image with Docker, you can use the following command

###### 1. Linux/Mac <br>

docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=MyComplexPassword!234'  -p 1433:1433 -d microsoft/mssql-server-linux

###### 2. Windows PowerShell <br>

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyComplexPassword!234" -p 1433:1433 -d microsoft/mssql-server-linux

1. Check logs of the run with "docker logs + first 3 numbers of hash""
2. If finished connect to Database with <br><b>localhost:1433</b> <br><b>Database: Backend<br><b>User: sa <br>Password: MyComplexPassword!234

<a href="https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker">-> Help</a>



## Technologies

![csharp-e7b8fcd4ce](./images/csharp-e7b8fcd4ce.png)
![docker_twitter_share](./images/docker_twitter_share.png)
![mssql-server](./images/mssql-server.png)
![net-core-logo-proposal](./images/net-core-logo-proposal.jpg)
![restfulapi](./images/restfulapi.jpg)
![windows-server-2016](./images/windows-server-2016.png)

## Documents



<a href="https://www.dropbox.com/s/dvcypwakozlgwse/FITLOG_Pflichtenheft.docx?dl=0">Duty stapler</a>

![FIT_ERD](./images/FIT_ERD.png)
