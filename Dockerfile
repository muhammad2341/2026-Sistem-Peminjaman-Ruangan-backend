FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["RoomBooking.Api.csproj", "./"]
RUN dotnet restore "RoomBooking.Api.csproj"

COPY . .
RUN dotnet publish "RoomBooking.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "RoomBooking.Api.dll"]
