MIGRATIONS_FOLDER="./Instagram.Infrastructure/Migrations/"
file_count=$(find "$MIGRATIONS_FOLDER" -maxdepth 1 -type f | wc -l)

if [ "$file_count" -eq "0" ]; then
  echo "Creating new migration..."
  dotnet-ef migrations add InitialCreate -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/
  
  echo "Updating database..."
  dotnet-ef database update -p ./Instagram.Infrastructure/ -s ./Instagram.WebApi/
fi

exec dotnet run --project ./Instagram.WebApi

