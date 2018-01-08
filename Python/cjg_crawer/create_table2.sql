create table content (
id int(5) not null auto_increment,
topic_id int(5),
post_time varchar(50),
poster_name varchar(50),
content varchar(1000),
primary key(id),
foreign key (topic_id) references topic(id)
);