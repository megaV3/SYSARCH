FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the entire solution and project folders as they are
COPY . .

# Run restore directly pointing to the inner project file 
# (This tells MSBuild exactly where the project file lives)
RUN dotnet restore "Villarin_SYSARCH/Villarin_SYSARCH.csproj"

# Build and publish pointing to the project file
RUN dotnet publish "Villarin_SYSARCH/Villarin_SYSARCH.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Villarin_SYSARCH.dll"]