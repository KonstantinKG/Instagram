MIGRATIONS_FOLDER="./Instagram.Infrastructure/Migrations/"
file_count=$(find "$MIGRATIONS_FOLDER" -maxdepth 1 -type f | wc -l)

dotnet-ef database update -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/

exec dotnet run --project ./Instagram.WebApi

