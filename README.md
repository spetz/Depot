# Depot is a sample distributed application (API + Service) using MongoDB, RabbitMQ, Redis and built with .NET Core.

In order to start the application type the following commands:

```
cd scripts
docker-compose build
./docker-run.sh
```

Or simply _dotnet run_ projects separately and make sure that MongoDB, RabbitMQ and Redis are running as services.

Depot API will be running at [http://localhost:5000](http://localhost:5000) and Entries Service at [http://localhost:5050](http://localhost:5050).