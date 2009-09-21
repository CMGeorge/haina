# ============================================================================
# Name		  : beluga.sql
# Author	  : shaochuan.yang
# Copyright   : haina
# Description : Beluga Database
# ============================================================================

create table contact(
    cid integer primary key autoincrement,
    type smallint not null,
    uid varchar(256) not null,
    name varchar(64),
    name_spell varchar(64),
    nickname varchar(128),
    nickname_spell varchar(128),
    sex smallint,
    photo varchar(256),
    signature varchar(256),
    phone_pref varchar(80),
    email_pref varchar(80),
    im_pref varchar(256),
    birthday varchar(10),
    org varchar(64),
    url varchar(256),
    call_ring varchar(256),
    msg_ring varchar(256),
    title varchar(256),
    note varchar(512));
GO

create index idx_contact_name_spell on contact(name_spell);
create index idx_contact_name on contact(name);
GO

create table contact_ext(
    ce_id integer primary key autoincrement,
    cid integer not null,
    comm_key smallint,
    comm_value varchar(256));
GO

create index idx_contact_ext_cid on contact_ext(cid);
create index idx_contact_ext_comm_value on contact_ext(comm_value);
GO

create table address(
    aid integer primary key autoincrement,
    block varchar(80),
    street varchar(64),
    district varchar(64),
    city varchar(64),
    state varchar(64),
    country varchar(64),
    postcode varchar(32));
GO

create index idx_address_aid on address(aid);
GO

create table cgroup(
    gid integer primary key autoincrement,
    tid integer not null,
    name varchar(64) not null,
    name_spell varchar(64),
    logo varchar(256),
    group_order smallint,
    delete_flag smallint,
    group_type smallint);
GO

create index idx_group_gid on cgroup(gid);
GO

create table tag(
    tid integer primary key,
    name varchar(64)  not null,
    name_spell varchar(64),
    logo varchar(256),
    tag_order smallint,
    delete_flag smallint);
GO

create index idx_tag_gid on tag(tid);
GO

create table r_contact_group(
    cid integer,
    gid integer);
GO

create trigger r_contact_group_delete after delete on cgroup
begin
    delete from r_contact_group where r_contact_group.gid = old.gid;
end;
GO

create trigger contact_delete after delete on contact
begin
    delete from contact_ext where contact_ext.cid = old.cid;
    delete from r_contact_group where r_contact_group.cid = old.cid;
end;
GO

create trigger contact_ext_delete after delete on contact_ext
begin
    delete from address where address.aid = old.comm_value;
end;
GO

create table recent_contact(
    rc_id integer primary key autoincrement,
    cid integer not null,
    event smallint,
    event_comm varchar(256),
    time timestamp);
GO

create index idx_recent_contact_rc_id on recent_contact(rc_id);
GO

create table phone_district(
    pd_id integer primary key autoincrement,
    district_number varchar(10),
    district_name varchar(64),
    owner_state varchar(64),
    phone_range varchar(45000),
    fee_type varchar(64),
    update_flag smallint,
    weather_code varchar(20));
GO

create table message(
    mid integer primary key autoincrement,
    type smallint not null,
    status smallint not null,
    time timestamp not null,
    fromc varchar(256),
    toc varchar(256),
    gid integer,
    mc_id integer not null,
    subject varchar(256),
    cc varchar(256),
    bcc varchar(256));
GO

create index idx_message_mid on message(mid);
create index idx_message_time on message(time);
GO

create table msg_content(
    mc_id integer primary key autoincrement,
    text_content varchar(1024),
    attach_content binary,
    attach_postfix varchar(5),
    attach_name varchar(64));
GO

create index idx_message_mc_id on msg_content(mc_id);
GO

create table quick_msg(
    qm_id integer primary key autoincrement,
    type integer,
    content varchar(512));
GO

create table signature(
    sid integer primary key autoincrement,
    sign_content varchar(512),
    default_flag smallint);
GO

create table msg_face(
    mf_id integer primary key autoincrement,
    symbol varchar(64),
    picture varchar(256));
GO

create table config(
    cid integer primary key autoincrement,
    cfg_name varchar(64),
    cfg_key integer,
    cfg_value integer);
GO