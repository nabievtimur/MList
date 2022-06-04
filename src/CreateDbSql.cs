namespace MList.Storage
{
    public static class DbCreate
    {
        public static string createDbSql = @"
create table addresses
(
    id      integer not null
        constraint addresses_pk
            primary key autoincrement,
    address text
);

create index addresses_address_index
    on addresses (address);

create unique index addresses_id_uindex
    on addresses (id);

create table cars
(
    id     integer not null
        constraint cars_pk
            primary key autoincrement,
    brand  text    not null,
    number text    not null
);

create unique index cars_id_uindex
    on cars (id);

create index cars_number_index
    on cars (number);

create unique index cars_number_uindex
    on cars (number);

create table employees
(
    id          integer not null
        constraint employee_pk
            primary key autoincrement,
    first_name  text    not null,
    last_name   text    not null,
    middle_name text    not null
);

create index employee_first_name_index
    on employees (first_name);

create index employee_first_name_last_name_middle_name_index
    on employees (first_name, last_name, middle_name);

create unique index employee_id_uindex
    on employees (id);

create index employee_last_name_index
    on employees (last_name);

create index employee_middle_name_index
    on employees (middle_name);

create table guns
(
    id     integer not null
        constraint guns_pk
            primary key autoincrement,
    brand  text,
    series text,
    number integer,
    ammo   text
);

create unique index guns_id_uindex
    on guns (id);

create unique index guns_number_uindex
    on guns (number);

create table mlist
(
    id            integer not null
        constraint mlist_pk
            primary key autoincrement,
    date_create   integer not null,
    date_begin    integer not null,
    end_date      integer not null,
    coach_date    integer not null,
    pass_gun_date integer not null,
    print_date    integer not null,
    notes         text,
    deep_time     integer,
    arrive_time   integer,
    pass_gun_time integer,
    num_mlist     integer not null
);

create unique index mlist_id_uindex
    on mlist (id);

create unique index mlist_num_mlist_uindex
    on mlist (num_mlist);

create table mlist_arrive_address
(
    mlist_id          integer not null
        references mlist
            on update cascade,
    arrive_address_id integer not null
        references addresses
            on update cascade
);

create table mlist_cars
(
    mlist_id integer not null
        references mlist
            on update cascade,
    car_id   integer not null
        references cars
            on update cascade
);

create index mlist_cars_car_id_index
    on mlist_cars (car_id);

create index mlist_cars_mlist_id_index
    on mlist_cars (mlist_id);

create table mlist_deep_address
(
    deep_address_id integer not null
        references addresses
            on update cascade,
    mlist_id        integer not null
        references mlist
);

create index mlist_deep_address_deep_address_id_index
    on mlist_deep_address (deep_address_id);

create index mlist_deep_address_mlist_id_index
    on mlist_deep_address (mlist_id);

create table mlist_gun
(
    mlist_id integer not null
        references mlist
            on update cascade,
    gun_id   integer not null
        references guns
            on update cascade
);

create index mlist_gun_gun_id_index
    on mlist_gun (gun_id);

create index mlist_gun_mlist_id_index
    on mlist_gun (mlist_id);

create table orders
(
    id          integer not null
        constraint orders_pk
            primary key autoincrement,
    number      integer not null,
    employee_id integer not null,
    date        integer not null
);

create table order_gun
(
    order_id integer not null
        references orders
            on update cascade,
    gun_id   integer not null
        references guns
            on update cascade
);

create index order_gun_gun_id_index
    on order_gun (gun_id);

create index order_gun_order_id_index
    on order_gun (order_id);

create unique index orders_id_uindex
    on orders (id);

create unique index orders_number_uindex
    on orders (number);";
    }
}