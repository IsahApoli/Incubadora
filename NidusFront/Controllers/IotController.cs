using Microsoft.AspNetCore.Mvc;
using System;
using NidusFront.Models;
using NidusFront.DAOs;

namespace NidusFront.Controllers
{
    public class IotController : Controller
    {
        private readonly TelemetriaDAO _dao;
        private readonly FiwareService _fiware;

        public IotController()
        {
            _dao = new TelemetriaDAO();
            _fiware = new FiwareService();
        }

        // =====================================================================
        // ENDPOINT DE TELEMETRIA
        // O ESP32 faz POST em "https://seusite.com/Iot/ReceberDados"
        // =====================================================================
        [HttpPost]
        public IActionResult ReceberDados([FromBody] TelemetriaViewModel model)
        {
            try
            {
                if (model == null || model.IdIncubadora == 0)
                    return BadRequest(new { erro = "Dados de telemetria inválidos ou ID da Incubadora ausente." });

                if (model.DataHora == DateTime.MinValue)
                    model.DataHora = DateTime.Now;

                _dao.Insert(model);

                return Ok(new { sucesso = true, mensagem = "Dados gravados na Nidus!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Falha interna no servidor", detalhe = ex.Message });
            }
        }

        // =====================================================================
        // ENDPOINT FIWARE — LIGAR INCUBADORA
        // Chamado pelo JavaScript: POST /api/fiware/ligar
        // =====================================================================
        [HttpPost]
        [Route("api/fiware/ligar")]
        public IActionResult Ligar()
        {
            try
            {
                _fiware.EnviarComando(ligar: true);
                return Ok(new { sucesso = true, mensagem = "Comando LIGAR enviado ao ESP32." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Falha ao enviar comando LIGAR", detalhe = ex.Message });
            }
        }

        // =====================================================================
        // ENDPOINT FIWARE — DESLIGAR INCUBADORA
        // Chamado pelo JavaScript: POST /api/fiware/desligar
        // =====================================================================
        [HttpPost]
        [Route("api/fiware/desligar")]
        public IActionResult Desligar()
        {
            try
            {
                _fiware.EnviarComando(ligar: false);
                return Ok(new { sucesso = true, mensagem = "Comando DESLIGAR enviado ao ESP32." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Falha ao enviar comando DESLIGAR", detalhe = ex.Message });
            }
        }
    }
}