MIGRATIONS_FOLDER="./Instagram.Infrastructure/Migrations"
file_count=$(find "$MIGRATIONS_FOLDER" -maxdepth 1 -type f | wc -l)

if [ ! "$file_count" -eq 0 ]; then
  dotnet-ef migrations remove -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/ -f
fi

dotnet-ef migrations add InitialCreate -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/
dotnet-ef database update -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/

exec dotnet run --project ./Instagram.WebApi --urls http://0.0.0.0:5054

