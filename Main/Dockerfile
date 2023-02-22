FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
WORKDIR /app
EXPOSE 80
EXPOSE 5000

COPY . .

ENTRYPOINT ["dotnet", "Main.dll"]