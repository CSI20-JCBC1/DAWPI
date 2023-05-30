PGDMP         *                 {        
   databasePI    14.7    14.7 <    1           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            2           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            3           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            4           1262    16394 
   databasePI    DATABASE     h   CREATE DATABASE "databasePI" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Spanish_Spain.1252';
    DROP DATABASE "databasePI";
                postgres    false                        2615    16395    dlk_controlacceso    SCHEMA     !   CREATE SCHEMA dlk_controlacceso;
    DROP SCHEMA dlk_controlacceso;
                postgres    false            �            1259    16494    cat_estado_cita    TABLE     �   CREATE TABLE dlk_controlacceso.cat_estado_cita (
    id bigint NOT NULL,
    estado_cita character varying NOT NULL,
    desc_estado_cita character varying
);
 .   DROP TABLE dlk_controlacceso.cat_estado_cita;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16493    cat_estado_cita_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.cat_estado_cita_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE dlk_controlacceso.cat_estado_cita_id_seq;
       dlk_controlacceso          postgres    false    221    5            5           0    0    cat_estado_cita_id_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE dlk_controlacceso.cat_estado_cita_id_seq OWNED BY dlk_controlacceso.cat_estado_cita.id;
          dlk_controlacceso          postgres    false    220            �            1259    16526    cat_info_medico    TABLE     �   CREATE TABLE dlk_controlacceso.cat_info_medico (
    id bigint NOT NULL,
    nombre_medico character varying NOT NULL,
    especialidad character varying NOT NULL,
    cod_sala character varying NOT NULL,
    cod_planta character varying NOT NULL
);
 .   DROP TABLE dlk_controlacceso.cat_info_medico;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16525    cat_info_medico_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.cat_info_medico_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE dlk_controlacceso.cat_info_medico_id_seq;
       dlk_controlacceso          postgres    false    223    5            6           0    0    cat_info_medico_id_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE dlk_controlacceso.cat_info_medico_id_seq OWNED BY dlk_controlacceso.cat_info_medico.id;
          dlk_controlacceso          postgres    false    222            �            1259    16437    cat_planta_cita    TABLE     �   CREATE TABLE dlk_controlacceso.cat_planta_cita (
    id bigint NOT NULL,
    cod_planta character varying NOT NULL,
    nombre_planta character varying
);
 .   DROP TABLE dlk_controlacceso.cat_planta_cita;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16436    cat_planta_cita_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.cat_planta_cita_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE dlk_controlacceso.cat_planta_cita_id_seq;
       dlk_controlacceso          postgres    false    217    5            7           0    0    cat_planta_cita_id_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE dlk_controlacceso.cat_planta_cita_id_seq OWNED BY dlk_controlacceso.cat_planta_cita.id;
          dlk_controlacceso          postgres    false    216            �            1259    16410    cat_rol_usuario    TABLE     �   CREATE TABLE dlk_controlacceso.cat_rol_usuario (
    id bigint NOT NULL,
    control_acceso character varying NOT NULL,
    nivel_acceso bigint NOT NULL
);
 .   DROP TABLE dlk_controlacceso.cat_rol_usuario;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16413    cat_rol_usuario_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.cat_rol_usuario_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE dlk_controlacceso.cat_rol_usuario_id_seq;
       dlk_controlacceso          postgres    false    5    212            8           0    0    cat_rol_usuario_id_seq    SEQUENCE OWNED BY     g   ALTER SEQUENCE dlk_controlacceso.cat_rol_usuario_id_seq OWNED BY dlk_controlacceso.cat_rol_usuario.id;
          dlk_controlacceso          postgres    false    213            �            1259    16451    cat_sala_cita    TABLE     �   CREATE TABLE dlk_controlacceso.cat_sala_cita (
    id bigint NOT NULL,
    cod_sala character varying NOT NULL,
    nombre_sala character varying
);
 ,   DROP TABLE dlk_controlacceso.cat_sala_cita;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16450    cat_sala_cita_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.cat_sala_cita_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 6   DROP SEQUENCE dlk_controlacceso.cat_sala_cita_id_seq;
       dlk_controlacceso          postgres    false    5    219            9           0    0    cat_sala_cita_id_seq    SEQUENCE OWNED BY     c   ALTER SEQUENCE dlk_controlacceso.cat_sala_cita_id_seq OWNED BY dlk_controlacceso.cat_sala_cita.id;
          dlk_controlacceso          postgres    false    218            �            1259    16428    citas    TABLE       CREATE TABLE dlk_controlacceso.citas (
    md_uuid character varying(255),
    md_date timestamp without time zone,
    id integer NOT NULL,
    asunto character varying(255),
    nombre_paciente character varying(255),
    fecha date,
    sintomas character varying(1000),
    nombre_medico character varying(255),
    hora time without time zone,
    cod_planta character varying(255),
    cod_sala character varying(255),
    enfermedad character varying(255),
    solucion character varying(255),
    estado_cita character varying(255)
);
 $   DROP TABLE dlk_controlacceso.citas;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16427    citas_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.citas_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE dlk_controlacceso.citas_id_seq;
       dlk_controlacceso          postgres    false    215    5            :           0    0    citas_id_seq    SEQUENCE OWNED BY     S   ALTER SEQUENCE dlk_controlacceso.citas_id_seq OWNED BY dlk_controlacceso.citas.id;
          dlk_controlacceso          postgres    false    214            �            1259    16396    usuarios    TABLE     w  CREATE TABLE dlk_controlacceso.usuarios (
    md_uuid character varying NOT NULL,
    md_date timestamp without time zone NOT NULL,
    id bigint NOT NULL,
    nombre_completo character varying NOT NULL,
    movil character varying NOT NULL,
    email character varying NOT NULL,
    contrasenya character varying NOT NULL,
    rol bigint NOT NULL,
    verificado boolean
);
 '   DROP TABLE dlk_controlacceso.usuarios;
       dlk_controlacceso         heap    postgres    false    5            �            1259    16401    usuarios_id_seq    SEQUENCE     �   CREATE SEQUENCE dlk_controlacceso.usuarios_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE dlk_controlacceso.usuarios_id_seq;
       dlk_controlacceso          postgres    false    5    210            ;           0    0    usuarios_id_seq    SEQUENCE OWNED BY     Y   ALTER SEQUENCE dlk_controlacceso.usuarios_id_seq OWNED BY dlk_controlacceso.usuarios.id;
          dlk_controlacceso          postgres    false    211            �           2604    16497    cat_estado_cita id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.cat_estado_cita ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.cat_estado_cita_id_seq'::regclass);
 L   ALTER TABLE dlk_controlacceso.cat_estado_cita ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    220    221    221            �           2604    16529    cat_info_medico id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.cat_info_medico ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.cat_info_medico_id_seq'::regclass);
 L   ALTER TABLE dlk_controlacceso.cat_info_medico ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    222    223    223            ~           2604    16440    cat_planta_cita id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.cat_planta_cita ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.cat_planta_cita_id_seq'::regclass);
 L   ALTER TABLE dlk_controlacceso.cat_planta_cita ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    217    216    217            |           2604    16414    cat_rol_usuario id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.cat_rol_usuario ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.cat_rol_usuario_id_seq'::regclass);
 L   ALTER TABLE dlk_controlacceso.cat_rol_usuario ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    213    212                       2604    16454    cat_sala_cita id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.cat_sala_cita ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.cat_sala_cita_id_seq'::regclass);
 J   ALTER TABLE dlk_controlacceso.cat_sala_cita ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    219    218    219            }           2604    16431    citas id    DEFAULT     z   ALTER TABLE ONLY dlk_controlacceso.citas ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.citas_id_seq'::regclass);
 B   ALTER TABLE dlk_controlacceso.citas ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    214    215    215            {           2604    16402    usuarios id    DEFAULT     �   ALTER TABLE ONLY dlk_controlacceso.usuarios ALTER COLUMN id SET DEFAULT nextval('dlk_controlacceso.usuarios_id_seq'::regclass);
 E   ALTER TABLE dlk_controlacceso.usuarios ALTER COLUMN id DROP DEFAULT;
       dlk_controlacceso          postgres    false    211    210            ,          0    16494    cat_estado_cita 
   TABLE DATA           W   COPY dlk_controlacceso.cat_estado_cita (id, estado_cita, desc_estado_cita) FROM stdin;
    dlk_controlacceso          postgres    false    221   �M       .          0    16526    cat_info_medico 
   TABLE DATA           k   COPY dlk_controlacceso.cat_info_medico (id, nombre_medico, especialidad, cod_sala, cod_planta) FROM stdin;
    dlk_controlacceso          postgres    false    223   N       (          0    16437    cat_planta_cita 
   TABLE DATA           S   COPY dlk_controlacceso.cat_planta_cita (id, cod_planta, nombre_planta) FROM stdin;
    dlk_controlacceso          postgres    false    217   WN       #          0    16410    cat_rol_usuario 
   TABLE DATA           V   COPY dlk_controlacceso.cat_rol_usuario (id, control_acceso, nivel_acceso) FROM stdin;
    dlk_controlacceso          postgres    false    212   �N       *          0    16451    cat_sala_cita 
   TABLE DATA           M   COPY dlk_controlacceso.cat_sala_cita (id, cod_sala, nombre_sala) FROM stdin;
    dlk_controlacceso          postgres    false    219   �N       &          0    16428    citas 
   TABLE DATA           �   COPY dlk_controlacceso.citas (md_uuid, md_date, id, asunto, nombre_paciente, fecha, sintomas, nombre_medico, hora, cod_planta, cod_sala, enfermedad, solucion, estado_cita) FROM stdin;
    dlk_controlacceso          postgres    false    215   O       !          0    16396    usuarios 
   TABLE DATA           �   COPY dlk_controlacceso.usuarios (md_uuid, md_date, id, nombre_completo, movil, email, contrasenya, rol, verificado) FROM stdin;
    dlk_controlacceso          postgres    false    210   �P       <           0    0    cat_estado_cita_id_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('dlk_controlacceso.cat_estado_cita_id_seq', 4, true);
          dlk_controlacceso          postgres    false    220            =           0    0    cat_info_medico_id_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('dlk_controlacceso.cat_info_medico_id_seq', 4, true);
          dlk_controlacceso          postgres    false    222            >           0    0    cat_planta_cita_id_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('dlk_controlacceso.cat_planta_cita_id_seq', 5, true);
          dlk_controlacceso          postgres    false    216            ?           0    0    cat_rol_usuario_id_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('dlk_controlacceso.cat_rol_usuario_id_seq', 4, true);
          dlk_controlacceso          postgres    false    213            @           0    0    cat_sala_cita_id_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('dlk_controlacceso.cat_sala_cita_id_seq', 4, true);
          dlk_controlacceso          postgres    false    218            A           0    0    citas_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('dlk_controlacceso.citas_id_seq', 17, true);
          dlk_controlacceso          postgres    false    214            B           0    0    usuarios_id_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('dlk_controlacceso.usuarios_id_seq', 35, true);
          dlk_controlacceso          postgres    false    211            �           2606    16501 $   cat_estado_cita cat_estado_cita_pkey 
   CONSTRAINT     v   ALTER TABLE ONLY dlk_controlacceso.cat_estado_cita
    ADD CONSTRAINT cat_estado_cita_pkey PRIMARY KEY (estado_cita);
 Y   ALTER TABLE ONLY dlk_controlacceso.cat_estado_cita DROP CONSTRAINT cat_estado_cita_pkey;
       dlk_controlacceso            postgres    false    221            �           2606    16533 $   cat_info_medico cat_info_medico_pkey 
   CONSTRAINT     x   ALTER TABLE ONLY dlk_controlacceso.cat_info_medico
    ADD CONSTRAINT cat_info_medico_pkey PRIMARY KEY (nombre_medico);
 Y   ALTER TABLE ONLY dlk_controlacceso.cat_info_medico DROP CONSTRAINT cat_info_medico_pkey;
       dlk_controlacceso            postgres    false    223            �           2606    16444 $   cat_planta_cita cat_planta_cita_pkey 
   CONSTRAINT     u   ALTER TABLE ONLY dlk_controlacceso.cat_planta_cita
    ADD CONSTRAINT cat_planta_cita_pkey PRIMARY KEY (cod_planta);
 Y   ALTER TABLE ONLY dlk_controlacceso.cat_planta_cita DROP CONSTRAINT cat_planta_cita_pkey;
       dlk_controlacceso            postgres    false    217            �           2606    16421 $   cat_rol_usuario cat_rol_usuario_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY dlk_controlacceso.cat_rol_usuario
    ADD CONSTRAINT cat_rol_usuario_pkey PRIMARY KEY (nivel_acceso);
 Y   ALTER TABLE ONLY dlk_controlacceso.cat_rol_usuario DROP CONSTRAINT cat_rol_usuario_pkey;
       dlk_controlacceso            postgres    false    212            �           2606    16458     cat_sala_cita cat_sala_cita_pkey 
   CONSTRAINT     o   ALTER TABLE ONLY dlk_controlacceso.cat_sala_cita
    ADD CONSTRAINT cat_sala_cita_pkey PRIMARY KEY (cod_sala);
 U   ALTER TABLE ONLY dlk_controlacceso.cat_sala_cita DROP CONSTRAINT cat_sala_cita_pkey;
       dlk_controlacceso            postgres    false    219            �           2606    16435    citas citas_pkey 
   CONSTRAINT     Y   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT citas_pkey PRIMARY KEY (id);
 E   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT citas_pkey;
       dlk_controlacceso            postgres    false    215            �           2606    16409    usuarios usuarios_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY dlk_controlacceso.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (nombre_completo);
 K   ALTER TABLE ONLY dlk_controlacceso.usuarios DROP CONSTRAINT usuarios_pkey;
       dlk_controlacceso            postgres    false    210            �           2606    16534    citas medico_info_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT medico_info_fk FOREIGN KEY (nombre_medico) REFERENCES dlk_controlacceso.cat_info_medico(nombre_medico) NOT VALID;
 I   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT medico_info_fk;
       dlk_controlacceso          postgres    false    223    215    3215            �           2606    16473    citas medico_usuarios_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT medico_usuarios_fk FOREIGN KEY (nombre_medico) REFERENCES dlk_controlacceso.usuarios(nombre_completo) NOT VALID;
 M   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT medico_usuarios_fk;
       dlk_controlacceso          postgres    false    210    215    3203            �           2606    16478    citas paciente_usuarios_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT paciente_usuarios_fk FOREIGN KEY (nombre_paciente) REFERENCES dlk_controlacceso.usuarios(nombre_completo) NOT VALID;
 O   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT paciente_usuarios_fk;
       dlk_controlacceso          postgres    false    215    3203    210            �           2606    16488    citas planta_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT planta_fk FOREIGN KEY (cod_planta) REFERENCES dlk_controlacceso.cat_planta_cita(cod_planta) NOT VALID;
 D   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT planta_fk;
       dlk_controlacceso          postgres    false    215    3209    217            �           2606    16422    usuarios rol_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.usuarios
    ADD CONSTRAINT rol_fk FOREIGN KEY (rol) REFERENCES dlk_controlacceso.cat_rol_usuario(nivel_acceso) NOT VALID;
 D   ALTER TABLE ONLY dlk_controlacceso.usuarios DROP CONSTRAINT rol_fk;
       dlk_controlacceso          postgres    false    3205    212    210            �           2606    16459    citas sala_fk    FK CONSTRAINT     �   ALTER TABLE ONLY dlk_controlacceso.citas
    ADD CONSTRAINT sala_fk FOREIGN KEY (cod_sala) REFERENCES dlk_controlacceso.cat_sala_cita(cod_sala) NOT VALID;
 B   ALTER TABLE ONLY dlk_controlacceso.citas DROP CONSTRAINT sala_fk;
       dlk_controlacceso          postgres    false    3211    215    219            ,   X   x�3�p�H�K�L�+IUHIUH,�L�KL�<�9�ˈӑ��OI�2�t�t��K�ɬqM8�<������&g$*T*d�%r��qqq ֮�      .   C   x�3�HM)�W�O):�6�4�J�=?����*N��⒢�Լ�Ԣ�����k9�9�b���� �EU      (   "   x�3�0��I�+IT0�2�0���b���� ���      #   @   x�3���Sp,�L�K,�4�2�HL�L�+I�4�2��=�2%39�Ӑ˄�1%73�Ӏ+F��� �O"      *   2   x�3�t6�t��+.�)IT0�2�t6F����#߈���7����� �'      &   �  x���M��0���)�2��߮�!BC�r6e����-%�ݐ�M`V�e�Ef&t$��(�z����N�Bޘ�p�6�{_K�`+d���fJ(ͅ�B��:��*���0)��x�	<�Þ.��-`��'x��w�~g�[t~}�	�.!��6N�� �,�aν)��`3���Tn�����V��c�7�v�qU�h���8#m�B`�� U'm'��Y)ˤ�!���x�;ï �d�t�p�4f�3����Ҷ�oX/���z]&©����z����O�>����!�0R�.�=��c�i}:,t��.��H&ugl'�l/�Ns��3K�<�>�&8-��7��Yk,���D��mN�m\�d5��g@��4��Jm�����Tq��>�y�,�T����O��S��|�]�PE��#��      !   n  x�e�MR"1��u<�i^^��(�� "~�5�$ݍ Do3����Mp,��\�*Ie�˫?���,)8@����U�V(�I��)A�h ��O�QAC�j�h���G͔$�x���y`��s;�%>��>�}�r�/���iw]��6�n�y�gú�{L�n.rsq��ț~<��0�<�N��˞������T�X���B���a^N4o�n!KP)c4���j��ջ�{�M$�'S���j����pP$���V����py_'||���O�6�C�9ʺ[1����m�nW���R^p�lI��*+�t�e���`�Zȣ�1��q��'�I���X�������n/?���m~�w�j��qͲ���ѓ?Tr�	�a��z�.\g볐�N,����0�E����(3�.��N��_�kD0a<`dP���b��k\���NX��������C�J�r��+��>�����F���:��*��$�<�I�SsZ�l�jNX�;%*k��EI9��:ɋ��
��u�>�i��-�vp��H����:���A�Y��X���=:}��_�wU��d]���vt�Ӭ����T>ذ$�����&��IP����'{{{ g���     