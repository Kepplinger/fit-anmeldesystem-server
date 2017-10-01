# Microsoft SQL - Database in Docker
## - How to?
1. Download & Install Docker from Official Page
2. Set up 4GB RAM for Docker <b><u>important!!</u></b>
3. Check with "docker --version"
4. Pull Image with "docker pull microsoft/mssql-server-linux"
5. To run the container image with Docker, you can use the following command
######  1. Linux/Mac <br>
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=MyComplexPassword!234' -e 'MSSQL_PID=Developer' -p 1401:1433 --name sqlcontainer1 -d microsoft/mssql-server-linux
######  2. Windows PowerShell <br>
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>" -e "MSSQL_PID=Developer" -p 1401:1433 --name sqlcontainer1 -d microsoft/mssql-server-linux

6. Check logs of the run with "docker logs <containerName>
7. If finished connect to Database with <br><b>localhost:1401</b> <br><b>User: sa <br>Password: MyComplexPassword!234


<a href="https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker">-> Help</a>
