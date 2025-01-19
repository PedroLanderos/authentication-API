docker build -f AuthenticationApi.Solution\AuthenticationApi.Presentation\Dockerfile -t authenticationapi .

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=password" -p 1434:1433 --name sqlserver-container -d mcr.microsoft.com/mssql/server:2022-latest

