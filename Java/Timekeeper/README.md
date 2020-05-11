Simple TimeKeeping API used to better learn Spring Boot and MongoDB. Has following endpoints hosted at localhost:8080/api/v1:

* /getTime/{name}: (GET) Returns total time spent on an entity, where {name} is name of entity
* /getTimeWeek: (GET) Returns total time spent for week
* /timeByName and /timeById: (POST) Adds time to database for entity; body can contain either the entity name 
    or entity ObjectId
* /entity: (POST) Creates a new entity in the database
