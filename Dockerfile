ARG IMAGE=mcr.microsoft.com/dotnet/
ARG SDK_TAG=sdk:8.0
ARG RUNTIME_TAG=aspnet:8.0

FROM ${IMAGE}${SDK_TAG} as base
# Change the working dir folder:
WORKDIR /application
# Copy Solution and Project files:
COPY WebApi.sln WebApi.sln
COPY WebApi/WebApi.csproj WebApi/WebApi.csproj
COPY WebApi.Tests/WebApi.Tests.csproj WebApi.Tests/WebApi.Tests.csproj
# Restore packages:
RUN  dotnet restore 
# Copy source files:
COPY . .
# Build Solution:
RUN dotnet build

# Publish the App:
FROM base as publish
RUN dotnet publish WebApi/WebApi.csproj -o /application/dist

# Generate the Production image with the published files:
FROM ${IMAGE}${RUNTIME_TAG} as webapi
ENV ASPNETCORE_URLS=http://+:7117
# Change the working dir folder:
WORKDIR /application
# Copy required files:
COPY ./WebApi/Data/ /application/data/
COPY --from=publish /application/dist /application
CMD ["./WebApi"]
