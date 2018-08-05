FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

COPY ./Web.sln .
COPY ./test/demo42tests/demo42tests.csproj test/demo42tests/
COPY ./src/WebUI/WebUI.csproj src/WebUI/
RUN dotnet restore src/WebUI/WebUI.csproj

COPY . .
RUN dotnet build -c Release src/WebUI/WebUI.csproj

FROM build as test
WORKDIR /app
RUN dotnet test test/demo42tests/demo42tests.csproj

FROM build AS publish
WORKDIR /app/src/WebUI
RUN dotnet publish WebUI.csproj -c Release -o /out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /out ./
ENV VERSION=1.0.18
ENV ASPNETCORE_ENVIRONMENT=development
ENTRYPOINT ["dotnet", "WebUI.dll"]
