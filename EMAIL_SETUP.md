# Sistema de Envío de Correos Electrónicos - DePan

## ✅ Estado Actual
**IMPLEMENTADO** - Sistema de correo configurado con Gmail SMTP usando MailKit.

## 📧 Configuración Actual

El sistema está configurado para usar Gmail SMTP. Los correos se envían automáticamente cuando se confirma un pedido.

## 🔧 Pasos para Activar

### 1. Habilitar Contraseña de Aplicación en Gmail

1. **Ve a tu cuenta de Gmail** (denisfernandezcastro12@gmail.com)
2. **Activa la Verificación en 2 pasos:**
   - Ve a [myaccount.google.com/security](https://myaccount.google.com/security)
   - Busca "Verificación en dos pasos"
   - Actívala si no lo está

3. **Genera una Contraseña de Aplicación:**
   - Ve a [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords)
   - Selecciona "Correo" como aplicación
   - Selecciona "Windows" como dispositivo
   - Haz clic en "Generar"
   - **Guarda la contraseña de 16 caracteres** (algo como: `abcd efgh ijkl mnop`)

### 2. Actualizar appsettings.Development.json

Abre `appsettings.Development.json` y reemplaza `tu-contraseña-de-aplicacion-aqui` con la contraseña que generaste:

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderEmail": "denisfernandezcastro12@gmail.com",
  "SenderName": "DePan Panadería Artesanal",
  "Password": "abcd efgh ijkl mnop"  // <- Reemplaza con tu contraseña
}
```

### 3. Reiniciar la Aplicación

Detén y vuelve a ejecutar la aplicación para que cargue la nueva configuración.

## 📬 Cómo Funciona

1. **Un cliente hace un pedido** en el checkout
2. **Se crea el pedido** en la base de datos
3. **Se envía automáticamente** un correo al email del cliente con:
   - Número de pedido
   - Lista de productos
   - Total y dirección de entrega
   - Fecha estimada de entrega
4. **El correo tiene el diseño** de la panadería (colores marrones y dorados)

## 🧪 Cómo Probar

1. **Configura la contraseña** en appsettings.Development.json
2. **Reinicia la aplicación**
3. **Haz un pedido** como cliente
4. **Revisa tu bandeja de entrada** del email con el que te registraste
5. **Verás el correo** con todos los detalles del pedido

## 📋 Contenido del Correo

El correo incluye:
- ✅ Confirmación visual con icono verde
- 📋 Número de pedido y fecha
- 🛒 Tabla con todos los productos
- 💰 Subtotal, gastos de envío y total
- 📍 Dirección de entrega completa
- 🚚 Fecha estimada de entrega
- 📞 Información de contacto de la panadería

## ⚠️ Solución de Problemas

### El correo no se envía

**Verifica:**
1. ✅ La contraseña de aplicación está correctamente copiada
2. ✅ No hay espacios extra en la contraseña
3. ✅ La verificación en 2 pasos está activada en Gmail
4. ✅ La aplicación se reinició después de configurar

### Error de autenticación

- Verifica que estás usando una **contraseña de aplicación**, NO tu contraseña normal de Gmail
- Genera una nueva contraseña de aplicación si la anterior no funciona

### El correo llega a spam

- Es normal en las primeras veces
- Gmail aprenderá con el tiempo que es correo legítimo
- Puedes marcar como "No es spam" en Gmail

## 📊 Límites de Gmail

- **500 correos por día** (más que suficiente para una panadería)
- Gratis
- Confiable

## 🎨 Personalización

El correo ya está diseñado con los colores de la panadería. Si quieres modificar el diseño, edita:
- `Services/EmailService.cs` → método `GenerarHtmlConfirmacionPedido`

