# Apply database migrations
dotnet-ef database update -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/

# Run the application
exec dotnet run --project ./Instagram.WebApi --urls http://0.0.0.0:5054

