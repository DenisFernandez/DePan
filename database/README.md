# 🗄️ Base de Datos - DePan

## Descripción

Base de datos relacional diseñada para gestionar una panadería online con sistema de pedidos, seguimiento en tiempo real y gestión de usuarios (clientes, repartidores y administradores).

## 📊 Diagrama Entidad-Relación

<img width="1222" height="546" alt="er_depandb drawio" src="https://github.com/user-attachments/assets/5e54b455-2f58-4400-ba55-92ddf24295da" />


## 🏗️ Estructura de la Base de Datos

### Tablas Principales

| Tabla | Descripción | Registros Clave |
|-------|-------------|-----------------|
| **USUARIO** | Gestión de clientes, repartidores y administradores | rol, email (UK) |
| **CATEGORIA** | Clasificación de productos (Pan, Bollería, Repostería) | nombre (UK) |
| **PRODUCTO** | Catálogo de productos con precios y stock | nombre, precio, stock |
| **CARRITO** | Carrito de compra persistente por usuario | id_usuario (UK) |
| **LINEA_CARRITO** | Productos añadidos al carrito | id_carrito, id_producto |
| **PEDIDO** | Pedidos realizados por clientes | numero_pedido (UK), estado |
| **LINEA_PEDIDO** | Detalle de productos en cada pedido | id_pedido, id_producto |
| **SEGUIMIENTO_PEDIDO** | Historial de estados y ubicación en tiempo real | latitud, longitud |
| **VALORACION** | Opiniones de clientes sobre productos | puntuacion (1-5) |

### Relaciones Clave

- Un **USUARIO** (cliente) puede tener muchos **PEDIDOS**
- Un **USUARIO** (repartidor) puede ser asignado a muchos **PEDIDOS**
- Un **PEDIDO** tiene múltiples **SEGUIMIENTOS** para trazabilidad
- Un **PRODUCTO** pertenece a una **CATEGORIA**
- Un **CARRITO** pertenece a un único **USUARIO**

## ⚙️ Características Avanzadas

### Triggers Automáticos

1. **Actualización de totales del carrito**
   - Se ejecuta al insertar, actualizar o eliminar líneas del carrito
   - Mantiene sincronizado el campo `total` automáticamente

2. **Registro de cambios de estado**
   - Cada cambio de estado en PEDIDO genera un registro en SEGUIMIENTO_PEDIDO
   - Permite auditoría completa del ciclo de vida del pedido

3. **Seguimiento inicial automático**
   - Al crear un pedido, se genera automáticamente el primer registro de seguimiento

### Vistas Útiles

- `v_pedidos_completos`: Información completa de pedidos con datos de cliente y repartidor
- `v_productos_mas_vendidos`: Estadísticas de ventas por producto
- `v_pedidos_en_curso`: Pedidos activos con última ubicación conocida

### Procedimientos Almacenados

- `sp_crear_pedido_desde_carrito`: Convierte el carrito en pedido, copia las líneas y vacía el carrito

## 🚀 Instalación

### Requisitos Previos

- MySQL 8.4 LTS o superior
- Cliente MySQL (mysql-client) o MySQL Workbench

### Paso 1: Crear la Base de Datos

```bash
# Desde la línea de comandos
mysql -u root -p < schema.sql
```

O desde **MySQL Workbench**:
1. Abrir MySQL Workbench
2. Conectar al servidor
3. File → Open SQL Script → Seleccionar `schema.sql`
4. Ejecutar (icono del rayo ⚡)

### Paso 2: Verificar la Instalación

```sql
USE depan_db;
SHOW TABLES;

-- Debería mostrar 9 tablas:
-- CARRITO, CATEGORIA, LINEA_CARRITO, LINEA_PEDIDO, 
-- PEDIDO, PRODUCTO, SEGUIMIENTO_PEDIDO, USUARIO, VALORACION
```

### Paso 3: Verificar Datos de Prueba

```sql
-- Ver usuarios de ejemplo
SELECT email, rol FROM USUARIO;

-- Ver productos
SELECT nombre, precio, stock FROM PRODUCTO;

-- Ver categorías
SELECT * FROM CATEGORIA;
```

## 📝 Datos de Prueba (Seed Data)

El script incluye datos iniciales:

### Usuarios por Defecto

