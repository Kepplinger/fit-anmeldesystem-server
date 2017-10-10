# Microsoft SQL - Database in Docker
## - How to?
1. Download & Install Docker from Official Page
2. Set up 4GB RAM for Docker <b><u>important!!</u></b>
3. Check with "docker --version"
4. Pull Image with "docker pull microsoft/mssql-server-linux"
5. To run the container image with Docker, you can use the following command
######  1. Linux/Mac <br>
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=MyComplexPassword!234'  -p 1433:1433 -d microsoft/mssql-server-linux
######  2. Windows PowerShell <br>
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyComplexPassword!234" -p 1433:1433 -d microsoft/mssql-server-linux

6. Check logs of the run with "docker logs + first 3 numbers of hash""
7. If finished connect to Database with <br><b>localhost:1433</b> <br><b>User: sa <br>Password: MyComplexPassword!234


<a href="https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker">-> Help</a>
