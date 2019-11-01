docker build -t dm-api .

rem for local run
rem docker run -it --rm -p 5000:80 --name dm_api dm-api

docker tag dm-api registry.heroku.com/dmam/web
heroku container:push web -a dmam
heroku container:release web -a dmam
heroku open -a dmam
heroku logs --tail -a dmam
pause