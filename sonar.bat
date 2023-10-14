@echo off

set NAME_PROJECT="Test_Unit_VaxiDrez"
set HOST=http://localhost:9000
SET TOKEN=sqp_4b38aef8efaad25dcbb4c3d186f919a1dc52a157

dotnet sonarscanner begin /k:"%NAME_PROJECT%" /d:sonar.host.url="%HOST%" /d:sonar.token="%TOKEN%"

dotnet build

dotnet sonarscanner end /d:sonar.token="%TOKEN%"
