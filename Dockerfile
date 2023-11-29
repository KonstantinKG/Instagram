FROM mcr.microsoft.com/dotnet/sdk:7.0 AS install-dotnet-ef
WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 7.0.14

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY --from=install-dotnet-ef /root/.dotnet/tools /root/.dotnet/tools

COPY . .
ENV PATH="${PATH}:/root/.dotnet/tools"

ENTRYPOINT ["sh", "./entrypoint.sh"]
