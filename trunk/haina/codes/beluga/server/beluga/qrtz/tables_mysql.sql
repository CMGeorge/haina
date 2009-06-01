#
# Quartz seems to work best with the driver mm.mysql-2.0.7-bin.jar
#
# In your Quartz properties file, you'll need to set 
# org.quartz.jobStore.driverDelegateClass = org.quartz.impl.jdbcjobstore.StdJDBCDelegate
#

DROP TABLE IF EXISTS QRTZ_JOB_LISTENERS;
DROP TABLE IF EXISTS QRTZ_TRIGGER_LISTENERS;
DROP TABLE IF EXISTS QRTZ_FIRED_TRIGGERS;
DROP TABLE IF EXISTS QRTZ_PAUSED_TRIGGER_GRPS;
DROP TABLE IF EXISTS QRTZ_SCHEDULER_STATE;
DROP TABLE IF EXISTS QRTZ_LOCKS;
DROP TABLE IF EXISTS QRTZ_SIMPLE_TRIGGERS;
DROP TABLE IF EXISTS QRTZ_CRON_TRIGGERS;
DROP TABLE IF EXISTS QRTZ_BLOB_TRIGGERS;
DROP TABLE IF EXISTS QRTZ_TRIGGERS;
DROP TABLE IF EXISTS QRTZ_JOB_DETAILS;
DROP TABLE IF EXISTS QRTZ_CALENDARS;


CREATE TABLE QRTZ_JOB_DETAILS
  (
    JOB_NAME  NVARCHAR(80) NOT NULL,
    JOB_GROUP NVARCHAR(80) NOT NULL,
    DESCRIPTION NVARCHAR(120) NULL,
    JOB_CLASS_NAME   NVARCHAR(128) NOT NULL,
    IS_DURABLE NVARCHAR(1) NOT NULL,
    IS_VOLATILE NVARCHAR(1) NOT NULL,
    IS_STATEFUL NVARCHAR(1) NOT NULL,
    REQUESTS_RECOVERY NVARCHAR(1) NOT NULL,
    JOB_DATA BLOB NULL,
    PRIMARY KEY (JOB_NAME,JOB_GROUP)
);

CREATE TABLE QRTZ_JOB_LISTENERS
  (
    JOB_NAME  NVARCHAR(80) NOT NULL,
    JOB_GROUP NVARCHAR(80) NOT NULL,
    JOB_LISTENER NVARCHAR(80) NOT NULL,
    PRIMARY KEY (JOB_NAME,JOB_GROUP,JOB_LISTENER),
    FOREIGN KEY (JOB_NAME,JOB_GROUP)
        REFERENCES QRTZ_JOB_DETAILS(JOB_NAME,JOB_GROUP)
);

CREATE TABLE QRTZ_TRIGGERS
  (
    TRIGGER_NAME NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    JOB_NAME  NVARCHAR(80) NOT NULL,
    JOB_GROUP NVARCHAR(80) NOT NULL,
    IS_VOLATILE NVARCHAR(1) NOT NULL,
    DESCRIPTION NVARCHAR(120) NULL,
    NEXT_FIRE_TIME BIGINT(13) NULL,
    PREV_FIRE_TIME BIGINT(13) NULL,
    TRIGGER_STATE NVARCHAR(16) NOT NULL,
    TRIGGER_TYPE NVARCHAR(8) NOT NULL,
    START_TIME BIGINT(13) NOT NULL,
    END_TIME BIGINT(13) NULL,
    CALENDAR_NAME NVARCHAR(80) NULL,
    MISFIRE_INSTR SMALLINT(2) NULL,
    JOB_DATA BLOB NULL,
    PRIMARY KEY (TRIGGER_NAME,TRIGGER_GROUP),
    FOREIGN KEY (JOB_NAME,JOB_GROUP)
        REFERENCES QRTZ_JOB_DETAILS(JOB_NAME,JOB_GROUP)
);

