# Use a imagem oficial do .NET 8 SDK como stage de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copie o arquivo de solução
COPY MixBalancer.sln ./

# Copie individualmente os arquivos .csproj de cada projeto
COPY MixBalancer.API/*.csproj ./MixBalancer.API/
COPY MixBalancer.Application/*.csproj ./MixBalancer.Application/
COPY MixBalancer.Domain/*.csproj ./MixBalancer.Domain/
COPY MixBalancer.Infrastructure/*.csproj ./MixBalancer.Infrastructure/
COPY MixBalancer.IntegrationTests/*.csproj ./MixBalancer.IntegrationTests/
COPY MixBalancer.Tests/*.csproj ./MixBalancer.Tests/
COPY MixBalancer.UnitTests/*.csproj ./MixBalancer.UnitTests/

# Agora que os arquivos de projeto foram copiados, rode o restore
RUN dotnet restore

# Copie o restante do código-fonte
COPY . ./

# Compile e publique a aplicação
RUN dotnet publish MixBalancer.API -c Release -o out

# Use a imagem runtime do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copie os arquivos publicados do stage de build
COPY --from=build /app/out ./

# Exponha a porta que a aplicação utiliza
EXPOSE 80

# Defina o ponto de entrada
ENTRYPOINT ["dotnet", "MixBalancer.API.dll"]
