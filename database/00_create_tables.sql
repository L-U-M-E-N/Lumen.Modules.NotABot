-- Table: public.notabot_stats_server

-- DROP TABLE IF EXISTS public.notabot_stats_server;

CREATE TABLE IF NOT EXISTS public.notabot_stats_server
(
    id bigint NOT NULL,
    name character varying COLLATE pg_catalog."default" NOT NULL,
    member_left integer NOT NULL,
    member_joined integer NOT NULL,
    messagecount integer NOT NULL,
    reactionadded integer NOT NULL,
    reactionremoved integer NOT NULL,
    membercount integer NOT NULL,
    date timestamp with time zone NOT NULL,
    CONSTRAINT notabot_stats_server_pkey PRIMARY KEY (id, date)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.notabot_stats_server
    OWNER to lumen;

GRANT ALL ON TABLE public.notabot_stats_server TO lumen;

-- Table: public.notabot_stats_server_channels

-- DROP TABLE IF EXISTS public.notabot_stats_server_channels;

CREATE TABLE IF NOT EXISTS public.notabot_stats_server_channels
(
    id bigint NOT NULL,
    date timestamp with time zone NOT NULL,
    channel bigint NOT NULL,
    messagecount integer NOT NULL,
    reactioncount integer NOT NULL,
    CONSTRAINT notabot_stats_server_channels_pkey PRIMARY KEY (id, date, channel)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.notabot_stats_server_channels
    OWNER to lumen;

GRANT ALL ON TABLE public.notabot_stats_server_channels TO lumen;

-- Table: public.notabot_stats_server_reactions

-- DROP TABLE IF EXISTS public.notabot_stats_server_reactions;

CREATE TABLE IF NOT EXISTS public.notabot_stats_server_reactions
(
    id bigint NOT NULL,
    date timestamp with time zone NOT NULL,
    reaction character varying COLLATE pg_catalog."default" NOT NULL,
    amount integer NOT NULL,
    CONSTRAINT notabot_stats_server_reactions_pkey PRIMARY KEY (id, date, reaction)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.notabot_stats_server_reactions
    OWNER to lumen;

GRANT ALL ON TABLE public.notabot_stats_server_reactions TO lumen;

-- Table: public.notabot_stats_server_users

-- DROP TABLE IF EXISTS public.notabot_stats_server_users;

CREATE TABLE IF NOT EXISTS public.notabot_stats_server_users
(
    id bigint NOT NULL,
    date timestamp with time zone NOT NULL,
    userid bigint NOT NULL,
    messagecount integer NOT NULL,
    reactioncount integer NOT NULL,
    CONSTRAINT notabot_stats_server_users_pkey PRIMARY KEY (id, date, userid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.notabot_stats_server_users
    OWNER to lumen;

GRANT ALL ON TABLE public.notabot_stats_server_users TO lumen;