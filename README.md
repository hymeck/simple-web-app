# Simple Web App

## Building image of docker container
`docker build -t simple-web-app -f Dockerfile .` - create docker image

## Heroku deployment
1. login to heroku
   `heroku login`
   
2. login to heroku's container registry
   `heroku container:login`
3. build an image and push it to the container registry
   `heroku container:push web -a <name of app on heroku>`
4. release the image to app on heroku
   `heroku container:release web -a <name of app on heroku>`

## Migration routine
0. Add `Microsoft.EntityFrameworkCore.Design` nuget package
   
1. Implement `IDesignTimeDbContextFactory<TDbContext>`
   
2. Move to `src` directory

3. run `dotnet ef migrations add <migration name> --startup-project WebApp\WebApp.csproj -- project Infrastructure\Infrastructure.csproj --context Infrastructure.Persistence.ApplicationDbContext -o Persistence/Migrations` where `<migration name>` is a name of migration 

4. update database using options mentioned above and connection string via `--connection`


## Useful links
- [Containerize and deploy apps with Docker and Heroku](https://betterprogramming.pub/how-to-containerize-and-deploy-apps-with-docker-and-heroku-b1c49e5bc070)
- [Port error](https://stackoverflow.com/questions/59434242/asp-net-core-gives-system-net-sockets-socketexception-error-on-heroku)
