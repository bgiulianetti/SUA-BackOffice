﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SUA.Models;
using Nest;
using Elasticsearch.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace SUA.Repositorios
{
    public class ESRepositorio
    {
        public const string INVALID_SETTINGS_EXCEPTION = "Configuración de ES inválida";

        //Standupero
        public const string INVALID_STANDUPERO_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los standuperos";
        public const string STANDUPERO_GET_ALL_EXCEPTION = "Falla al querer obtener todos los standuperos";
        public const string STANDUPERO_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero por apellido debe pasar un apellido válido";
        public const string STANDUPERO_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero por apellido";
        public const string STANDUPERO_GET_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero por dni";
        public const string STANDUPERO_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero por dni debe pasar un dni válido";
        public const string STANDUPERO_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero innerId por dni debe pasar un dni válido";
        public const string STANDUPERO_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero innerId por dni";
        public const string STANDUPERO_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un standupero debe pasar un standupero válido";
        public const string STANDUPERO_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un standupero debe pasar un standupero que no exista previamente";
        public const string STANDUPERO_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un standupero nuevo";
        public const string STANDUPERO_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un standupero debe pasar un standupero válido";
        public const string STANDUPERO_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un standupero debe pasar un standupero que exista previamente";
        public const string STANDUPERO_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un standupero";
        public const string STANDUPERO_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un standupero por dni debe pasar un dni válido";
        public const string STANDUPERO_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un standupero debe pasar un standupero que exista previamente";
        public const string STANDUPERO_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un standupero";


        //Productor
        public const string INVALID_PRODUCTOR_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los productores";
        public const string PRODUCTOR_GET_ALL_EXCEPTION = "Falla al querer obtener todos los productores";
        public const string PRODUCTOR_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION = "Para obtener un productor por apellido debe pasar un apellido válido";
        public const string PRODUCTOR_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un productor por apellido";
        public const string PRODUCTOR_GET_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un productor por dni";
        public const string PRODUCTOR_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un productor por dni debe pasar un dni válido";
        public const string PRODUCTOR_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un productor innerId por dni debe pasar un dni válido";
        public const string PRODUCTOR_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un productor innerId por dni";
        public const string PRODUCTOR_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un productor debe pasar un productor válido";
        public const string PRODUCTOR_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un productor debe pasar un productor que no exista previamente";
        public const string PRODUCTOR_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un productor nuevo";
        public const string PRODUCTOR_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un productor debe pasar un productor válido";
        public const string PRODUCTOR_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un productor debe pasar un productor que exista previamente";
        public const string PRODUCTOR_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un productor";
        public const string PRODUCTOR_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un productor por dni debe pasar un dni válido";
        public const string PRODUCTOR_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un productor debe pasar un productor que exista previamente";
        public const string PRODUCTOR_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un productor";





        //show
        public const string INVALID_SHOW_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los shows";
        public const string SHOW_GET_ALL_EXCEPTION = "Falla al querer obtener todos los shows";
        public const string SHOW_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un show por nombre debe pasar un nombre válido";
        public const string SHOW_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un show por nombre";
        public const string SHOW_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un show por id debe pasar un id válido";
        public const string SHOW_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un show por id";
        public const string SHOW_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un show debe pasar un show válido";
        public const string SHOW_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un show debe pasar un show que no exista previamente";
        public const string SHOW_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un show nuevo";
        public const string SHOW_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un show debe pasar un show válido";
        public const string SHOW_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un show debe pasar un show que exista previamente";
        public const string SHOW_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un show";
        public const string SHOW_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un show innerId por id debe pasar un id válido";
        public const string SHOW_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un show innerId por id";
        public const string SHOW_GET_INNERID_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un show innerId por nombre debe pasar un nombre válido";
        public const string SHOW_GET_INNERID_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un show innerId por nombre";
        public const string SHOW_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para borrar un show por id debe pasar un id válido";
        public const string SHOW_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un show debe pasar un show que exista previamente";
        public const string SHOW_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un show";




        //Salas
        public const string INVALID_SALA_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todas las salas";
        public const string SALA_GET_ALL_EXCEPTION = "Falla al querer obtener todas las salas";
        public const string SALA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener una sala por id debe pasar un id válido";
        public const string SALA_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una sala por id";
        public const string SALA_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener una sala por nombre debe pasar un nombre válido";
        public const string SALA_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una sala por nombre";
        public const string SALA_GET_BY_CIUDAD_INVALID_PARAMETER_EXCEPTION = "Para obtener una sala por ciudad debe pasar una provincia válida";
        public const string SALA_GET_BY_CIUDAD_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una sala por ciudad";
        public const string SALA_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar una sala debe pasar una sala válida";
        public const string SALA_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar una sala debe pasar una sala que no exista previamente";
        public const string SALA_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear una sala nueva";
        public const string SALA_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar una sala debe pasar una sala válida";
        public const string SALA_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar una sala debe pasar una sala que exista previamente";
        public const string SALA_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar una sala";
        public const string SALA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener una sala innerId por id debe pasar un id válido";
        public const string SALA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una sala innerId por id";
        public const string SALA_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para borrar una sala por id debe pasar un id válido";
        public const string SALA_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar una sala debe pasar una sala que exista previamente";
        public const string SALA_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar una sala";



        //Fecha
        public const string INVALID_FECHA_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todas las fechas";
        public const string FECHA_GET_ALL_EXCEPTION = "Falla al querer obtener todas las fechas";
        public const string FECHA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener una fehca por id debe pasar un id válido";
        public const string FECHA_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una fecha por id";
        public const string FECHA_GET_BY_NOMBRE_SHOW_INVALID_PARAMETER_EXCEPTION = "Para obtener fehcas por nombre de show debe pasar un nombre de show válido";
        public const string FECHA_GET_BY_NOMBRE_SHOW_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por nombre de show";
        public const string FECHA_GET_BY_PROVINCIA_INVALID_PARAMETER_EXCEPTION = "Para obtener fechas por provincia debe pasar una provincia válida";
        public const string FECHA_GET_BY_PROVINCIA_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por provincia";
        public const string FECHA_GET_BY_NOMBRE_SALA_INVALID_PARAMETER_EXCEPTION = "Para obtener fechas por nombre de sala debe pasar un nombre de sala válido";
        public const string FECHA_GET_BY_NOMBRE_SALA_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por nombre de sala";
        public const string FECHA_GET_BY_ID_SALA_INVALID_PARAMETER_EXCEPTION = "Para obtener fechas por id de sala debe pasar un id de sala válido";
        public const string FECHA_GET_BY_ID_SALA_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por id de sala";
        public const string FECHA_GET_BY_ID_CIUDAD_INVALID_PARAMETER_EXCEPTION = "Para obtener fechas por id de ciudad debe pasar un id de sala válido";
        public const string FECHA_GET_BY_ID_CIUDAD_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por id de ciudad";
        public const string FECHA_GET_BY_SALA_Y_HORARIO_SALA_INVALID_PARAMETER_EXCEPTION = "Para obtener fechas por sala y horario debe pasar una sala y un horario válido";
        public const string FECHA_GET_BY_SALA_Y_HORARIO_SALA_INVALID_SEARCH_EXCEPTION = "Error al querer buscar fechas por sala y horario";
        public const string FECHA_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar una fecha debe pasar una fecha válida";
        public const string FECHA_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar una fecha debe pasar una fecha que no exista previamente";
        public const string FECHA_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear una fecha nueva";
        public const string FECHA_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar una fecha debe pasar una fecha válida";
        public const string FECHA_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar una fecha debe pasar una fecha que exista previamente";
        public const string FECHA_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar una fecha";
        public const string FECHA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener una fecha innerId por id debe pasar un id válido";
        public const string FECHA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar una fecha innerId por id";
        public const string FECHA_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para borrar una fecha por id debe pasar un id válido";
        public const string FECHA_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar una fecha debe pasar una fecha que exista previamente";
        public const string FECHA_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar una fecha";



        //User
        public const string INVALID_USER_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los usuarios";
        public const string USER_GET_ALL_EXCEPTION = "Falla al querer obtener todos los usuarios";
        public const string USER_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un usuario por id debe pasar un id válido";
        public const string USER_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un usuario por id";
        public const string USER_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un usuario por nombre debe pasar un nombre válido";
        public const string USER_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un usuario por nombre";
        public const string USER_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un usuario debe pasar un usuario válido";
        public const string USER_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un usuario debe pasar un usuario que no exista previamente";
        public const string USER_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un usuario nuevo";
        public const string USER_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un usuario innerId por id debe pasar un id válido";
        public const string USER_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un usuario innerId por id";
        public const string USER_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un usuario debe pasar un usuario válido";
        public const string USER_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un usuario debe pasar un usuario que exista previamente";
        public const string USER_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un usuario";
        public const string USER_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para borrar un usuario por id debe pasar un id válido";
        public const string USER_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un usuario debe pasar un usuario que exista previamente";
        public const string USER_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un usuario";


        //Logs
        public const string INVALID_LOG_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los logs";
        public const string LOG_GET_ALL_EXCEPTION = "Falla al querer obtener todos los logs";
        public const string LOG_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un log debe pasar un log válido";
        public const string LOG_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un log nuevo";



        //Hotel
        public const string INVALID_HOTEL_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los hoteles";
        public const string HOTEL_GET_ALL_EXCEPTION = "Falla al querer obtener todos los hoteles";
        public const string HOTEL_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un hotel por id debe pasar un id válido";
        public const string HOTEL_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un hotel por id";
        public const string HOTEL_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un hotel por nombre debe pasar un nombre válido";
        public const string HOTEL_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un hotel por nombre";
        public const string HOTEL_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un hotel innerId por id debe pasar un id válido";
        public const string HOTEL_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un hotel innerId por id";
        public const string HOTEL_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un hotel debe pasar un hotel válido";
        public const string HOTEL_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un hotel debe pasar un hotel que no exista previamente";
        public const string HOTEL_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un hotel nuevo";
        public const string HOTEL_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un hotel debe pasar un hotel válido";
        public const string HOTEL_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un hotel debe pasar un hotel que exista previamente";
        public const string HOTEL_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un hotel";
        public const string HOTEL_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un hotel por id debe pasar un id válido";
        public const string HOTEL_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un hotel debe pasar un hotel que exista previamente";
        public const string HOTEL_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un hotel";


        //Restaurante
        public const string INVALID_RESTAURANTE_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los restaurantes";
        public const string RESTAURANTE_GET_ALL_EXCEPTION = "Falla al querer obtener todos los restaurantes";
        public const string RESTAURANTE_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un restaurante por id debe pasar un id válido";
        public const string RESTAURANTE_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un restaurante por id";
        public const string RESTAURANTE_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un restaurante por nombre debe pasar un nombre válido";
        public const string RESTAURANTE_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un restaurante por nombre";
        public const string RESTAURANTE_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un restaurante innerId por id debe pasar un id válido";
        public const string RESTAURANTE_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un restaurante innerId por id";
        public const string RESTAURANTE_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un restaurante debe pasar un restaurante válido";
        public const string RESTAURANTE_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un restaurante debe pasar un restaurante que no exista previamente";
        public const string RESTAURANTE_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un restaurante nuevo";
        public const string RESTAURANTE_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un restaurante debe pasar un restaurante válido";
        public const string RESTAURANTE_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un restaurante debe pasar un restaurante que exista previamente";
        public const string RESTAURANTE_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un restaurante";
        public const string RESTAURANTE_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un restaurante por id debe pasar un id válido";
        public const string RESTAURANTE_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un restaurante debe pasar un restaurante que exista previamente";
        public const string RESTAURANTE_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un restaurante";



        //Proveedor
        public const string INVALID_PROVEEDOR_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los proveedores";
        public const string PROVEEDOR_GET_ALL_EXCEPTION = "Falla al querer obtener todos los proveedores";
        public const string PROVEEDOR_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un proveedor por id debe pasar un id válido";
        public const string PROVEEDOR_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un proveedor por id";
        public const string PROVEEDOR_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un proveedor por nombre debe pasar un nombre válido";
        public const string PROVEEDOR_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un proveedor por nombre";
        public const string PROVEEDOR_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un proveedor innerId por id debe pasar un id válido";
        public const string PROVEEDOR_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un proveedor innerId por id";
        public const string PROVEEDOR_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un proveedor debe pasar un proveedor válido";
        public const string PROVEEDOR_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un proveedor debe pasar un proveedor que no exista previamente";
        public const string PROVEEDOR_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un proveedor nuevo";
        public const string PROVEEDOR_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un proveedor debe pasar un proveedor válido";
        public const string PROVEEDOR_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un proveedor debe pasar un proveedor que exista previamente";
        public const string PROVEEDOR_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un proveedor";
        public const string PROVEEDOR_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un proveedor por id debe pasar un id válido";
        public const string PROVEEDOR_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un proveedor debe pasar un proveedor que exista previamente";
        public const string PROVEEDOR_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un proveedor";


        //Prensa
        public const string INVALID_PRENSA_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los prensa";
        public const string PRENSA_GET_ALL_EXCEPTION = "Falla al querer obtener todos los prensa";
        public const string PRENSA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un prensa por id debe pasar un id válido";
        public const string PRENSA_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un prensa por id";
        public const string PRENSA_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION = "Para obtener un prensa por nombre debe pasar un nombre válido";
        public const string PRENSA_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un prensa por nombre";
        public const string PRENSA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un prensa innerId por id debe pasar un id válido";
        public const string PRENSA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un prensa innerId por id";
        public const string PRENSA_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un prensa debe pasar un prensa válido";
        public const string PRENSA_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un prensa debe pasar un prensa que no exista previamente";
        public const string PRENSA_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un prensa nuevo";
        public const string PRENSA_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un prensa debe pasar un prensa válido";
        public const string PRENSA_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un prensa debe pasar un prensa que exista previamente";
        public const string PRENSA_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un prensa";
        public const string PRENSA_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un prensa por id debe pasar un id válido";
        public const string PRENSA_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un prensa debe pasar un prensa que exista previamente";
        public const string PRENSA_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un prensa";




        //Votacion
        public const string INVALID_VOTACION_ES_CONNECTION_EXCEPTION = "db_connection_error";
        public const string VOTACION_GET_ALL_EXCEPTION = "get_votaciones_error";
        public const string VOTACION_GET_BY_IP_INVALID_PARAMETER_EXCEPTION = "invalid_ip";
        public const string VOTACION_GET_BY_SHOW_INVALID_PARAMETER_EXCEPTION = "invalid_show";
        public const string VOTACION_GET_BY_SHOW_AND_EMAIL_INVALID_PARAMETER_EXCEPTION = "invalid_email";
        public const string VOTACION_GET_BY_SHOW_AND_TEL_INVALID_PARAMETER_EXCEPTION = "invalid_tel";
        public const string VOTACION_GET_BY_SHOW_INVALID_SEARCH_EXCEPTION = "get_votacion_by_show_error";
        public const string VOTACION_GET_BY_SHOW_AND_TEL_INVALID_SEARCH_EXCEPTION = "get_votacion_by_show_and_tel_search_error";
        public const string VOTACION_GET_BY_SHOW_AND_MAIL_INVALID_SEARCH_EXCEPTION = "get_votacion_by_show_and_mail_search_error";
        public const string VOTACION_CREATE_INVALID_PARAMETER_EXCEPTION = "add_votacion_invalid_parameter_error";
        public const string VOTACION_CANT_MAX_EXCEPTION = "voto_ya_registrado_error";
        public const string VOTACION_CREATE_NOT_CREATED_EXCEPTION = "add_votacion_error";




        //Gasto
        public const string INVALID_GASTO_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch para obtener todos los gastos";
        public const string GASTO_GET_ALL_EXCEPTION = "Falla al querer obtener todos los gastos";
        public const string GASTO_GET_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un gasto por id";
        public const string GASTO_GET_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un gasto por id debe pasar un id válido";
        public const string GASTO_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION = "Para obtener un innerId de un gasto por id debe pasar un id válido";
        public const string GASTO_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un gasto innerId por id";
        public const string GASTO_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un gasto debe pasar un gasto válido";
        public const string GASTO_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un gasto debe pasar un gasto que no exista previamente";
        public const string GASTO_CREATE_NOT_CREATED_EXCEPTION = "Falla al querer crear un gasto nuevo";
        public const string GASTO_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un gasto debe pasar un gasto válido";
        public const string GASTO_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un gasto debe pasar un gasto que exista previamente";
        public const string GASTO_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un gasto";
        public const string GASTO_DELETE_INVALID_PARAMETER_EXCEPTION = "Para borrar un gasto por id debe pasar un id válido";
        public const string GASTO_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un gasto debe pasar un gasto que exista previamente";
        public const string GASTO_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un gasto";




        protected ElasticClient Client { get; set; }
        protected string Index { get; set; }

        public ESRepositorio(ESSettings settings, string index)
        {
            if (settings == null)
                throw new Exception(INVALID_SETTINGS_EXCEPTION);

            var config = new ConnectionSettings(settings.Node.Uri);
            Client = new ElasticClient(config);

            Index = index;
        }



        /*-------------------Standupero-------------------*/
        public List<Standupero> GetStanduperos()
        {
            var response = Client.Search<Standupero>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_STANDUPERO_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(STANDUPERO_GET_ALL_EXCEPTION);

            var standuperos = new List<Standupero>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standuperos.Add(item);
            }
            return standuperos;
        }
        public Standupero GetStanduperoByApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                throw new Exception(STANDUPERO_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Apellido).Query(apellido)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION);

            Standupero standupero = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standupero = item;
            }
            return standupero;
        }
        public Standupero GetStanduperoByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("dni", dni)));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_GET_BY_DNI_INVALID_SEARCH_EXCEPTION);

            Standupero standupero = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standupero = item;
            }
            return standupero;
        }
        public string GetStanduperoInnerIdByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("dni", dni))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddStandupero(Standupero standupero)
        {
            if (standupero == null)
                throw new Exception(STANDUPERO_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetStanduperoByDni(standupero.Dni);
            if (resultado != null)
                throw new Exception(STANDUPERO_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(standupero, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkStandupero(List<Standupero> standuperos)
        {
/*
            if (!IndexExists())
                CreateIndex();
*/
            var response = Client.IndexManyAsync(standuperos, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateStandupero(Standupero standupero)
        {
            if (standupero == null)
                throw new Exception(STANDUPERO_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(standupero.Dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(standupero, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(STANDUPERO_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteStandupero(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Standupero>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(STANDUPERO_DELETE_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllStanduperos()
        {
            Client.DeleteByQuery<Standupero>(q => q.Type(Index));
        }
        public InstagramUserInfoResponse GeStanduperoInstagramUserInfo(string instagramUsername)
        {
            var Client = new HttpClient()
            { BaseAddress = new Uri("https://www.instagram.com/") };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var request = instagramUsername;
            var response = Client.GetAsync(request).Result;
            InstagramUserInfoResponse igInfo = null;
            if (response.IsSuccessStatusCode)
            {
                var responseJson = response.Content.ReadAsStringAsync().Result;
                var stringObject = responseJson.Split(new string[] { "window._sharedData =" }, StringSplitOptions.None)[1]
                                               .Split(new string[] { "</script>" }, StringSplitOptions.None)[0];
                stringObject = stringObject.Remove(stringObject.Length - 1);
                igInfo = JsonConvert.DeserializeObject<InstagramUserInfoResponse>(stringObject);
            }
            return igInfo;
        }
        public InstagramUserInfoResponse GeStanduperoSocialBlade(string instagramUsername)
        {
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString("https://socialblade.com/instagram/user/darioorsi/legacy");
            }
            return null;
        }






        /*--------------------Productor-----------------------*/
        public List<Productor> GetProductores()
        {
            var response = Client.Search<Productor>(p => p
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_PRODUCTOR_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_GET_ALL_EXCEPTION);

            var productores = new List<Productor>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productores.Add(item);
            }
            return productores;
        }
        public Productor GetProductorByApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                throw new Exception(PRODUCTOR_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Productor>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Apellido).Query(apellido)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION);

            Productor productor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productor = item;
            }
            return productor;
        }
        public Productor GetProductorByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(PRODUCTOR_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Productor>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("dni", dni)));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_GET_BY_DNI_INVALID_SEARCH_EXCEPTION);

            Productor productor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productor = item;
            }
            return productor;
        }
        public string GetProductorInnerIdByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(PRODUCTOR_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Productor>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("dni", dni))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddProductor(Productor productor)
        {
            if (productor == null)
                throw new Exception(PRODUCTOR_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetProductorByDni(productor.Dni);
            if (resultado != null)
                throw new Exception(PRODUCTOR_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(productor, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkProductor(List<Productor> productores)
        {
/*
            if (!IndexExists())
                CreateIndex();
*/
            var response = Client.IndexManyAsync(productores, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateProductor(Productor productor)
        {
            if (productor == null)
                throw new Exception(PRODUCTOR_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetProductorInnerIdByDni(productor.Dni);
            if (innerId == null)
                throw new Exception(PRODUCTOR_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(productor, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(PRODUCTOR_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteProductor(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(PRODUCTOR_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetProductorInnerIdByDni(dni);
            if (innerId == null)
                throw new Exception(PRODUCTOR_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Productor>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(PRODUCTOR_DELETE_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllProductores()
        {
            Client.DeleteByQuery<Productor>(q => q.Type(Index));
        }





        /*--------------------Show-----------------------*/
        public List<Show> GetShows()
        {
            var response = Client.Search<Show>(p => p
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_SHOW_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(SHOW_GET_ALL_EXCEPTION);

            var shows = new List<Show>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    shows.Add(item);
            }
            return shows;
        }
        public Show GetShowByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(SHOW_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Show>(s => s
                .Index(Index)
                .Type(Index));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SHOW_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            return response.Documents.Where(i => i.Nombre == nombre).FirstOrDefault();
        }
        public Show GetShowById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SHOW_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Show>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("uniqueId", id)));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SHOW_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Show show = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    show = item;
            }
            return show;
        }
        public void AddShow(Show show)
        {
            if (show == null)
                throw new Exception(SHOW_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetShowByNombre(show.Nombre);
            if (resultado != null)
                throw new Exception(SHOW_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(show, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(SHOW_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkShow(List<Show> shows)
        {
/*
            if (!IndexExists())
                CreateIndex();
*/
            var response = Client.IndexManyAsync(shows, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(SHOW_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateShow(Show show)
        {
            if (show == null)
                throw new Exception(SHOW_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetShowInnerIdById(show.UniqueId);
            if (innerId == null)
                throw new Exception(SHOW_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(show, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(SHOW_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public string GetShowInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SHOW_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Show>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(SHOW_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public string GetShowInnerIdByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(SHOW_GET_INNERID_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Show>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("nombre", nombre))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(SHOW_GET_INNERID_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void DeleteShow(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SHOW_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetShowInnerIdById(id);
            if (innerId == null)
                throw new Exception(SHOW_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Show>(innerId, d => d
                                                    .Index(Index)
                                                    .Type(Index)
                                                    .Refresh(Refresh.True)
                                                    );
            if (!response.IsValid)
                throw new Exception(SHOW_DELETE_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllShows()
        {
            Client.DeleteByQuery<Show>(q => q.Type(Index));
        }





        /*--------------------Sala-----------------------*/
        public List<Sala> GetSalas()
        {
            var response = Client.Search<Sala>(p => p
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_SALA_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(SALA_GET_ALL_EXCEPTION);

            var salas = new List<Sala>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    salas.Add(item);
            }
            return salas;
        }
        public Sala GetSalaById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SALA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Sala>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SALA_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Sala sala = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    sala = item;
            }
            return sala;
        }
        public Sala GetSalaByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(SALA_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Sala>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q.Term("nombre", nombre))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SALA_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            Sala sala = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    sala = item;
            }
            return sala;
        }
        public List<Sala> GetSalasByCiudad(string ciudad)
        {
            if (string.IsNullOrEmpty(ciudad))
                throw new Exception(SALA_GET_BY_CIUDAD_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Sala>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Direccion.Ciudad).Query(ciudad)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SALA_GET_BY_CIUDAD_INVALID_SEARCH_EXCEPTION);

            var salas = new List<Sala>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Direccion.Ciudad.Trim() == ciudad.Trim())
                        salas.Add(item);
            }
            return salas;
        }
        public void AddSala(Sala sala)
        {
            if (sala == null)
                throw new Exception(SALA_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetSalaById(sala.UniqueId);
            if (resultado != null)
                throw new Exception(SALA_CREATE_ALREADY_EXISTS_EXCEPTION);

            var salaObtenidaPorNombre = GetSalaByNombre(sala.Nombre);
            if (salaObtenidaPorNombre != null && salaObtenidaPorNombre.Direccion.Provincia == sala.Direccion.Provincia)
                throw new Exception(SALA_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(sala, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(SALA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkSala(List<Sala> salas)
        {
            var response = Client.IndexManyAsync(salas, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(SALA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateSala(Sala sala)
        {
            if (sala == null)
                throw new Exception(SALA_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetSalaInnerIdById(sala.UniqueId);
            if (innerId == null)
                throw new Exception(SALA_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(sala, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(SALA_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public string GetSalaInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SALA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Sala>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(SALA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void DeleteSala(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SALA_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetSalaInnerIdById(id);
            if (innerId == null)
                throw new Exception(SALA_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Sala>(innerId, d => d
                                                    .Index(Index)
                                                    .Type(Index)
                                                    .Refresh(Refresh.True)
                                                    );
            if (!response.IsValid)
                throw new Exception(SALA_DELETE_NOT_DELETED_EXCEPTION);
        }
        public List<string> GetCiudadesInSalas()
        {
            var response = Client.Search<Sala>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Source(sr => sr
                .Includes(i => i
                    .Field(fi => fi
                        .Direccion.Ciudad)
                        )
                      )
                );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_CIUDAD_INVALID_SEARCH_EXCEPTION);

            var ciudades = new List<string>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (!ciudades.Contains(item.Direccion.Ciudad.Trim()))
                        ciudades.Add(item.Direccion.Ciudad.Trim());
                }
            }
            return ciudades;
        }






        /*-------------------Fecha-------------------*/

        public List<Fecha> GetFechas()
        {
            var response = Client.Search<Fecha>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_FECHA_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(FECHA_GET_ALL_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if(item.Status == null)
                        fechas.Add(item);
                }

            }
            return fechas;
        }
        public Fecha GetFechaById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(FECHA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Fecha fecha = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.UniqueId == id)
                    {
                        fecha = item;
                        break;
                    }
                }

            }
            return fecha;
        }
        public List<Fecha> GetFechasByShow(string nombreShow)
        {
            if (string.IsNullOrEmpty(nombreShow))
                throw new Exception(FECHA_GET_BY_NOMBRE_SHOW_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Show._Show).Query(nombreShow)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_NOMBRE_SHOW_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                        fechas.Add(item);
                }

            }
            return fechas;
        }
        public List<Fecha> GetFechasByShowId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(FECHA_GET_BY_NOMBRE_SHOW_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Show.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_NOMBRE_SHOW_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                        fechas.Add(item);
                }

            }
            return fechas;
        }
        public List<Fecha> GetFechasByProvincia(string nombreProvincia)
        {
            if (string.IsNullOrEmpty(nombreProvincia))
                throw new Exception(FECHA_GET_BY_PROVINCIA_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Sala.Direccion.Provincia).Query(nombreProvincia)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_PROVINCIA_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                        fechas.Add(item);
                }

            }
            return fechas;
        }
        public List<Fecha> GetFechasBySala(string nombreSala)
        {
            if (string.IsNullOrEmpty(nombreSala))
                throw new Exception(FECHA_GET_BY_NOMBRE_SALA_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Sala.Nombre).Query(nombreSala)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_NOMBRE_SALA_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                    {
                        if (item.Sala.Nombre.Trim() == nombreSala.Trim())
                        fechas.Add(item);
                    }

                }
            }
            return fechas;
        }
        public List<Fecha> GetFechasByIdSala(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m.Field(f => f.Sala.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                        fechas.Add(item);
                }

            }
            return fechas;
        }
        public List<Fecha> GetFechasByIdSalaAndIdShow(string idSala, string idShow)
        {
            if (string.IsNullOrEmpty(idShow) || string.IsNullOrEmpty(idSala))
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Sala.UniqueId).Query(idSala)
                        .Field(f => f.Show.UniqueId).Query(idShow)
                        )
                     )
                );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                    {
                        if (item.Sala.UniqueId == idSala && item.Show.UniqueId == idShow)
                        fechas.Add(item);
                    }
                }
            }
            return fechas;
        }
        public List<Fecha> GetFechasByCiudadAndIdShow(string ciudad, string idShow)
        {
            if (string.IsNullOrEmpty(idShow) || string.IsNullOrEmpty(ciudad))
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Sala.Direccion.Ciudad).Query(ciudad)
                        .Field(f => f.Show.UniqueId).Query(idShow)
                        )
                     )
                );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_SALA_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                    {
                        if (item.Sala.Direccion.Ciudad.Trim() == ciudad.Trim() && item.Show.UniqueId == idShow)
                        fechas.Add(item);
                    }
                }
            }
            return fechas;
        }
        public List<Fecha> GetFechasByCiudad(string ciudad, DateTime desde, DateTime hasta)
        {
            if (string.IsNullOrEmpty(ciudad))
                throw new Exception(FECHA_GET_BY_ID_CIUDAD_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .From(0)
                .Size(GetCount(Index))
                 .Query(q => q
                    .Match(m => m
                        .Field(f => f.Sala.Direccion.Ciudad).Query(ciudad))
                     )
             );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_ID_CIUDAD_INVALID_SEARCH_EXCEPTION);

            var fechas = new List<Fecha>();
            fechas.AddRange(response.Documents);
            fechas = fechas.Where(f => f.Status == null && f.Sala.Direccion.Ciudad == ciudad  && f.FechaHorario >= desde && f.FechaHorario <= hasta ).ToList();

            return fechas;
        }
        public Fecha GetFechaBySalaAndFechaAndHorario(string idSala, DateTime fechaYHorario)
        {
            if (string.IsNullOrEmpty(idSala) || fechaYHorario == null)
                throw new Exception(FECHA_GET_BY_SALA_Y_HORARIO_SALA_INVALID_PARAMETER_EXCEPTION);


            var response = Client.Search<Fecha>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Sala.UniqueId).Query(idSala)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_BY_SALA_Y_HORARIO_SALA_INVALID_SEARCH_EXCEPTION);

            Fecha fecha = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Status == null)
                    {
                        if (item.FechaHorario == fechaYHorario)
                            fecha = item;
                    }
                }
            }
            return fecha;
        }
        public void AddFecha(Fecha fecha)
        {
            if (fecha == null)
                throw new Exception(FECHA_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetFechaById(fecha.UniqueId);
            if (resultado != null)
                throw new Exception(FECHA_CREATE_ALREADY_EXISTS_EXCEPTION);

            var fechaObtenida = GetFechaBySalaAndFechaAndHorario(fecha.Sala.UniqueId, fecha.FechaHorario);
            if (fechaObtenida != null)
                throw new Exception(FECHA_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(fecha, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(FECHA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkFecha(List<Fecha> fechas)
        {
            var response = Client.IndexManyAsync(fechas, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(FECHA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateFecha(Fecha fecha)
        {
            if (fecha == null)
                throw new Exception(FECHA_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetFechaInnerIdById(fecha.UniqueId);
            if (innerId == null)
                throw new Exception(FECHA_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(fecha, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(FECHA_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public string GetFechaInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(FECHA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Fecha>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(FECHA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void DeleteFecha(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(FECHA_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetFechaInnerIdById(id);
            if (innerId == null)
                throw new Exception(FECHA_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Fecha>(innerId, d => d
                                                    .Index(Index)
                                                    .Type(Index)
                                                    .Refresh(Refresh.True)
                                                    );
            if (!response.IsValid)
                throw new Exception(FECHA_DELETE_NOT_DELETED_EXCEPTION);
        }

        public List<Fecha> GetFechaForGoogleCalendarAction()
        {
            var response = Client.Search<Fecha>(s => s
               .Index(Index)
               .Type(Index)
               .From(0)
               .Size(GetCount(Index))
              );

            if (response == null)
                throw new Exception(INVALID_FECHA_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(FECHA_GET_ALL_EXCEPTION);

            var fechas = new List<Fecha>();

            fechas.AddRange(response.Documents);
            fechas = fechas.Where(f=>f.GoogleCalendarState != null && f.GoogleCalendarState != "ok").ToList();
            return fechas;
        }




        /*-------------------Usuarios-------------------*/

        public List<UserModel> GetUsuarios()
        {
            var response = Client.Search<UserModel>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_USER_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(USER_GET_ALL_EXCEPTION);

            var users = new List<UserModel>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    users.Add(item);
            }
            return users;
        }
        public UserModel GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(USER_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<UserModel>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(USER_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            UserModel user = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    user = item;
            }
            return user;
        }
        public UserModel GetUserByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(USER_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<UserModel>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Username).Query(nombre)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(USER_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            UserModel user = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Username == nombre)
                        user = item;
            }
            return user;
        }
        public UserModel GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception(USER_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<UserModel>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.MailRecover).Query(email)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(USER_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            UserModel user = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.MailRecover == email)
                        user = item;
                }
            }
            return user;
        }
        public void AddUser(UserModel user)
        {
            if (user == null)
                throw new Exception(USER_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
            {
                CreateIndex();
            }
            else
            {
                var resultado = GetUserById(user.UniqueId);
                if (resultado != null)
                    throw new Exception(USER_CREATE_ALREADY_EXISTS_EXCEPTION);

                var userObtenidaPorNombre = GetUserByNombre(user.Username);
                if (userObtenidaPorNombre != null)
                    throw new Exception(USER_CREATE_ALREADY_EXISTS_EXCEPTION);

                var userObtenidaPorEmail = GetUserByEmail(user.MailRecover);
                if (userObtenidaPorEmail != null)
                    throw new Exception(USER_CREATE_ALREADY_EXISTS_EXCEPTION);
            }

            var response = Client.IndexAsync(user, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(USER_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkUser(List<UserModel> usuarios)
        {
            var response = Client.IndexManyAsync(usuarios, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(USER_CREATE_NOT_CREATED_EXCEPTION);
        }
        public string GetUserInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(USER_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<UserModel>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(USER_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void UpdateUser(UserModel user)
        {
            if (user == null)
                throw new Exception(USER_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetUserInnerIdById(user.UniqueId);
            if (innerId == null)
                throw new Exception(USER_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(user, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(USER_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(USER_DELETE_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetUserInnerIdById(id);
            if (innerId == null)
                throw new Exception(USER_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<UserModel>(innerId, d => d
                                                    .Index(Index)
                                                    .Type(Index)
                                                    .Refresh(Refresh.True)
                                                    );
            if (!response.IsValid)
                throw new Exception(USER_DELETE_NOT_DELETED_EXCEPTION);
        }



        /*-------------------Logs-------------------*/
        public List<Log> GetLogs(string all)
        {

            int count = 0;
            if (all == "true")
                count = GetCount(Index);
            else
                count = 200;

            var response = Client.Search<Log>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(count)
                   .Sort(f => f.Descending(c => c.Fecha))
                  );

            if (response == null)
                throw new Exception(INVALID_LOG_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(LOG_GET_ALL_EXCEPTION);

            var logs = new List<Log>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    logs.Add(item);
            }
            return logs;
        }
        public void AddLog(Log log)
        {
            if (log == null)
                throw new Exception(LOG_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();


            var response = Client.IndexAsync(log, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(LOG_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkLog(List<Log> logs)
        {
            var response = Client.IndexManyAsync(logs, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(LOG_CREATE_NOT_CREATED_EXCEPTION);
        }



        /*-------------------Hoteles-------------------*/
        public List<Hotel> GetHoteles()
        {
            var response = Client.Search<Hotel>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_HOTEL_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(HOTEL_GET_ALL_EXCEPTION);

            var hoteles = new List<Hotel>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    hoteles.Add(item);
            }
            return hoteles;
        }
        public Hotel GetHotelById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(HOTEL_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Hotel>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(HOTEL_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Hotel hotel = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    hotel = item;
            }
            return hotel;
        }
        public Hotel GetHotelByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(HOTEL_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Hotel>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Nombre).Query(nombre)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(HOTEL_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            Hotel hotel = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Nombre == nombre)
                        hotel = item;
            }
            return hotel;
        }
        public string GetHotelInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(HOTEL_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Hotel>(s => s
               .Index(Index)
               .Type(Index)
               .Query(q => q
                   .Match(m => m.Field(f => f.UniqueId).Query(id)))
                   );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(HOTEL_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddHotel(Hotel hotel)
        {
            if (hotel == null)
                throw new Exception(HOTEL_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetHotelByNombre(hotel.Nombre);
            if (resultado != null)
                throw new Exception(HOTEL_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(hotel, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(HOTEL_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkHotel(List<Hotel> hoteles)
        {
            var response = Client.IndexManyAsync(hoteles, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(HOTEL_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateHotel(Hotel hotel)
        {
            if (hotel == null)
                throw new Exception(HOTEL_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetHotelInnerIdById(hotel.UniqueId);
            if (innerId == null)
                throw new Exception(HOTEL_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(hotel, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(HOTEL_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteHotel(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(HOTEL_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetHotelInnerIdById(id);
            if (innerId == null)
                throw new Exception(HOTEL_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Hotel>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(HOTEL_DELETE_NOT_DELETED_EXCEPTION);
        }



        /*-------------------Restaurantes-------------------*/
        public List<Restaurante> GetRestaurantes()
        {
            var response = Client.Search<Restaurante>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_RESTAURANTE_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_GET_ALL_EXCEPTION);

            var restaurantes = new List<Restaurante>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    restaurantes.Add(item);
            }
            return restaurantes;
        }
        public Restaurante GetRestauranteById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(RESTAURANTE_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Restaurante>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Restaurante restaurante = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    restaurante = item;
            }
            return restaurante;
        }
        public Restaurante GetRestauranteByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(RESTAURANTE_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Restaurante>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Nombre).Query(nombre)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            Restaurante restaurante = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Nombre == nombre)
                        restaurante = item;
            }
            return restaurante;
        }
        public string GetRestauranteInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(RESTAURANTE_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Restaurante>(s => s
               .Index(Index)
               .Type(Index)
               .Query(q => q
                   .Match(m => m.Field(f => f.UniqueId).Query(id)))
                   );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddRestaurante(Restaurante restaurante)
        {
            if (restaurante == null)
                throw new Exception(RESTAURANTE_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetRestauranteByNombre(restaurante.Nombre);
            if (resultado != null)
                throw new Exception(RESTAURANTE_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(restaurante, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkRestaurante(List<Restaurante> restaurantes)
        {
            var response = Client.IndexManyAsync(restaurantes, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(RESTAURANTE_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateRestaurante(Restaurante restaurante)
        {
            if (restaurante == null)
                throw new Exception(RESTAURANTE_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetRestauranteInnerIdById(restaurante.UniqueId);
            if (innerId == null)
                throw new Exception(RESTAURANTE_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(restaurante, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(RESTAURANTE_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteRestaurante(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(RESTAURANTE_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetRestauranteInnerIdById(id);
            if (innerId == null)
                throw new Exception(RESTAURANTE_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Restaurante>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(RESTAURANTE_DELETE_NOT_DELETED_EXCEPTION);
        }




        /*-------------------Proveedores-------------------*/
        public List<Proveedor> GetProveedores()
        {
            var response = Client.Search<Proveedor>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_PROVEEDOR_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_GET_ALL_EXCEPTION);

            var proveedores = new List<Proveedor>();
            proveedores.AddRange(response.Documents);
            return proveedores;
        }
        public Proveedor GetProveedorById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PROVEEDOR_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Proveedor>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Proveedor proveedor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if(item.UniqueId == id)
                        proveedor = item;
                }

            }
            return proveedor;
        }
        public Proveedor GetProveedorByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(PROVEEDOR_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Proveedor>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Nombre).Query(nombre)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            Proveedor proveedor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Nombre == nombre)
                    {
                        proveedor = item;
                        break;
                    }
            }
            return proveedor;
        }
        public string GetProveedorInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PROVEEDOR_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Proveedor>(s => s
               .Index(Index)
               .Type(Index)
               .Query(q => q
                   .Match(m => m.Field(f => f.UniqueId).Query(id)))
                   );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new Exception(PROVEEDOR_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetProveedorByNombre(proveedor.Nombre);
            if (resultado != null)
                throw new Exception(PROVEEDOR_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(proveedor, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkProveedor(List<Proveedor> proveedores)
        {
            var response = Client.IndexManyAsync(proveedores, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(PROVEEDOR_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new Exception(PROVEEDOR_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetProveedorInnerIdById(proveedor.UniqueId);
            if (innerId == null)
                throw new Exception(PROVEEDOR_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(proveedor, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(PROVEEDOR_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteProveedor(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PROVEEDOR_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetProveedorInnerIdById(id);
            if (innerId == null)
                throw new Exception(PROVEEDOR_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Proveedor>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(PROVEEDOR_DELETE_NOT_DELETED_EXCEPTION);
        }


        /*-------------------Prensa-------------------*/
        public List<Prensa> GetPrensa()
        {
            var response = Client.Search<Prensa>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_PRENSA_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(PRENSA_GET_ALL_EXCEPTION);

            var prensa = new List<Prensa>();
            prensa.AddRange(response.Documents);
            return prensa;
        }
        public Prensa GetPrensaById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PRENSA_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Prensa>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.UniqueId).Query(id)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PRENSA_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            Prensa prensa = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.UniqueId == id)
                    {
                        prensa = item;
                        break;
                    }

                }
            }
            return prensa;
        }
        public Prensa GetPrensaByNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception(PRENSA_GET_BY_NOMBRE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Prensa>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Nombre).Query(nombre)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(PRENSA_GET_BY_NOMBRE_INVALID_SEARCH_EXCEPTION);

            Prensa prensa = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (item.Nombre == nombre)
                    {
                        prensa = item;
                        break;
                    }
            }
            return prensa;
        }
        public string GetPrensaInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PRENSA_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Prensa>(s => s
               .Index(Index)
               .Type(Index)
               .Query(q => q
                   .Match(m => m.Field(f => f.UniqueId).Query(id)))
                   );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(PRENSA_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddPrensa(Prensa prensa)
        {
            if (prensa == null)
                throw new Exception(PRENSA_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetPrensaByNombre(prensa.Nombre);
            if (resultado != null)
                throw new Exception(PRENSA_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(prensa, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(PRENSA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkPrensa(List<Prensa> prensa)
        {
            var response = Client.IndexManyAsync(prensa, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(PRENSA_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdatePrensa(Prensa prensa)
        {
            if (prensa == null)
                throw new Exception(PRENSA_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetPrensaInnerIdById(prensa.UniqueId);
            if (innerId == null)
                throw new Exception(PRENSA_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(prensa, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(PRENSA_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeletePrensa(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(PRENSA_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetPrensaInnerIdById(id);
            if (innerId == null)
                throw new Exception(PRENSA_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Prensa>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(PRENSA_DELETE_NOT_DELETED_EXCEPTION);
        }



        /*-------------------Votacion-------------------*/

        public List<Votacion> GetVotacionesViejo(int skip, int take)
        {
            var response = Client.Search<Votacion>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(skip)
                   .Size(take)
                  );

            if (response == null)
                throw new Exception(INVALID_VOTACION_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(VOTACION_GET_ALL_EXCEPTION);

            var votaciones = new List<Votacion>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    votaciones.Add(item);
            }
            return votaciones;
        }
        public List<Votacion> GetVotacionesByShowViejo(string show)
        {
            if (string.IsNullOrEmpty(show))
                throw new Exception(VOTACION_GET_BY_SHOW_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Votacion>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(9999)
                   .Query(q => q
                   .Match(m => m.Field(f => f.Show).Query(show)))
                   );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(VOTACION_GET_BY_SHOW_INVALID_SEARCH_EXCEPTION);

            var votaciones = new List<Votacion>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (show == item.Show)
                        votaciones.Add(item);
            }
            return votaciones;
        }
        public Votacion GetVotacionesByEmailAndShow(string email, string show)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception(VOTACION_GET_BY_SHOW_AND_EMAIL_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Votacion>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q
                    .Match(m => m.Field(f => f.Email).Query(email)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(VOTACION_GET_BY_SHOW_AND_MAIL_INVALID_SEARCH_EXCEPTION);

            Votacion votacion = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (email == item.Email && show == item.Show)
                    {
                        votacion = item;
                        break;
                    }
            }
            return votacion;
        }
        public Votacion GetVotacionesByTelAndShow(string tel, string show)
        {
            if (string.IsNullOrEmpty(tel))
                throw new Exception(VOTACION_GET_BY_SHOW_AND_TEL_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Votacion>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q
                    .Match(m => m.Field(f => f.Telefono).Query(tel)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(VOTACION_GET_BY_SHOW_AND_TEL_INVALID_SEARCH_EXCEPTION);

            Votacion votacion = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (tel == item.Telefono && show == item.Show)
                    {
                        votacion = item;
                        break;
                    }
            }
            return votacion;
        }
        public List<Votacion> GetVotacionesByIpAndShow(string ip, string show)
        {
            if (string.IsNullOrEmpty(ip))
                throw new Exception(SHOW_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Votacion>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("ip", ip)));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(SHOW_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            var votaciones = new List<Votacion>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    if (ip == item.Ip && show == item.Show)
                        votaciones.Add(item);
            }
            return votaciones;
        }
        public void AddVotacion(Votacion votacion)
        {
            if (votacion == null)
                throw new Exception(VOTACION_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var votaciones = GetVotacionesByIpAndShow(votacion.Ip, votacion.Show);
            if (votaciones != null && votaciones.Count >= 3)
                throw new Exception(VOTACION_CANT_MAX_EXCEPTION);

            var votacionObtenida = GetVotacionesByEmailAndShow(votacion.Email, votacion.Show);
            if (votacionObtenida != null)
                throw new Exception(VOTACION_CANT_MAX_EXCEPTION);

            var votacionObtenidaTel = GetVotacionesByTelAndShow(votacion.Telefono, votacion.Show);
            if (votacionObtenidaTel != null)
                throw new Exception(VOTACION_CANT_MAX_EXCEPTION);

            var response = Client.IndexAsync(votacion, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(VOTACION_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkVotacion(List<Votacion> votaciones)
        {
            var response = Client.IndexManyAsync(votaciones, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(VOTACION_CREATE_NOT_CREATED_EXCEPTION);
        }
        public List<Votacion> GetVotaciones()
        {
            var response = Client.Search<Votacion>
                (scr => scr.Index(Index)
                     .From(0)
                     .Take(10000)
                     .MatchAll()
                     .Scroll("2m"));
            var votaciones = new List<Votacion>();

            if (!response.IsValid || string.IsNullOrEmpty(response.ScrollId))
                throw new Exception(VOTACION_GET_ALL_EXCEPTION);

            if (response.Documents.Any())
                votaciones.AddRange(response.Documents);

            string scrollid = response.ScrollId;
            bool isScrollSetHasData = true;
            while (isScrollSetHasData)
            {
                var loopingResponse = Client.Scroll<Votacion>("2m", scrollid);
                if (loopingResponse.IsValid)
                {
                    votaciones.AddRange(loopingResponse.Documents);
                    scrollid = loopingResponse.ScrollId;
                }
                isScrollSetHasData = loopingResponse.Documents.Any();
            }

            Client.ClearScroll(new ClearScrollRequest(scrollid));
            return votaciones;
        }
        public List<Votacion> GetVotacionesByShow(string show)
        {
            var response = Client.Search<Votacion>
                (scr => scr.Index(Index)
                     .From(0)
                     .Take(10000)
                     .Query(q => q
                        .Match(m => m.Field(f => f.Show).Query(show)))
                     .Scroll("2m"));
            var votaciones = new List<Votacion>();

            if (!response.IsValid || string.IsNullOrEmpty(response.ScrollId))
                throw new Exception(VOTACION_GET_ALL_EXCEPTION);

            if (response.Documents.Any())
            {
                foreach (var item in response.Documents)
                {
                    if(item.Show == show)
                        votaciones.Add(item);
                }
            }

            string scrollid = response.ScrollId;
            bool isScrollSetHasData = true;
            while (isScrollSetHasData)
            {
                var loopingResponse = Client.Scroll<Votacion>("2m", scrollid);
                if (loopingResponse.IsValid)
                {
                    foreach (var item in loopingResponse.Documents)
                    {
                        votaciones.Add(item);
                    }
                    scrollid = loopingResponse.ScrollId;
                }
                isScrollSetHasData = loopingResponse.Documents.Any();
            }

            Client.ClearScroll(new ClearScrollRequest(scrollid));
            return votaciones;
        }


        /*-------------------Instagram Users-------------------*/

        public List<InstagramUser> GetInstagramUsers()
        {
            var response = Client.Search<InstagramUser>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_USER_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(USER_GET_ALL_EXCEPTION);

            return response.Documents.ToList();
        }
        public InstagramUser GetInstagramUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception(USER_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<InstagramUser>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Username).Query(username)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(USER_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            InstagramUser user = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                {
                    if (item.Username == username)
                    {
                        user = item;
                        break;
                    }
                }

            }
            return user;
        }
        public void AddInstagramUser(InstagramUser user)
        {
            if (user == null)
                throw new Exception(USER_CREATE_INVALID_PARAMETER_EXCEPTION);

            var response = Client.IndexAsync(user, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(USER_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkInstagramUser(List<InstagramUser> usuarios)
        {
            var response = Client.IndexManyAsync(usuarios, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(USER_CREATE_NOT_CREATED_EXCEPTION);
        }
        public string GetInstagramUserInnerIdByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception(USER_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<InstagramUser>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("username", username))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(USER_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void UpdateInstagramUser(InstagramUser user)
        {
            if (user == null)
                throw new Exception(USER_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetInstagramUserInnerIdByUsername(user.Username);
            if (innerId == null)
                throw new Exception(USER_UPDATE_NOT_EXISTS_EXCEPTION);


            var result = Client.Index(user, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(USER_UPDATE_NOT_UPDATED_EXCEPTION);
        }



        /*-------------------Gasto-------------------*/
        public List<Gasto> GetGastos()
        {
            var response = Client.Search<Gasto>(s => s
                   .Index(Index)
                   .Type(Index)
                   .From(0)
                   .Size(GetCount(Index))
                  );

            if (response == null)
                throw new Exception(INVALID_GASTO_ES_CONNECTION_EXCEPTION);

            if (!response.IsValid)
                throw new Exception(GASTO_GET_ALL_EXCEPTION);

            return response.Documents.ToList();
        }
        public Gasto GetGastoById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(GASTO_GET_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Gasto>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("uniqueId", id)));

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(GASTO_GET_BY_ID_INVALID_SEARCH_EXCEPTION);

            return response.Documents.Where(g => g.UniqueId == id).ToList().FirstOrDefault();
        }
        public string GetGastoInnerIdById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(GASTO_GET_INNERID_BY_ID_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Gasto>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (!response.IsValid)
                throw new Exception(GASTO_GET_INNERID_BY_ID_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddGasto(Gasto gasto)
        {
            if (gasto == null)
                throw new Exception(GASTO_CREATE_INVALID_PARAMETER_EXCEPTION);

            if (!IndexExists())
                CreateIndex();

            var resultado = GetGastoById(gasto.UniqueId);
            if (resultado != null)
                throw new Exception(GASTO_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(gasto, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(GASTO_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void AddBulkGasto(List<Gasto> gastos)
        {
            var response = Client.IndexManyAsync(gastos, Index, Index).Result;

            if (!response.IsValid)
                throw new Exception(GASTO_CREATE_NOT_CREATED_EXCEPTION);
        }
        public void UpdateGasto(Gasto gasto)
        {
            if (gasto == null)
                throw new Exception(GASTO_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetGastoInnerIdById(gasto.UniqueId);
            if (innerId == null)
                throw new Exception(GASTO_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(gasto, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(GASTO_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteGasto(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(GASTO_DELETE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetGastoInnerIdById(id);
            if (innerId == null)
                throw new Exception(GASTO_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Gasto>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(GASTO_DELETE_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllGastos()
        {
            Client.DeleteByQuery<Gasto>(q => q.Type(Index));
        }




        /*---------Metodos genericos------------------*/

        public int GetCount(string tipo)
        {
            ICountResponse response = null;
            if (tipo == "standupero")
                response = Client.Count<Standupero>(c => c.Index(Index).Type(Index));
            else if (tipo == "productor")
                response = Client.Count<Productor>(c => c.Index(Index).Type(Index));
            else if (tipo == "sala")
                response = Client.Count<Sala>(c => c.Index(Index).Type(Index));
            else if (tipo == "fecha")
                response = Client.Count<Fecha>(c => c.Index(Index).Type(Index));
            else if (tipo == "show")
                response = Client.Count<Show>(c => c.Index(Index).Type(Index));
            else if (tipo == "user")
                response = Client.Count<UserModel>(c => c.Index(Index).Type(Index));
            else if (tipo == "log")
                response = Client.Count<Log>(c => c.Index(Index).Type(Index));
            else if (tipo == "hotel")
                response = Client.Count<Hotel>(c => c.Index(Index).Type(Index));
            else if (tipo == "restaurante")
                response = Client.Count<Restaurante>(c => c.Index(Index).Type(Index));
            else if (tipo == "proveedor")
                response = Client.Count<Proveedor>(c => c.Index(Index).Type(Index));
            else if (tipo == "prensa")
                response = Client.Count<Prensa>(c => c.Index(Index).Type(Index));
            else if (tipo == "votacion")
                response = Client.Count<Votacion>(c => c.Index(Index).Type(Index));
            else if (tipo == "instagramuser")
                response = Client.Count<InstagramUser>(c => c.Index(Index).Type(Index));
            else if (tipo == "gasto")
                response = Client.Count<Gasto>(c => c.Index(Index).Type(Index));
            return (int)response.Count;
        }
        public void DeleteIndex()
        {
            if (IndexExists())
            {
                var response = Client.DeleteIndex(Index);
                if (!response.IsValid)
                    throw new Exception("delte_index_exception");
            }
        }
        public bool IndexExists()
        {
            var response = Client.IndexExists(Index);
            return response.Exists;
        }
        public void CreateIndex()
        {
            Client.CreateIndex(Index, c => c
                       .Settings(s => s
                         .NumberOfShards(5)
                         .NumberOfReplicas(5))
                   );
        }

        public enum ContentType
        {
            standupero,
            show,
            sala,
            productor,
            fecha,
            user,
            log,
            hotel,
            restaurante,
            proveedor,
            prensa,
            votacion,
            instagramuser,
            gasto
        }
    }
}