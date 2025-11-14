using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using DePan.Models;

namespace DePan.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> EnviarCorreoConfirmacionPedidoAsync(Pedido pedido, Usuario usuario)
        {
            try
            {
                _logger.LogInformation($"Iniciando envío de correo para pedido {pedido.NumeroPedido} a {usuario.Email}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _configuration["Email:SenderName"],
                    _configuration["Email:SenderEmail"]
                ));
                message.To.Add(new MailboxAddress(
                    $"{usuario.Nombre} {usuario.Apellidos}",
                    usuario.Email
                ));
                message.Subject = $"Confirmación de Pedido #{pedido.NumeroPedido} - DePan";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerarHtmlConfirmacionPedido(pedido, usuario)
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _configuration["Email:SmtpHost"],
                        int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
                        SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(
                        _configuration["Email:SenderEmail"],
                        _configuration["Email:Password"]
                    );

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation($"✅ Correo enviado exitosamente a {usuario.Email} para pedido {pedido.NumeroPedido}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ ERROR enviando correo: {ex.Message}");
                _logger.LogError($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                }

                return false;
            }
        }

        private string GenerarHtmlConfirmacionPedido(Pedido pedido, Usuario usuario)
        {
            var lineasHtml = "";
            var index = 0;
            foreach (var linea in pedido.LineaPedidos)
            {
                var bgColor = index % 2 == 0 ? "#faf8f5" : "#ffffff";
                lineasHtml += $@"
                    <tr style='background-color: {bgColor};'>
                        <td style='padding: 14px 10px; color: #333; font-size: 14px;'>{linea.NombreProducto}</td>
                        <td style='padding: 14px 10px; text-align: center; color: #666; font-size: 14px;'>{linea.Cantidad}</td>
                        <td style='padding: 14px 10px; text-align: right; color: #666; font-size: 14px;'>{linea.PrecioUnitario:C}</td>
                        <td style='padding: 14px 10px; text-align: right; font-weight: 600; color: #7a4e31; font-size: 15px;'>{linea.Subtotal:C}</td>
                    </tr>";
                index++;
            }

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='font-family: ""Segoe UI"", Roboto, ""Helvetica Neue"", Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f5f5f5;'>
    <div style='max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.15);'>
        
        <!-- Header -->
        <div style='background: linear-gradient(to bottom, #ffffff 0%, #faf8f5 100%); padding: 25px 20px; text-align: center; border-bottom: 3px solid #d4a574;'>
            <svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 120 60' width='80' height='40' style='margin-bottom: 10px;'>
                <defs>
                    <filter id='rough'>
                        <feTurbulence type='fractalNoise' baseFrequency='0.05' numOctaves='3' result='noise'/>
                        <feDisplacementMap in='SourceGraphic' in2='noise' scale='1.5'/>
                    </filter>
                </defs>
                <ellipse cx='60' cy='30' rx='55' ry='20' fill='#d4a574' stroke='#8b5e3c' stroke-width='2' filter='url(#rough)'/>
                <ellipse cx='60' cy='35' rx='50' ry='15' fill='#c4955f' opacity='0.6'/>
                <path d='M 25 25 Q 25 28 25 32' stroke='#8b5e3c' stroke-width='2.5' fill='none' stroke-linecap='round'/>
                <path d='M 40 20 Q 40 24 40 30' stroke='#8b5e3c' stroke-width='2.5' fill='none' stroke-linecap='round'/>
                <path d='M 55 18 Q 55 23 55 30' stroke='#8b5e3c' stroke-width='2.5' fill='none' stroke-linecap='round'/>
                <path d='M 70 18 Q 70 23 70 30' stroke='#8b5e3c' stroke-width='2.5' fill='none' stroke-linecap='round'/>
                <path d='M 85 20 Q 85 24 85 30' stroke='#8b5e3c' stroke-width='2.5' fill='none' stroke-linecap='round'/>
                <circle cx='30' cy='35' r='1.5' fill='#8b5e3c'/>
                <circle cx='45' cy='33' r='1.5' fill='#8b5e3c'/>
                <circle cx='75' cy='33' r='1.5' fill='#8b5e3c'/>
                <circle cx='90' cy='35' r='1.5' fill='#8b5e3c'/>
            </svg>
            <h1 style='margin: 5px 0 0 0; font-size: 32px; color: #7a4e31; font-weight: 600;'>DePan</h1>
            <p style='margin: 5px 0 0 0; font-size: 14px; color: #8b5e3c; font-weight: 500; letter-spacing: 1px;'>PANADERÍA ARTESANAL</p>
        </div>

        <!-- Contenido -->
        <div style='padding: 35px 25px;'>
            <div style='text-align: center; margin-bottom: 35px;'>
                <div style='display: inline-block; background: linear-gradient(135deg, #28a745, #20c997); color: white; width: 70px; height: 70px; border-radius: 50%; line-height: 70px; font-size: 36px; margin-bottom: 15px; box-shadow: 0 4px 8px rgba(40, 167, 69, 0.3);'>✓</div>
                <h2 style='color: #28a745; margin: 10px 0; font-size: 28px; font-weight: 600;'>¡Pedido Confirmado!</h2>
                <p style='color: #666; margin: 5px 0; font-size: 16px;'>Gracias por tu compra, <strong style='color: #7a4e31;'>{usuario.Nombre}</strong></p>
            </div>

            <!-- Info del pedido -->
            <div style='background: linear-gradient(135deg, #e7f5ff, #d0ebff); border-left: 5px solid #0056b3; padding: 20px; margin-bottom: 25px; border-radius: 8px; box-shadow: 0 2px 6px rgba(0, 86, 179, 0.1);'>
                <p style='margin: 0; font-size: 20px; color: #0056b3; font-weight: 600;'>Número de Pedido: {pedido.NumeroPedido}</p>
                <p style='margin: 8px 0 0 0; color: #555; font-size: 14px;'>📅 Fecha: {pedido.FechaPedido:dd/MM/yyyy HH:mm}</p>
            </div>

            <!-- Productos -->
            <h3 style='color: #7a4e31; margin: 30px 0 18px 0; padding-bottom: 12px; border-bottom: 3px solid #d4a574; font-size: 22px; font-weight: 600;'>🥖 Productos del Pedido</h3>
            <table style='width: 100%; border-collapse: collapse; margin-bottom: 25px; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                <thead>
                    <tr style='background: linear-gradient(to bottom, #7a4e31, #8b5e3c); color: white;'>
                        <th style='padding: 14px 10px; text-align: left; font-weight: 600; font-size: 14px;'>Producto</th>
                        <th style='padding: 14px 10px; text-align: center; font-weight: 600; font-size: 14px;'>Cant.</th>
                        <th style='padding: 14px 10px; text-align: right; font-weight: 600; font-size: 14px;'>Precio</th>
                        <th style='padding: 14px 10px; text-align: right; font-weight: 600; font-size: 14px;'>Subtotal</th>
                    </tr>
                </thead>
                <tbody>
                    {lineasHtml}
                </tbody>
            </table>

            <!-- Totales -->
            <div style='background: linear-gradient(135deg, #faf8f5, #f5f5dc); padding: 20px; border-radius: 8px; border: 2px solid #d4a574; box-shadow: 0 2px 8px rgba(212, 165, 116, 0.2);'>
                <div style='margin-bottom: 10px; color: #555; font-size: 15px;'>
                    <span style='float: left;'>Subtotal:</span>
                    <span style='float: right; font-weight: 600; color: #333;'>{pedido.Subtotal:C}</span>
                    <div style='clear: both;'></div>
                </div>
                <div style='margin-bottom: 12px; color: #555; font-size: 15px;'>
                    <span style='float: left;'>Gastos de Envío:</span>
                    <span style='float: right; font-weight: 600; color: #333;'>{pedido.GastosEnvio:C}</span>
                    <div style='clear: both;'></div>
                </div>
                <div style='padding-top: 12px; border-top: 2px solid #7a4e31; font-size: 22px; font-weight: 700; color: #28a745;'>
                    <span style='float: left;'>Total:</span>
                    <span style='float: right;'>{pedido.Total:C}</span>
                    <div style='clear: both;'></div>
                </div>
            </div>

            <!-- Dirección de entrega -->
            <h3 style='color: #7a4e31; margin: 30px 0 18px 0; padding-bottom: 12px; border-bottom: 3px solid #d4a574; font-size: 22px; font-weight: 600;'>📍 Dirección de Entrega</h3>
            <div style='background: linear-gradient(135deg, #ffffff, #faf8f5); padding: 20px; border-radius: 8px; border: 1px solid #d4a574; box-shadow: 0 2px 6px rgba(0,0,0,0.05);'>
                <p style='margin: 8px 0; font-size: 16px; color: #333;'><strong style='color: #7a4e31;'>🏠 </strong>{pedido.DireccionEntrega}</p>
                <p style='margin: 8px 0; font-size: 15px; color: #555;'><strong style='color: #7a4e31;'>📮 </strong>{pedido.CiudadEntrega}, {pedido.CodigoPostalEntrega}</p>
                <p style='margin: 8px 0; font-size: 15px; color: #555;'><strong style='color: #7a4e31;'>📞 </strong>{pedido.TelefonoContacto}</p>
            </div>

            {(pedido.FechaEntregaEstimada.HasValue ? $@"
            <!-- Entrega estimada -->
            <div style='background: linear-gradient(135deg, #fff8e1, #ffe082); padding: 20px; border-radius: 8px; margin-top: 25px; text-align: center; border: 2px solid #ffb300; box-shadow: 0 2px 8px rgba(255, 179, 0, 0.2);'>
                <p style='margin: 0; color: #f57c00; font-weight: 600; font-size: 18px;'>🚚 Entrega Estimada</p>
                <p style='margin: 8px 0 0 0; font-size: 20px; color: #333; font-weight: 700;'>{pedido.FechaEntregaEstimada.Value:dd/MM/yyyy}</p>
            </div>" : "")}
        </div>

        <!-- Footer -->
        <div style='background: linear-gradient(to bottom, #7a4e31, #6b4423); color: #f5f5dc; padding: 25px 20px; text-align: center; font-size: 13px;'>
            <p style='margin: 0 0 12px 0; font-size: 15px; color: #d4a574; font-weight: 600;'>DePan - Panadería Artesanal</p>
            <p style='margin: 0 0 8px 0;'>📍 Lugar Montalvo 3, 18 - 36979 Sanxenxo, Pontevedra</p>
            <p style='margin: 0 0 8px 0;'>📞 986 724 498 | 📧 info@depan.com</p>
            <p style='margin: 0; color: #d4a574; font-style: italic;'>Lunes-Viernes: 7:00-14:00 | 17:00-20:00 | Sábados: 7:00-14:00</p>
            <p style='margin: 15px 0 0 0; padding-top: 15px; border-top: 1px solid #8b5e3c; color: #c4955f; font-size: 11px;'>© 2025 DePan. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
