# Base image สำหรับรันแอป ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Build stage: ใช้ .NET SDK สำหรับ build โปรเจกต์
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy ไฟล์โปรเจกต์ก่อน เพื่อเพิ่มประสิทธิภาพ caching ในการ build
COPY ["Disaster_Prediction_And_Alert_System_API.csproj", "./"]
RUN dotnet restore "./Disaster_Prediction_And_Alert_System_API.csproj"

# Copy โค้ดที่เหลือเข้ามาใน container
COPY . .

# Build โปรเจกต์ด้วย configuration ที่ระบุ (Release หรือ Development)
RUN dotnet build "Disaster_Prediction_And_Alert_System_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: สร้างไฟล์พร้อม deploy (optimized)
FROM build AS publish
RUN dotnet publish "Disaster_Prediction_And_Alert_System_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image: ใช้ base image รันแอป โดย copy จาก publish stage มา
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# กำหนดคำสั่งเริ่มรันแอป
ENTRYPOINT ["dotnet", "Disaster_Prediction_And_Alert_System_API.dll"]
