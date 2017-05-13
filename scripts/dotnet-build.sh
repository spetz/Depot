#!/bin/bash
source=-"-source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/netcoretour/api/v3/index.json --no-cache"
cd ../src
projects=(Depot.Api Depot.Messages Depot.Services.Entries)
for project in ${projects[*]}
do
	echo ========================================================
	echo Building project: $project
	echo ========================================================
	dotnet build $project/$project.csproj
done

testprojects=(Depot.Tests Depot.Tests.EndToEnd)
cd ../tests
for project in ${testprojects[*]}
do
	echo ========================================================
	echo Building test project: $project
	echo ========================================================
	dotnet build $project/$project.csproj
done