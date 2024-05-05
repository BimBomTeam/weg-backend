#Build stage

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY "WEG-Server.sln" "./"

COPY "WEG.WebUI/WEG.WebUI.csproj" "WEG.WebUI/"
RUN dotnet restore "WEG.WebUI/WEG.WebUI.csproj"

COPY "WEG.Application/WEG.Application.csproj" "WEG.Application/"
RUN dotnet restore "WEG.Application/WEG.Application.csproj"

COPY "WEG.Domain/WEG.Domain.csproj" "WEG.Domain/"
RUN dotnet restore "WEG.Domain/WEG.Domain.csproj"

COPY "WEG.Infrastructure/WEG.Infrastructure.csproj" "WEG.Infrastructure/"
RUN dotnet restore "WEG.Infrastructure/WEG.Infrastructure.csproj"

#RUN dotnet restore

COPY . ./
RUN dotnet publish "WEG.WebUI/WEG.WebUI.csproj" -c Release -o out

#Serve stage
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
#COPY mysite.crt /etc/ssl/certs/
#COPY mysite.key /etc/ssl/private/
COPY --from=build /app/out .

EXPOSE 5252

ENTRYPOINT ["dotnet", "WEG.WebUI.dll"]

