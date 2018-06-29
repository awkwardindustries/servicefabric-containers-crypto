using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ContainerCrypto.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class CryptoController : Controller
    {
        private IStringCryptoService _stringCryptoService;

        public CryptoController(IStringCryptoService stringCryptoService)
        {
            _stringCryptoService = stringCryptoService;
        }

        [HttpPost]
        [Route("encrypt")]
        public IActionResult Encrypt([FromBody] string textToEncrypt)
        {
            try
            {
                var encryptedText = _stringCryptoService.Encrypt(textToEncrypt);
                return Ok(encryptedText);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("decrypt")]
        public IActionResult Decrypt([FromBody] string base64EncodedTextToDecrypt)
        {
            try
            {
                var decryptedText = _stringCryptoService.Decrypt(base64EncodedTextToDecrypt);
                return Ok(decryptedText);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}