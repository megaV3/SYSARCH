FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy only the project file first to cache NuGet layers
COPY ["Villarin_SYSARCH/Villarin_SYSARCH.csproj", "Villarin_SYSARCH/"]
RUN dotnet restore "Villarin_SYSARCH/Villarin_SYSARCH.csproj"

# 2. Copy the rest of the source code only after restoring
COPY . .

# 3. Build and publish
RUN dotnet publish "Villarin_SYSARCH/Villarin_SYSARCH.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# 4. Critical settings for Render port mapping
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 5. Prevent Error 139 by tuning the Garbage Collector for low-memory containers
ENV DOTNET_GCServer=0
ENV DOTNET_GCMemoryLimit=400000000 

ENTRYPOINT ["dotnet", "Villarin_SYSARCH.dll"]
