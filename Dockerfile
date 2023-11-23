FROM mcr.microsoft.com/dotnet/sdk:7.0

COPY . .

ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef --version 7.0

ENTRYPOINT ["sh", "./entrypoint.sh"]
