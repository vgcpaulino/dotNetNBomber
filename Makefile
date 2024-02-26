docker-build:
	@docker-compose build webapi

docker-start:
	@docker-compose up webapi --abort-on-container-exit

restore:
	@dotnet restore

build:
	@dotnet build ./webapi.sln
