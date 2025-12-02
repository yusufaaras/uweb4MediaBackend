# Base image: Uygulamanın çalışacağı temel çalışma zamanı ortamı.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80 # HTTP trafiği için 80 numaralı portu açar

# Build image: Uygulamayı derlemek ve yayınlamak için gerekli SDK'yı içeren ortam.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Çözüm dosyasını ve tüm proje dosyalarını (csproj) kopyalayarak bağımlılıkların önbelleğe alınmasını sağlar.
# Bu, kaynak kodun tamamı değişmediğinde 'dotnet restore' adımının yeniden çalışmasını engeller.
# Dockerfile'ın çözüm kökünde (uweb4Media.sln ile aynı dizinde) olduğunu varsayar.
COPY uweb4Media.sln ./
# Proje dosyalarını kendi klasörlerinden kopyalarız
COPY ["uweb4Media.WebAPI/*.csproj", "uweb4Media.WebAPI/"]
COPY ["Core/uweb4Media.Application/*.csproj", "Core/uweb4Media.Application/"]
COPY ["Infrastructure/Uweb4Media.Persistence/*.csproj", "Infrastructure/Uweb4Media.Persistence/"]

# Bağımlılıkları geri yükler. uweb4Media.WebAPI.csproj dosyasının tam yolunu belirtiyoruz.
RUN dotnet restore "uweb4Media.WebAPI/uweb4Media.WebAPI.csproj"

# Kalan tüm kaynak kodunu kopyalar (bu, restore işleminden sonra değişen dosyaları da içerir).
COPY . .

# Uygulamayı yayınlar (derler ve dağıtıma hazır hale getirir).
# 'Release' konfigürasyonunda yayınlar ve çıktıyı /app/publish dizinine kaydeder.
RUN dotnet publish "uweb4Media.WebAPI/uweb4Media.WebAPI.csproj" -c Release -o /app/publish

# Final image: Uygulamanın çalışacağı son, hafif imaj.
# Sadece 'base' imajını ve yayınlanmış uygulama çıktılarını içerir.
FROM base AS final
WORKDIR /app
# 'build' aşamasından yayınlanmış çıktıları kopyalar.
COPY --from=build /app/publish .
# Uygulamanın başlangıç noktasını belirler.
ENTRYPOINT ["dotnet", "uweb4Media.WebAPI.dll"]