FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Poc.Generator", "Poc.Generator/"]
COPY ["Poc.Shared", "Poc.Shared/"]
RUN dotnet restore "Poc.Generator/Poc.Generator.csproj"
COPY . .
WORKDIR "/src/Poc.Generator"
RUN dotnet build "Poc.Generator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.Generator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poc.Generator.dll"]
