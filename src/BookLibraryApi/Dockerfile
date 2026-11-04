FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY BookLibraryApi/BookLibraryApi.csproj BookLibraryApi/
RUN dotnet restore BookLibraryApi/BookLibraryApi.csproj
COPY BookLibraryApi/ BookLibraryApi/
RUN dotnet publish BookLibraryApi/BookLibraryApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BookLibraryApi.dll"]
