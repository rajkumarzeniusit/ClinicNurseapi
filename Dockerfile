# Use the official .NET 9 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published binaries from your local system to the container
COPY ./publish /app

# Copy the .pfx certificate into the container
COPY mycert.pfx /https/mycert.pfx

# Expose the new port
EXPOSE 8910

# Set the environment variables for ASP.NET Core to listen on all network interfaces and port 9090
ENV ASPNETCORE_URLS=https://+:8910
ENV DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_TLSVERSION=1.2

# Set up the certificate for HTTPS
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/mycert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Zeniusit@123

# Set entry point to run the application
ENTRYPOINT ["dotnet", "TrudoseAdminPortalAPI.dll"]