| Email | Contraseña | Rol |
|-------|------------|-----|
| admin@depan.com | (ver nota) | administrador |
| repartidor@depan.com | (ver nota) | repartidor |
| cliente@ejemplo.com | (ver nota) | cliente |

> ⚠️ **IMPORTANTE**: Las contraseñas en el script son hashes de ejemplo. En tu aplicación deberás usar BCrypt para generar hashes reales.


## 🔐 Seguridad

### Buenas Prácticas Implementadas

✅ Contraseñas hasheadas con BCrypt (debes implementar en C#)
✅ Email único por usuario
✅ Validaciones con CHECK constraints
✅ Foreign keys con políticas de eliminación apropiadas
✅ Índices en campos de búsqueda frecuente

### Configuración de Conexión Segura

**NO** incluyas las credenciales reales en el código. Usa variables de entorno:

```csharp
// appsettings.json (NO subir a GitHub)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=depan_db;User=usuario;Password=contraseña;"
  }
}
```

## 📈 Normalización

La base de datos cumple con la **Tercera Forma Normal (3FN)**:

- ✅ Cada tabla tiene clave primaria
- ✅ No hay dependencias parciales
- ✅ No hay dependencias transitivas
- ✅ Datos históricos preservados (snapshot de precios/nombres en pedidos)

## 🔧 Mantenimiento

### Respaldo de la Base de Datos

```bash
# Crear backup completo
mysqldump -u root -p depan_db > backup_depan_$(date +%Y%m%d).sql

# Restaurar desde backup
mysql -u root -p depan_db < backup_depan_20251006.sql
```

### Limpieza de Datos Antiguos

```sql
-- Eliminar carritos abandonados (más de 30 días sin actualizar)
DELETE FROM CARRITO 
WHERE fecha_actualizacion < DATE_SUB(NOW(), INTERVAL 30 DAY);

-- Archivar pedidos antiguos entregados (más de 1 año)
-- (implementar según necesidades)
```

## 📊 Consultas Útiles

### Productos más vendidos del mes

```sql
SELECT 
    p.nombre,
    SUM(lp.cantidad) as unidades_vendidas,
    SUM(lp.subtotal) as ingresos
FROM PRODUCTO p
INNER JOIN LINEA_PEDIDO lp ON p.id_producto = lp.id_producto
INNER JOIN PEDIDO pe ON lp.id_pedido = pe.id_pedido
WHERE pe.fecha_pedido >= DATE_SUB(NOW(), INTERVAL 1 MONTH)
GROUP BY p.id_producto, p.nombre
ORDER BY unidades_vendidas DESC
LIMIT 10;
```

### Pedidos pendientes de asignar repartidor

```sql
SELECT 
    numero_pedido,
    fecha_pedido,
    total,
    ciudad_entrega
FROM PEDIDO
WHERE id_repartidor IS NULL 
  AND estado = 'pendiente'
ORDER BY fecha_pedido ASC;
```

### Clientes con más pedidos

```sql
SELECT 
    u.nombre,
    u.apellidos,
    COUNT(p.id_pedido) as total_pedidos,
    SUM(p.total) as gasto_total
FROM USUARIO u
INNER JOIN PEDIDO p ON u.id_usuario = p.id_usuario_cliente
GROUP BY u.id_usuario, u.nombre, u.apellidos
ORDER BY total_pedidos DESC
LIMIT 10;
```

## 🐛 Solución de Problemas

### Error: "Access denied for user"

```bash
# Verificar permisos del usuario
mysql -u root -p
GRANT ALL PRIVILEGES ON depan_db.* TO 'tu_usuario'@'localhost';
FLUSH PRIVILEGES;
```

### Error: "Table doesn't exist"

```bash
# Verificar que estás en la base de datos correcta
USE depan_db;
SHOW TABLES;
```

### Error en triggers

```sql
-- Ver triggers existentes
SHOW TRIGGERS FROM depan_db;

-- Si hay problemas, puedes eliminarlos y recrearlos
DROP TRIGGER IF EXISTS trg_actualizar_total_carrito_insert;
-- Luego ejecuta de nuevo la parte de triggers del schema.sql
```

## 📚 Recursos Adicionales

- [Documentación MySQL 8.4](https://dev.mysql.com/doc/refman/8.4/en/)
- [Mejores prácticas de diseño de BD](https://dev.mysql.com/doc/)
- [Guía de normalización](https://en.wikipedia.org/wiki/Database_normalization)

---
**Autor**: Denis Fernández Castro
