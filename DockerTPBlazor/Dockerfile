#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DockerTPBlazor/DockerTPBlazor.csproj", "DockerTPBlazor/"]
RUN dotnet restore "DockerTPBlazor/DockerTPBlazor.csproj"
COPY . .
WORKDIR "/src/DockerTPBlazor"
RUN dotnet build "DockerTPBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerTPBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerTPBlazor.dll"]