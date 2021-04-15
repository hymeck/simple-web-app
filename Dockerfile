# docker pull mcr.microsoft.com/dotnet/sdk:5.0 if does not have
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /source

COPY src/SimpleWebApp.sln .

COPY *.sln .

# Copy csproj and restore as distinct layers
COPY src/Application/Application.csproj ./Application/
COPY src/Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY src/WebApp/WebApp.csproj ./WebApp/


RUN dotnet restore ./Application/
RUN dotnet restore ./Infrastructure/
RUN dotnet restore ./WebApp/


# Copy everything else and build
COPY src/Application/. ./Application/
COPY src/Infrastructure/. ./Infrastructure/
COPY src/WebApp/. ./WebApp/


WORKDIR /source/WebApp
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebApp.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet WebApp.dll
