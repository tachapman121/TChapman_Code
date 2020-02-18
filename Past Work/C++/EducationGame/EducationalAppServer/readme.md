Educational App Server
===

This is the server to be the middleman between the client and the mysql database. Features fancy(ish) c++ sockets. 

### Database structure

**Database** healthy_running_game

**Table** users  
**Columns**
- user_id _int(11) AI PK_
- username _text_
- password _text_
- is_admin _bit(1)_ default **false**

---

**Table** scores  
**Columns**
- score_id _int(11) AI PK_
- user_id _int(11)_
- score _int(11)_
- difficulty _int(11)_ (values 1, 2, or 3)
- 

### Updating Server app

After you login to the server, in the home folder there will be a shell executable `update_deploy`. Just do a

```shell
./update_deploy
```

and it will pull the changes from git and start the server.


