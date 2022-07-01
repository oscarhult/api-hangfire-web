FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
RUN dotnet publish /src -c Release -r linux-musl-x64 --sc false -o /out -p:PublishSingleFile=true -p:DebugSymbols=false -p:DebugType=None

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY --from=build /out .
ENTRYPOINT ["./app"]
