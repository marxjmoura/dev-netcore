# Developing with .NET Core

How I build a RESTful API with .NET Core and PostgreSQL.

This project can be run with [Docker](#how-to-run-with-docker) or [locally](#how-to-run-locally).

See the [how to use](#how-to-use) section for API documentation.

## Status

[![CircleCI](https://circleci.com/gh/marxjmoura/dev-netcore/tree/master.svg?style=shield)](https://circleci.com/gh/marxjmoura/dev-netcore/tree/master)
[![codecov](https://codecov.io/gh/marxjmoura/dev-netcore/branch/master/graph/badge.svg)](https://codecov.io/gh/marxjmoura/dev-netcore)

## How to run with Docker

Run the following commands from the project's root directory.

1. Build the Docker image.

```bash
docker build -t marxjmoura/dev-netcore/api:1.0.0 -f src/Developing.API/Dockerfile .
```

2. Run the containers.

```bash
# Create a network
docker network create --subnet=172.18.0.0/16 development
```

```bash
# Run PostgreSQL container
docker run -d \
  --name postgres \
  --hostname postgres \
  --network=development \
  --restart=always \
  -p 5432:5432 \
  -e POSTGRES_DB=development \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  postgres:11.6-alpine
```

```bash
# Run API container
docker container run -d \
  --name dev-netcore-api \
  --hostname dev-netcore-api \
  --network=development \
  --restart=always \
  -p 5000:80 \
  -e ASPNETCORE_AllowedHosts="*" \
  -e ASPNETCORE_ConnectionString="Server=postgres; Port=5432; Database=development; User Id=postgres; Password=postgres;" \
  -e ASPNETCORE_JWT__Issuer="http://api.local/" \
  -e ASPNETCORE_JWT__Audience="http://client.local/" \
  -e ASPNETCORE_JWT__Secret="87c10446-aa6a-4df3-8615-d4302cd205fb" \
  -e ASPNETCORE_Logging_LogLevel__Default="Warning" \
  marxjmoura/dev-netcore/api:1.0.0
```

```bash
# Run migrations
docker cp dev-netcore-api:/app/migrate.sql migrate.sql &&
  docker cp migrate.sql postgres:/migrate.sql &&
  docker exec -it postgres psql -U postgres -d development -f migrate.sql &&
  rm migrate.sql
```

> The data is kept in the container for demonstration purposes only.

The API can be accessed at [localhost:5000](http://localhost:5000).

## How to debug locally

1. Install [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1).

2. Install [PostgreSQL](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads) (version 11.6).

> Keep the default user `postgres` and password `postgres`.  
> You can also change the connection string in the `appsettings.json` file.

3. Install the Entity Framework tool.

```bash
dotnet tool install --global dotnet-ef --version 3.1.2
```

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

4. Run the migrations from directory `src/Developing.API`.

```bash
dotnet ef database update
```

5. Still in the `src/Developing.API` directory, run the API.

```bash
dotnet run
```

The API can be accessed at [localhost:5000](http://localhost:5000).

## How to run the tests

Run the following commands from `src/Developing.Tests` directory.

To run the tests:

```bash
dotnet test \
  /p:AltCover="true" \
  /p:AltCoverForce="true" \
  /p:AltCoverThreshold="80" \
  /p:AltCoverOpenCover="true" \
  /p:AltCoverXmlReport="coverage/opencover.xml" \
  /p:AltCoverInputDirectory="src/Developing.API" \
  /p:AltCoverAttributeFilter="ExcludeFromCodeCoverage" \
  /p:AltCoverAssemblyExcludeFilter="System(.*)|xunit|src/Developing.Tests|src/Developing.API.Views"
```

And to generate the coverage report:

```bash
dotnet reportgenerator \
  "-reports:coverage/opencover.xml" \
  "-reporttypes:Html;HtmlSummary" \
  "-targetdir:coverage/report"
```

The generated report can be accessed at `src/Developing.Tests/coverage/report/index.html`.

## How to use

The API consists of a vehicle catalog service maintaining three resources: brands, models and vehicles.

There are two access levels: `PUBLIC` and `ADMIN` (requires an access token).
For `ADMIN` access use the following content in the authorization request header:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwOi8vYXBpLmxvY2FsLyIsImF1ZCI6Imh0dHA6Ly9jbGllbnQubG9jYWwvIn0.83rGjbSN-2V5dK7OEWdR8NbMajHJjSjaHAew4q3CGQw
```

Brands:

| Method   | Path             | Access level    | Description                                |
| :------: | ---------------- | :-------------: | ------------------------------------------ |
| `POST`   | `/brands`        | `ADMIN`         | Create a new brand.                        |
| `GET`    | `/brands/{id}`   | `ADMIN`         | Find a brand by ID.                        |
| `PUT`    | `/brands/{id}`   | `ADMIN`         | Update a brand.                            |
| `DELETE` | `/brands/{id}`   | `ADMIN`         | Delete a brand.                            |
| `GET`    | `/brands`        | `PUBLIC`        | List all brands ordered by name ascending. |

Request body for POST and PUT:

```json
{
  "name": "Ford"
}
```

Models:

| Method   | Path                     | Access level    | Description                                                               |
| :------: | ------------------------ | :-------------: | ------------------------------------------------------------------------- |
| `POST`   | `/models`                | `ADMIN`         | Create a new model.                                                       |
| `GET`    | `/models/{id}`           | `ADMIN`         | Find a model by ID.                                                       |
| `PUT`    | `/models/{id}`           | `ADMIN`         | Update a model.                                                           |
| `DELETE` | `/models/{id}`           | `ADMIN`         | Delete a model.                                                           |
| `GET`    | `/models?brandId={id}`   | `PUBLIC`        | List all models ordered by name ascending. Optionally filter by brand ID. |

Request body for POST and PUT:

```json
{
  "brandId": 1,
  "name": "Mustang Boss 429 Fastback"
}
```

Vehicles:

| Method   | Path                       | Access level    | Description                                                                  |
| :------: | -------------------------- | :-------------: | ---------------------------------------------------------------------------- |
| `POST`   | `/vehicles`                | `ADMIN`         | Create a new vehicle.                                                        |
| `GET`    | `/vehicles/{id}`           | `ADMIN`         | Find a vehicle by ID.                                                        |
| `PUT`    | `/vehicles/{id}`           | `ADMIN`         | Update a vehicle.                                                            |
| `DELETE` | `/vehicles/{id}`           | `ADMIN`         | Delete a vehicle.                                                            |
| `GET`    | `/vehicles?modelId={id}`   | `PUBLIC`        | List all vehicles ordered by value ascending. Optionally filter by model ID. |

Request body for POST and PUT:

```json
{
  "modelId": 1,
  "modelYear": 1969,
  "value": 21063.22,
  "fuel": "Gasolina"
}
```
