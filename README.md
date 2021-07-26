# Установка и запуск

ДМчику необходимы MongoDb, PostgresSql, ElasticSearch, RabbitMQ, Node.js, dotnet 3.1+.
Локальной разработке значительно помогут PgAdmin4, Kibana.

Настоятельно рекомендую при установке использовать docker, но это не принципиально. Когда-нибудь в будущем я напишу готовый docker-compose, но это потом.

Пока можно запускать по одиночке. Обратите внимание, что все докеры желательно должны быть настроены на один нетворк; также я советую запускать RMQ сразу с менеджмент-плагином, поскольку он не поставляется отдельно.

```
#!bash

docker network create dm
docker run -d --network dm --name dm-pg -p 5432:5432 -e POSTGRES_PASSWORD=admin postgres:latest
docker run -d --network dm --name dm-mongo -p 27017:27017 mongo:latest
docker run -d --network dm --name dm-es -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.6.2
docker run -d --network dm --name dm-rmq -p 5672:5672 -p 15672:15672 --hostname dm-rmq rabbitmq:3-management
```

Если вам мало этого и хочется работать с содержимым некоторых хранилищ, то вот ваш путь:

```
#!bash

docker run -d --network dm --name dm-kibana -p 5601:5601 -e "ELASTICSEARCH_HOSTS=http://dm-es:9200" docker.elastic.co/kibana/kibana:7.6.2
docker run -d --network dm --name dm-pgadmin -p 5555:80 -e PGADMIN_DEFAULT_EMAIL=admin@dm.am -e PGADMIN_DEFAULT_PASSWORD=admin dpage/pgadmin4:latest
```

Для работы с Mongo можно ставить локально программу MongoDb Compass.

Теперь, когда у вас появились все необходимые базы данных, нужно заняться применением схемы. Для этого идем в директорию `DM/Services/DM.Services.DataAccess`, запускаем там консоль и выполняем команду `dotnet ef database update`. После короткого дупления вы должны увидеть в консоли список из различных миграций и сообщение о том, что все готово. Никаких ошибок тут быть не должно!

Наконец, когда у нас есть данные, мы можем запускать приложения!
Для корркетной работы всего ДМчика необходимо запустить:

 - DM.Web.API
 - DM.Services.Mail.Sender.Consumer (эта штука будет отправлять письма)
 - DM.Services.Search.Consumer (эта штука будет индексировать данные для поискового движка)

Пока для последних не предусмотрена докеризация, мне было тупо лень. Но для DM.Web.API идем в директорию DM корня и выполняем

```
#!bash

docker build -t dm-api .
docker run -e PORT=5000 --network dm --name dm-api -it --rm -p 5000:5000 dm-api
```

С текущим состоянием мастера у вас скорее всего возникнут проблемы при подключении к базам и прочему. Но при запуске контейнера можно указать переменные окружения, которые будут приняты с большим приоритетом, чем значения в конфигурации.

```
#!bash

docker run --network dm --name dm_api -it --rm -p 5000:5000 \
 -e PORT=5000 \
 -e DM_ConnectionStrings__Rdb="User ID=postgres;Password=admin;Host=dm-pg;Port=5432;Database=dm3.5;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;" \
 -e DM_ConnectionStrings__Mongo="mongodb://dm-mongo:27017/dm3-5?maxPoolSize=1000" \
 -e DM_ConnectionStrings__SearchEngine="http://dm-es:9200" \
 -e DM_ConnectionStrings__Logs="http://dm-es:9200" \
 -e DM_ConnectionStrings__MessageQueue="amqp://guest:guest@dm-rmq:5672/" \
 dm-api
```

Вариант в одну строку
```
#!bash

docker run -d --network dm --name dm_api -it --rm -p 5000:5000 -e PORT=5000 -e DM_ConnectionStrings__Rdb="User ID=postgres;Password=admin;Host=dm-pg;Port=5432;Database=dm3.5;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;" -e DM_ConnectionStrings__Mongo="mongodb://dm-mongo:27017/dm3-5?maxPoolSize=1000" -e DM_ConnectionStrings__SearchEngine="http://dm-es:9200" -e DM_ConnectionStrings__Logs="http://dm-es:9200" -e DM_ConnectionStrings__MessageQueue="amqp://dm-rmq:5672" dm-api
```

Если вы пользуетесь виндовыми контейнерами, то вместо разделителя `__`, кажется, нужно использовать `:`.

Если вам не нравится этот подход, то можно перед сборкой контейнера поменять конфигурацию:

*рабочий DM\Web\DM.Web.API\appsettings.json*

```
#!json

{
  "ConnectionStrings": {
    "Rdb": "User ID=postgres;Password=admin;Host=dm-pg;Port=5432;Database=dm3.5;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;",
    "Mongo": "mongodb://dm-mongo:27017/dm3-5?maxPoolSize=1000",
    "SearchEngine": "http://dm-es:9200",
    "Logs": "http://dm-es:9200",
    "MessageQueue": "ampq://guest:guest@dm-rmq:5672/"
  },
  "IntegrationSettings": {
    "ApiUrl": "https://api.dm.am",
    "WebUrl": "https://dm.am",
    "AdminUrl": "https://a.dm.am",
    "MobileUrl": "https://m.dm.am"
  }
}
```


________________

Для запуска DM.Web.Modern необходимо установить Node.js хотя бы 12й версии. Не сомневаюсь, что это можно как-то делать через докер, но мне влом. Так что

```
#!bash

npm i -g yarn
yarn & yarn run serve
```
