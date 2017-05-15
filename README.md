# Depot is a sample distributed app (API + Service) using MongoDB, RabbitMQ, Redis & built with .NET Core.

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/spetz/Depot.svg?branch=master)](https://travis-ci.org/spetz/Depot)
|develop            |[![develop branch build status](https://api.travis-ci.org/spetz/Depot.svg?branch=develop)](https://travis-ci.org/spetz/Depot/branches)


In order to start the application type the following commands:

```
cd scripts
docker-compose build
./docker-run.sh
```

Or simply _dotnet run_ projects separately and make sure that MongoDB, RabbitMQ and Redis are running as services.

Depot API will be running at [http://localhost:5000](http://localhost:5000) and Entries Service at [http://localhost:5050](http://localhost:5050).

For more information take a look at my [blog post](http://piotrgankiewicz.com/2017/05/15/depot-building-asp-net-core-distributed-application/).

## Usage

Create a new entry via HTTP request:

```
curl localhost:5000/entries -X POST -H "content-type: application/json" -d '{"key": "my-key", "value": "sample value"}'
```

Browse logs: [http://localhost:5000/logs](http://localhost:5000/logs)

Browse keys: [http://localhost:5050/entries](http://localhost:5050/entries)

Fetch value: [http://localhost:5050/entries/{key}](http://localhost:5050/entries/{key})
