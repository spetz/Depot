#!/bin/bash
source=-"-source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/netcoretour/api/v3/index.json --no-cache"
cd ../src
projects=(Depot.Api Depot.Services.Entries)
for project in ${projects[*]}
do
	echo ========================================================
	echo Restoring packages for project: $project
	echo ========================================================
	dotnet restore $project $source
done

testprojects=(Depot.Tests Depot.Tests.EndToEnd)
cd ../tests
for project in ${testprojects[*]}
do
	echo ========================================================
	echo Restoring packages for test project: $project
	echo ========================================================
	dotnet restore $project $source
done