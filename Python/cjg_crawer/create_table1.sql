create table topic (
id int(5) not null auto_increment,
title varchar(255) not null,
reply_counter int(10),
poster_name varchar(50),
view_counter int(10),
last_time varchar(50),
link varchar(255),
primary key(id)
);