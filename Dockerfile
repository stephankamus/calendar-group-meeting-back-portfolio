# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CalendarGroupMeeting.Api/CalendarGroupMeeting.Api.csproj", "CalendarGroupMeeting.Api/"]
RUN dotnet restore "CalendarGroupMeeting.Api/CalendarGroupMeeting.Api.csproj"

# Copy source code and build
COPY . .
WORKDIR "/src/CalendarGroupMeeting.Api"
RUN dotnet build "CalendarGroupMeeting.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CalendarGroupMeeting.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published files
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CalendarGroupMeeting.Api.dll"]
