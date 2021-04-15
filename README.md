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

## Useful links
- [Containerize and deploy apps with Docker and Heroku](https://betterprogramming.pub/how-to-containerize-and-deploy-apps-with-docker-and-heroku-b1c49e5bc070)
- [Port error](https://stackoverflow.com/questions/59434242/asp-net-core-gives-system-net-sockets-socketexception-error-on-heroku)
