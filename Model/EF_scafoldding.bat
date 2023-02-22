@echo off
cd %cd%
dotnet ef dbcontext scaffold "Server=**;User Id=root;Password=**;Database=drugdb" Pomelo.EntityFrameworkCore.MySql -o Model -f
pause