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
ℹ️ Recientemente empecé a utilizar GitHubDesktop, facilita mucho los commits y los push, es una herramienta muy cómoda e intuitiva.

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
    
Semana 4 (20/10/2025 - 27/10/2025)

    ✅ Implementación de Entidades con LINQ
    Servicio ProductoService con operaciones CRUD completas.
    Consultas LINQ para, obtener productos y categorías, filtrar por categoría y disponibilidad, nombre y descripción.
    
    ✅ Sistema Completo de Catálogo de Productos
    Vista Index: Catálogo público con filtros por categoría y búsqueda
    Vista Details: Página de detalles individuales de productos
    
    ✅ CRUD de Productos para Administradores
    Panel de Administración (Admin): Vista completa de todos los productos
    Protección por roles: Accesible para usuarios "Administrador" y "administrador"
    Crear, eliminar y editar productos.
    
    ✅ Resolución de Problemas de Autenticación
    Solución de problemas de roles (mayúsculas/minúsculas).
    Configuración correcta de cookies JWT.

Semana 5 (28/10/2025 - 04/11/2025)

     ✅ Implementación del carrito de Compras.
      Controladores, vistas y modelos añadidos
      Actualización de la cantidad del stock disponible de cada producto al añadir alguno al carrito en tiempo real.
      Interfaz intuitiva, al añadir un producto se muestra encima del icono del carrito cuantos productos añadiste.
      Relación con el sistema de pedidos.
        
     ✅ Implementación del sistema de pedidos.
      Controladores, vistas y modelos tanto como para usuario normal como para administrador.
      El administrador puede cambiar el estado del pedido que se le muestra al usuario.
      
     ✅ Rediseño de las vistas.
      Para darle un toque más tradicional, de manera que entone mejor con mi idea de una panadería rústica.
      
     ✅ Correción de problemas relacionados al stock y su actualización en tiempo real.
     (Los problemas y soluciones vienen detallados en el commit de esta semana).

Semana 6 (05/11/2025 - 11/11/2025)

     ✅ Expiración de reservas: Cada 5 minutos el se buscan carritos con más de 30 minutos sin confirmar el pedido y libera el stock del producto que han seleccionado.

     ✅ Notificaciones visuales:
        En la vista del carrito aparecerán advertencias cuando queden menos de 15 minutos.
        Aparecerá un puntito rojo a modo de notificación encima del icono del carrito cuando haya productos en el carrito que estén próximos a expirar y ser liberados.
        Cuando los productos hayan expirado del carrito y sean liberados también se le informará al usuario con una notificación dentro de la vista del carrito "Tus productos expiraron", además 
        se mantendrá el putito rojo encima del carrito durante 3 minutos o cuando el usuario le de a la "x" a la notificación.
        
     ✅ Implementación de una vista solamente para administradores en la que pueden consultar gráficas de los productos más vendidos y diferentes datos de utilidad sobre pedidos y su actual estado.

     ✅ Modificada la vista de administración de pedidos, añadí la función de poder eliminar productos.
     
     ✅ Eliminada la vista de Test DB porque ya no es necesaria. (era solamente para pruebas).

     ✅ Actualizada la vista de "Mis Pedidos" con la información a tiempo real de cada pedido y en qué estado se encuenta.

     ✅ Añadido el script de generación .sql de la base de datos actualizada en la carpeta /database.
     
     ✅ Corregido un error que, al intentar cambiar el estado de un pedido desde la vista de amdministradores no dejaba cambiarlo.
         -Prompt enviado a la IA para solucionar el problema: Tengo errores en el panel de administración al modificar el estado de un pedido,
         me salta un mensaje de alerta de localhost: Error: Error al actualizar estado.
         
         -Solución: 
          Creada clase ActualizarEstadoRequest para recibir el JSON.
          Añadida validación de datos antes de procesar: Verifica que request no sea null,  que PedidoId sea válido, que NuevoEstado no esté vacío.
          Eliminado el token antiforgery innecesario.
          
Periodo Final de Entrega (11/11/2025 - 15/11/2025)

     ✅ Cambios en la estética para el header y el footer, añadí iconos, efectos hover al pasar por encima de los encabezados, footer con enlaces al resto de las vistas.

     ✅ Creación de un logo personalizado (logo-pan.svg) para el header al lado de DePan.
        
     ✅ Añadí la vista de Quiénes Somos, en la cual añadi varios miembros del equipo de la panadería en formato "cards", integración de google maps con la ubicación física de la panadería y mapa interactivo dentro de la vista. También añadí una sección debajo de la ubicación con 3 reseñas directamente sacadas de google maps y con un botón que te lleva a todas las reseñas en google maps reviews.
     
     ✅ Añadí información útil (detalles del pedido y de a que email se ha enviado el correo de confirmación de pedido) después de confirmar un pedido a la misma vista.
     
     ✅ Añadí que se enviara después de que el usuario realizase un pedido, una confirmación por email de los detalles del mismo. El email al que se envía el correo es el mismo con el que se registra el usuario. Para ello utilicé el servicio EmailService con MailKit 4.14.1 y el protocolo SMTP de gamil con TLS (smtp.gmail.com:587). Paquete NuGet necesario: MailKit 4.14.1 , la integración del servicio de correo se ha hecho en PedidosController.cs

     ✅ Corregido un problema a la hora de enviar emails: directamente no se enviaba el correo, investigué preguntándole a la IA y es porque necesitaba tener activada la verificación en 2 pasos desde la cuenta de correo (gmail en este caso) desde la que quiero enviar los correos.

     ✅ Añadido feedback para usuario no registrados/loggeados, de manera que si intentan acceder a su carrito o meter productos en el, se les informe de que deben estar loggeados para hacerlo y les reenvia directamente a la vista de registrase / iniciar sesión.

     ✅ Corregido un error que no actualizaba el estado del producto correctamente en el catálogo ni en los detalles del producto, cuando el usuario compraba la última unidad del stock de ese producto no dejaba comprar más (funcionamiento correcto), pero seguía apareciendo la etiqueta verde como si el producto siguiese disponible, cree una función que verificase el stock después de que el usuario presionase el botón de añadir al carrito.

     ✅ Corregido una funcionalidad: cuando el usuario añade productos desde "Detalles del pedido" antes si añadía 3 por ejemplo, el contador de selección de productos se mantenía en 3, ahora automáticamente cree una función que actualiza ese contador cada vez que se le da al botón de añadir al carrito para que bajase a 1 de nuevo.

     ✅ Añadí un botón de añadir directamente al carrito desde el catálogo de productos, para usuarios habituales que no necesitan entrar en detalles de cada producto.

     ✅ Modifiqué la gráfica de evolución temporal de ventas para que solamente mostrase el mes actual y los pedidos realizados por día. De esta manera el usuario administrador puede anticipar el impacto en el stock por diferentes días del mes de un vistazo a la gráfica.

     ✅ Añadí tooltips explicativos a las 4 secciones dentro de el apartado de reportes avanzados.
     
     ✅ Añadí verificación de campos en la vista de confirmar pedido, para que el campo de teléfono sean exactamente 9 dígitos, el campo de código postal 5 dígitos, el campo de dirección y ciudad obligatorios.
     
    

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

    
