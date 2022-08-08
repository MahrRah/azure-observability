FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Poc.Processor", "Poc.Processor/"]
COPY ["Poc.Shared", "Poc.Shared/"]

RUN dotnet restore "Poc.Processor/Poc.Processor.csproj"
COPY . .
WORKDIR "/src/Poc.Processor"

RUN dotnet build "Poc.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.Processor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poc.Processor.dll"]