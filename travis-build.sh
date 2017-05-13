#!/bin/bash

PROJECTS=(src/Depot.Api src/Depot.Services.Entries tests/Depot.Tests tests/Depot.Tests.EndToEnd)
for PROJECT in ${PROJECTS[*]}
do
  dotnet restore $PROJECT -source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/netcoretour/api/v3/index.json --no-cache
  dotnet build $PROJECT
done


