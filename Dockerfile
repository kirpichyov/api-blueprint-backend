FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY src .
RUN dotnet restore "ApiBlueprint.Api/ApiBlueprint.Api.csproj"
RUN dotnet build "ApiBlueprint.Api/ApiBlueprint.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiBlueprint.Api/ApiBlueprint.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish  /app/publish .
CMD dotnet dev-certs https --clean
CMD dotnet dev-certs https
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ApiBlueprint.Api.dll
