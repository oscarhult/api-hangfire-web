FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
WORKDIR /src
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY --from=build /out /app
WORKDIR /app
EXPOSE 5555/tcp
ENTRYPOINT ["dotnet", "app.dll"]