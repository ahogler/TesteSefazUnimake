using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Servicos.NFe;
using Unimake.Business.DFe.Xml.NFe;
using Unimake.Security.Platform;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static string CaminhoCertificadoDigital { get; set; } = "C:\\Cert\\ptca.pfx";
        private static string SenhaCertificadoDigital { get; set; } = "36878535";

        private static X509Certificate2 CertificadoSelecionadoField;

        private static X509Certificate2 CertificadoSelecionado
        {
            get 
            {
                if (CertificadoSelecionadoField == null)
                {
                    CertificadoSelecionadoField = new CertificadoDigital().CarregarCertificadoDigitalA1(CaminhoCertificadoDigital, SenhaCertificadoDigital);
                }
                return CertificadoSelecionadoField;
            }
        }

        [HttpGet]
        [Route("StatusServico")]
        public ActionResult<string> StatusServico()
        {
            //var xml = new ConsStatServ
            //{
            //    Versao = "4.00",
            //    CUF = UFBrasil.SC,
            //    TpAmb = TipoAmbiente.Homologacao
            //};

            //var configuracao = new Configuracao
            //{
            //    TipoDFe = TipoDFe.NFe,
            //    TipoEmissao = TipoEmissao.Normal,
            //    CertificadoDigital = CertificadoSelecionado
            //};

            //var statusServico = new StatusServico(xml, configuracao);
            //return Ok(statusServico.Result.CStat + " " + statusServico.Result.XMotivo);

            var xml = new ConsSitNFe
            {
                Versao = "4.00",
                ChNFe = "42220736878535000107550010000000021000000030",
                TpAmb = TipoAmbiente.Producao
            };

            var configuracao = new Configuracao
            {
                TipoDFe = TipoDFe.NFe,
                TipoEmissao = TipoEmissao.Normal,
                CertificadoDigital = CertificadoSelecionado
            };
            try
            {
                var statusServico = new ConsultaProtocolo(xml, configuracao);
                statusServico.Executar();

                return Ok(statusServico.Result.CStat + " " + statusServico.Result.XMotivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpGet]
        [Route("TesteConexao")]
        public ActionResult<string> TesteConexao()
        {
            return Ok("Está rodando");
        }
    }
}