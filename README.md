# 📝 Memoria de Proyecto "DePan"

Este repositorio recoge el progreso semanal del proyecto **DePan**, una aplicación para navegador que servirá para gestionar una panadería.

## 👤 Autor
  - Nombre: Denis Fernández Castro.
- Estudiante de 2º DAM en Colegio Vivas, Vigo.

## 📖 Descripción del Proyecto
El objetivo es crear una aplicación que permita a los clientes hacer pedidos de manera on-line, siendo así que puedan visualizar un catálogo con todos los productos de la panadería,
elegir una cantidad de los mismos, elegir su forma de pago y por ultimo confirmar el pedido, el cual será enviado a los repartidores.

## 🛠️ Tecnologías
- Lenguaje: C#
- IDE: Visual Studio Code 
- Framework : .NET 8
- Control de versiones: Git + GitHub y GitHubDesktop

## 📅 Progreso Semanal
Semana 1 (29/09/2025 - 05/10/2025)

    ✅ Definición de metas y objetivos del proyecto

    ✅ Diseño del modelo entidad-relación en MySQL

    ✅ Documentación inicial en el repositorio de GitHub

    ✅ Creación de la base de datos depan_bd

    ✅ Configuración inicial del README.md

Semana 2 (06/10/2025 - 12/10/2025)

    ✅ Configuración del entorno de desarrollo (Visual Studio Code)

    ✅ Creación del proyecto ASP.NET Core MVC

    ✅ Configuración de Entity Framework con MySQL

    ✅ Scaffolding de modelos desde base de datos existente en HeidiSQL nombre: depan_db

    ✅ Implementación de la estructura base del proyecto

    ✅ Configuración de dependencias (Pomelo.EntityFrameworkCore.MySql, AutoMapper)

    ✅ Conexión exitosa a la base de datos depan_db

    ✅ Organización y subida del código a GitHub
    
    ℹ️ (Cambio de Visual Studio 2022 a VSC por comodidad. Es el IDE al que estoy más acostumbrado y el que estoy utilizando actualmente en las prácticas.)
    
    ℹ️ Uso de la librería Pomelo.EntityFrameworkCore.MySql
      Explicación:
        La uso para conectar la aplicación ASP.NET Core con la base de datos (depan_db) y trabajar con ella mediante LINQ.
        La librería también me traduce las consultas LINQ de C# a SQL para MySQL, me permite crear migraciones y realizar scaffolding de los modelos de la base de datos, 
        para generar automáticamente las clases del modelo de datos (dentro de la carpeta ModelsDB) y el contexto (DepanDbContext) de la estructura de los mismos.
        

Semana 3 (13/10/2025 - 19/10/2025)

    ✅ Vistas para registro, login y bienvenida creadas y configuradas correctamente.
    
    #Sistema de Autenticación JWT Completo
    
    Modelos de Autenticación:
    
    ✅ LoginModel - Validación de credenciales
    ✅ RegisterModel - Registro de nuevos usuarios
    
    ✅ Generación de tokens seguros
    ✅ Configuración de claims personalizados
    
    ✅ Endpoints para Login, Register y Logout
    ✅ Hash seguro de contraseñas con SHA256
    
    ✅ Validación de usuarios existentes
    ✅ Página de Login con validación
    ✅ Página de Registro con formulario completo
    
    Configuración de Seguridad:
    
    ✅ Autenticación JWT configurada
    ✅ Clave secreta segura para tokens JWT
    ✅ Cookies HTTP-only para almacenamiento seguro añadidas

## 🚀 Instalación y Ejecución
# Clonar el repositorio
git clone https://github.com/DenisFernandez/DePan.git

# Navegar al directorio
cd DePan

# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run

## 📊 Base de Datos

    Nombre: depan_bd

    Gestor: MySQL 8.4 LTS

    Conexión: Configurada en appsettings.json
    
## 📞 Contacto: denisfernandezcastro12@gmail.com
## 🔗 Repositorio: https://github.com/DenisFernandez/DePan

    