CREATE TABLE QRTZ_SIMPLE_TRIGGERS
  (
    TRIGGER_NAME NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    REPEAT_COUNT BIGINT(7) NOT NULL,
    REPEAT_INTERVAL BIGINT(12) NOT NULL,
    TIMES_TRIGGERED BIGINT(7) NOT NULL,
    PRIMARY KEY (TRIGGER_NAME,TRIGGER_GROUP),
    FOREIGN KEY (TRIGGER_NAME,TRIGGER_GROUP)
        REFERENCES QRTZ_TRIGGERS(TRIGGER_NAME,TRIGGER_GROUP)
);

CREATE TABLE QRTZ_CRON_TRIGGERS
  (
    TRIGGER_NAME NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    CRON_EXPRESSION NVARCHAR(80) NOT NULL,
    TIME_ZONE_ID NVARCHAR(80),
    PRIMARY KEY (TRIGGER_NAME,TRIGGER_GROUP),
    FOREIGN KEY (TRIGGER_NAME,TRIGGER_GROUP)
        REFERENCES QRTZ_TRIGGERS(TRIGGER_NAME,TRIGGER_GROUP)
);

CREATE TABLE QRTZ_BLOB_TRIGGERS
  (
    TRIGGER_NAME NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    BLOB_DATA BLOB NULL,
    PRIMARY KEY (TRIGGER_NAME,TRIGGER_GROUP),
    FOREIGN KEY (TRIGGER_NAME,TRIGGER_GROUP)
        REFERENCES QRTZ_TRIGGERS(TRIGGER_NAME,TRIGGER_GROUP)
);

CREATE TABLE QRTZ_TRIGGER_LISTENERS
  (
    TRIGGER_NAME  NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    TRIGGER_LISTENER NVARCHAR(80) NOT NULL,
    PRIMARY KEY (TRIGGER_NAME,TRIGGER_GROUP,TRIGGER_LISTENER),
    FOREIGN KEY (TRIGGER_NAME,TRIGGER_GROUP)
        REFERENCES QRTZ_TRIGGERS(TRIGGER_NAME,TRIGGER_GROUP)
);


CREATE TABLE QRTZ_CALENDARS
  (
    CALENDAR_NAME  NVARCHAR(80) NOT NULL,
    CALENDAR BLOB NOT NULL,
    PRIMARY KEY (CALENDAR_NAME)
);



CREATE TABLE QRTZ_PAUSED_TRIGGER_GRPS
  (
    TRIGGER_GROUP  NVARCHAR(80) NOT NULL, 
    PRIMARY KEY (TRIGGER_GROUP)
);

CREATE TABLE QRTZ_FIRED_TRIGGERS
  (
    ENTRY_ID NVARCHAR(95) NOT NULL,
    TRIGGER_NAME NVARCHAR(80) NOT NULL,
    TRIGGER_GROUP NVARCHAR(80) NOT NULL,
    IS_VOLATILE NVARCHAR(1) NOT NULL,
    INSTANCE_NAME NVARCHAR(80) NOT NULL,
    FIRED_TIME BIGINT(13) NOT NULL,
    STATE NVARCHAR(16) NOT NULL,
    JOB_NAME NVARCHAR(80) NULL,
    JOB_GROUP NVARCHAR(80) NULL,
    IS_STATEFUL NVARCHAR(1) NULL,
    REQUESTS_RECOVERY NVARCHAR(1) NULL,
    PRIMARY KEY (ENTRY_ID)
);

CREATE TABLE QRTZ_SCHEDULER_STATE
  (
    INSTANCE_NAME NVARCHAR(80) NOT NULL,
    LAST_CHECKIN_TIME BIGINT(13) NOT NULL,
    CHECKIN_INTERVAL BIGINT(13) NOT NULL,
    RECOVERER NVARCHAR(80) NULL,
    PRIMARY KEY (INSTANCE_NAME)
);

CREATE TABLE QRTZ_LOCKS
  (
    LOCK_NAME  NVARCHAR(40) NOT NULL, 
    PRIMARY KEY (LOCK_NAME)
);


INSERT INTO QRTZ_LOCKS values('TRIGGER_ACCESS');
INSERT INTO QRTZ_LOCKS values('JOB_ACCESS');
INSERT INTO QRTZ_LOCKS values('CALENDAR_ACCESS');
INSERT INTO QRTZ_LOCKS values('STATE_ACCESS');
INSERT INTO QRTZ_LOCKS values('MISFIRE_ACCESS');


commit;