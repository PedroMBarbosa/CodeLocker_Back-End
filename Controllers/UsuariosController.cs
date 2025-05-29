using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBCodeLockerContext _context;
        private readonly string qrCodeDirectory = Path.Combine(Directory.GetCurrentDirectory(), "QRCodeImagens");

        public UsuariosController(DBCodeLockerContext context)
        {
            _context = context;
        }

        [HttpPost("CadastrarUsuario")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            try
            {
                // Verifica se já existe um cliente com o mesmo email
                if (_context.usuario.Any(u => u.email == usuario.email))
                    return BadRequest(new { message = "Usuário já existe!" });

                // Primeiro salva o usuário para gerar o ID
                _context.usuario.Add(usuario);
                _context.SaveChanges();

                // Gera QR Code com o código personalizado: "7920" + usuario.id + "099"
                if (!Directory.Exists(qrCodeDirectory))
                    Directory.CreateDirectory(qrCodeDirectory);

                string qrContent = $"7920{usuario.id}099";

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q))
                using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData))
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    string fileName = $"{Guid.NewGuid()}.png";
                    string filePath = Path.Combine(qrCodeDirectory, fileName);
                    qrCodeImage.Save(filePath, ImageFormat.Png);
                    usuario.qrcode = $"QRCodeImagens/{fileName}";
                }

                // Atualiza o usuário para salvar o caminho do QR code
                _context.SaveChanges();

                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Erro ao registrar usuário.",
                    erro = ex.InnerException?.Message ?? ex.Message,
                    stack = ex.StackTrace
                });
            }
        }

        [HttpDelete("DeletarUsuario/{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var usuario = _context.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound(new{message="não foi possivel encontrar o usuário"});
            }
            _context.usuario.Remove(usuario);
            _context.SaveChanges();

            return Ok(new{message="usuário deletado com sucesso"});
        }
        [HttpPut("EditarUsuario/{id}")]
        public IActionResult EditarUsuario(int id, [FromBody] Usuario usuarioAtualizado)
        {
            var usuario = _context.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            usuario.nome = usuarioAtualizado.nome;
            usuario.email = usuarioAtualizado.email;
            usuario.telefone = usuarioAtualizado.telefone;
            usuario.senha = usuarioAtualizado.senha;

            _context.SaveChanges();

            return Ok(new { message = "Usuário atualizado com sucesso." });
            
        }
        [HttpGet("ListarUsuarios")]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _context.usuario.ToList();
            return Ok(usuarios);
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                var usuario = _context.usuario.FirstOrDefault(u =>
                    u.email.Trim().ToLower() == login.email.Trim().ToLower() &&
                    u.senha.Trim() == login.senha.Trim());

                if (usuario == null)
                {
                    return Unauthorized(new { message = "E-mail ou senha inválidos." });
                }

                return Ok(new
                {
                    message = "Login realizado com sucesso!",
                    usuario = new
                    {
                        usuario.id,
                        usuario.nome,
                        usuario.email,
                        usuario.telefone,
                        usuario.qrcode
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Erro ao tentar fazer login.",
                    erro = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


    }
}
