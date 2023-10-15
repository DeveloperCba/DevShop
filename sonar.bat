@echo off

set NAME_PROJECT="DevShop"
set HOST=http://localhost:8000
SET TOKEN=sqp_b277291f5a600e02ef39395513d6ea00897c613d

dotnet sonarscanner begin /k:"%NAME_PROJECT%" /d:sonar.host.url="%HOST%" /d:sonar.token="%TOKEN%"

dotnet build

dotnet sonarscanner end /d:sonar.token="%TOKEN%"
