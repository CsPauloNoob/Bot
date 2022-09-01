FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR app/

# Copy everything
COPY bin/Release/netcoreapp3.1/publish app/

ENTRYPOINT ["dotnet", "Bot_Manager.dll"]